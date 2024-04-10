using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.Networking;
using TMPro;
using System.IO; // For UnityWebRequest

public class CardController : MonoBehaviour
{
    [SerializeField] private FadeMaterial m_FadeMaterial;
    [SerializeField] private Toggle m_PassthroughToggle;
    [SerializeField] private GameObject m_ControllerPositionGUI;
    [SerializeField] private Canvas m_ColorTestCanvas;
    [SerializeField] private Canvas m_StartupMenu;
    [SerializeField] private ControllerPositionGUI m_controllerPositonObject;
    [SerializeField] private Transform m_Camera;
    [SerializeField] private Transform m_CardPrefab;


    public Image slide_Image;
    private int currentSlideIndex = 1; //Current slide number. range [1 - folder amount]
    private int currentFolderIndex = 0; //Current folder index [0-3]

    private List<string> folderNames = new List<string>();
    private List<int> slideCounts = new List<int>();

    private string folderRoot = string.Empty;
   
    private bool passthroughMode = false;
    private bool slideVisible = true;
    private bool colorVisible = false;
    private ArrowKeys playerInputActions;
    private int startupState = 0;

    private void Awake() {
        playerInputActions = new ArrowKeys();
        playerInputActions.Keyboard.Enable();
        playerInputActions.Keyboard.Arrows.performed += Arrows_performed;
        playerInputActions.Keyboard.LongArrows.performed += Long_Arrows_performed;
        m_ControllerPositionGUI.SetActive(true);
    }

    /// <summary>
    /// Method <c>Long_Arrows_performed</c> is an event handler for long presses of the arrow keys.
    /// </summary>
    /// <param name="context">Points back to the action input event to be handled.</param>
    private void Long_Arrows_performed(InputAction.CallbackContext context) {
        if (context.performed)
        {
            InputControl keyPressed = context.control;
            if (keyPressed.name == "upArrow")
            {
                Debug.Log("Up Arrow long-pressed!");
            }
            if (keyPressed.name == "downArrow")
            {
                Debug.Log("Down Arrow long-pressed!");
            }
            if (keyPressed.name == "leftArrow")
            {
                Debug.Log("Left Arrow long-pressed!");
            }
            if (keyPressed.name == "rightArrow")
            {
                Debug.Log("Right Arrow long-pressed!");               
            }
        }
    }

    /// <summary>
    /// Method <c>Arrows_performed</c> is an event handler for normal presses of the arrow keys.
    /// </summary>
    /// <param name="context">points back to the action input event to be handled.</param> 
    private void Arrows_performed(InputAction.CallbackContext context) {
        if (context.performed) {
            InputControl keyPressed = context.control;
            if (keyPressed.name == "upArrow") {
                Debug.Log("Up Arrow pressed!");
                TogglePassthrough();
                ToggleSlideVisibility();
            }
            if (keyPressed.name == "downArrow") {
                Debug.Log("Down Arrow pressed!");                
                // NextFolder();                
                if (colorVisible) {
                    ToggleColorVisibility();
                    // currentFolderIndex = -1;
                }
                if (!passthroughMode) {
                    NextFolder();
                }
                
            }
            if (keyPressed.name == "leftArrow") {
                Debug.Log("Left Arrow pressed!");
                if (!colorVisible && !passthroughMode) {
                    PreviousSlide();
                }
            }
            if (keyPressed.name == "rightArrow") {
                switch (startupState) {
                    case 0:
                        m_controllerPositonObject.SetLeftMinY();
                        startupState++;
                        m_StartupMenu.GetComponent<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = "Press right arrow to set Left Max Y.";
                        break;
                    case 1:
                        m_controllerPositonObject.SetLeftMaxY();
                        startupState++;
                        m_StartupMenu.GetComponent<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = "Press right arrow to set Right Min Y.";
                        break;
                    case 2:
                        m_controllerPositonObject.SetRightMinY();
                        startupState++;
                        m_StartupMenu.GetComponent<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = "Press right arrow to set Right Max Y.";
                        break;
                    case 3:
                        m_controllerPositonObject.SetRightMaxY();
                        m_StartupMenu.GetComponent<CanvasGroup>().alpha = 0;
                        this.transform.GetComponentInChildren<CanvasGroup>().alpha = 1;
                        startupState++;
                        break;                   
                    default:
                        if (!colorVisible && !passthroughMode) {
                            NextSlide();
                        }
                        break;
                }
                Debug.Log("Right Arrow pressed!");
                
            }
        }
    }

    /// <summary>
    /// Method <c>Start()</c> is called before the first frame update.  Initialize 
    /// passthrough mode, slide visibility, and set the file path for the first test
    /// folder before displaying the first slide in the first test.
    /// </summary>
    void Start()
    {
        //Determines path to use depending on device type. 
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            folderRoot = "/Assets/Resources";
        }
        else
        {
            folderRoot = "sdcard/Documents";
        }

        InitializeDirectory();

      
        passthroughMode = false;
        slideVisible = true;
        
        /*ShowSlide(currentSlideIndex, folderNames[currentFolderIndex], folderRoot);*/

        m_StartupMenu.GetComponent<CanvasGroup>().alpha = 1;
        m_StartupMenu.GetComponent<Canvas>().GetComponentInChildren<TextMeshProUGUI>().text = "Press right arrow to set Left Min Y.";
        m_ColorTestCanvas.GetComponent<CanvasGroup>().alpha = 0;
        this.transform.GetComponentInChildren<CanvasGroup>().alpha = 0;
    }

    /// <summary>
    /// Method <c>Update()</c> is called once per frame - tasks that need to be 
    /// constantly updated go here.
    /// </summary>
    void Update()
    {
        // m_CardPrefab.transform.rotation = m_Camera.transform.rotation;

    }

    /// <summary>
    /// Method <c>Showslide()</c> loads the image file Slide[slideIndex]
    /// </summary>
    /// <param name="slideIndex">The index of the current slide.</param>
    /// <param name="folderName">The name of the folder that contains the current test.</param>
    /// <param name="folderRoot"></param>
    void ShowSlide(int slideIndex, string folderName, string folderRoot)
    {

        if  (SystemInfo.deviceType == DeviceType.Desktop)

        {
            string imageName = "Slide" + slideIndex;
            string folderPath = folderName + "/" + imageName;

            //Load the image from the specified folder
            Sprite spriteImage = Resources.Load<Sprite>(folderPath); //NOTE: NEED A RESOURCES FOLDER IN ASSETS
            Debug.Log("vondoste - Folderpath: " + folderPath);

            if (spriteImage != null)
            {
                //Set the image on the card
                slide_Image.sprite = spriteImage;
            }
            else
            {
                Debug.LogError("vondoste - Image not Found: " + folderPath);
            }

        } else 
        {
            string imageName = "Slide" + slideIndex + ".jpg";
            string folderPath = System.IO.Path.Combine(folderRoot, folderName);
            string imagePath = System.IO.Path.Combine(folderPath, imageName);
            Debug.Log("vondoste - Folderpath: " + imagePath);

            // Load image from file
            byte[] imageData = System.IO.File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData);

            // Convert texture to sprite
            Sprite spriteImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            if (spriteImage != null)
            {
                // Set the image on the card
                slide_Image.sprite = spriteImage;
            }
            else
            {
                Debug.LogError("Image not Found: " + folderPath);

            }
        }
            



    }


    /// <summary>
    /// Method <c>NextSlide()</c> displays the next slide in the directory,
    /// or loops back to the first slide.
    /// </summary>
    public void NextSlide()
    {
        currentSlideIndex++;
        if (currentSlideIndex > slideCounts[currentFolderIndex])
        {
            currentSlideIndex = 1;
        }
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex], folderRoot);
    }

    /// <summary>
    /// Method <c>PreviousSlide()</c> displays the previous slide in the directory,
    /// or loops back to the last slide.
    /// </summary>
    public void PreviousSlide()
    {
        currentSlideIndex--;
        if (currentSlideIndex < 1)
        {
            currentSlideIndex = slideCounts[currentFolderIndex];
        }
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex], folderRoot);


    }

    /// <summary>
    /// Method <c>NextFolder()</c> selects the next test folder in the
    /// list, or loops back to the beginning of the list.  It also cues up 
    /// the first slide in the folder and displays it.
    /// </summary>
    public void NextFolder()
    {
        currentFolderIndex++;
        if (currentFolderIndex >= folderNames.Count)
        {
            currentFolderIndex = -1;
            //ToggleSlideVisibility();
            ToggleColorVisibility();
            return;
        }

        currentSlideIndex = 1;
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex], folderRoot);

    }

    /// <summary>
    /// Method <c>PreviousFolder()</c> selects the previous test folder in the
    /// list, or loops back to the end of the list.  It also cues up 
    /// the first slide in the folder and displays it.
    /// *** currently not being used ***
    /// </summary>
    public void PreviousFolder()
    {
        currentFolderIndex--;
        if (currentFolderIndex < 0)
        {
            currentFolderIndex = folderNames.Count - 1;
        }
        currentSlideIndex = 1;
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex], folderRoot);

    }

    /// <summary>
    /// Method <c>TogglePassthrough</c> sets the FadeSkybox attribute to the opposite state
    /// of the boolean variable passthroughMode, then toggles the variable.
    /// </summary>
    public void TogglePassthrough()
    {
        if (passthroughMode)
        {
            m_FadeMaterial.FadeSkybox(false);
            m_ControllerPositionGUI.SetActive(true);
            passthroughMode = false;
        }
        else
        {
            m_FadeMaterial.FadeSkybox(true);
            m_ControllerPositionGUI.SetActive(false);
            passthroughMode = true;
        }
    }

    /// <summary>
    /// Method <c>ToggleSlideVisibility</c> reads slideVisible, and if true, 
    /// sets the alpha attribute of the CanvasGroup component on this object 
    /// to 0 to make the slide canvas invisible, then sets canvasVisible to false.
    /// If slideVisible is false, it sets the alpha attribute to 1 to make the 
    /// canvas visible, and toggles slideVisible.
    /// </summary>
    public void ToggleSlideVisibility()
    {
        if (slideVisible)
        {
            this.transform.GetComponentInChildren<CanvasGroup>().alpha = 0;
            slideVisible = false;
            // colorVisible = false;
            m_ColorTestCanvas.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            slideVisible = true;
            if (colorVisible) {
                this.transform.GetComponentInChildren<CanvasGroup>().alpha = 0;                
                // colorVisible = false;
                m_ColorTestCanvas.GetComponent<CanvasGroup>().alpha = 1;
            } else {
                this.transform.GetComponentInChildren<CanvasGroup>().alpha = 1;                
                // colorVisible = false;
                m_ColorTestCanvas.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    /// <summary>
    /// Method <c>ToggleSlideVisibility</c> reads slideVisible, and if true, 
    /// sets the alpha attribute of the CanvasGroup component on this object 
    /// to 0 to make the slide canvas invisible, then sets canvasVisible to false.
    /// If slideVisible is false, it sets the alpha attribute to 1 to make the 
    /// canvas visible, and toggles slideVisible.
    /// </summary>
    public void ToggleColorVisibility() {
        if (colorVisible) {
            m_ColorTestCanvas.GetComponent<CanvasGroup>().alpha = 0;
            this.transform.GetComponentInChildren<CanvasGroup>().alpha = 1;
            colorVisible = false;
        } else {
            m_ColorTestCanvas.GetComponent<CanvasGroup>().alpha = 1;
            this.transform.GetComponentInChildren<CanvasGroup>().alpha = 0;
            colorVisible = true;
        }
    }

    public void InitializeDirectory()
    {
        if(SystemInfo.deviceType != DeviceType.Desktop)
        {
            //Scan the root folder for subdirectories (set of slides)
            string[] subDirectories = Directory.GetDirectories(folderRoot);

            //Iterate through subdirectories to get folderNames and SlideCount
            foreach (string subDirectory in subDirectories)
            {
                folderNames.Add(Path.GetFileName(subDirectory)); //Add folder name to list
                int sub_count = Directory.GetFiles(subDirectory, "*.jpg").Length; //count the number of files in the subdirectory that are .jph 
                slideCounts.Add(sub_count);
            }
        } else 
        {
            folderNames = new List<string> { "SlideSet1", "SlideSet2", "SlideSet3", "SlideSet4" }; // Names of the folders containing slides
            slideCounts = new List<int> { 12, 6, 54, 29 };
        }

    }
}
