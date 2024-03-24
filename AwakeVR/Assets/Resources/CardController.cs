using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.Networking;
using TMPro; // For UnityWebRequest

public class CardController : MonoBehaviour
{
    [SerializeField] FadeMaterial m_FadeMaterial;

    [SerializeField] Toggle m_PassthroughToggle;

    [SerializeField] GameObject m_ControllerPositionGUI;

    //[SerializeField]
    //Transform m_text;

    public Image slide_Image;
    private int currentSlideIndex = 1; //Current slide number. range [1 - folder amount]
    // private int currentFolderSlideCount = 5;
    private int currentFolderIndex = 0; //Current folder index [0-3]
    private string[] folderNames = { "SlideSet1", "SlideSet2", "SlideSet3", "SlideSet4" }; // Names of the folders containing slides
    private int[] slideCounts = { 12, 6, 54, 29 };
    private string questFolderRoot = "sdcard/Documents";
    private string desktopFolderRoot = "/Assets/Resources";
    private string folderRoot = string.Empty;
    /*private string folderPath = string.Empty;*/
    private bool passthroughMode = false;
    private bool slideVisible = true;
    private ArrowKeys playerInputActions;
    // private MRTemplateInputActions leftPositionProperty;
    // private Vector3 position;

    private void Awake() {
        playerInputActions = new ArrowKeys();
        playerInputActions.Keyboard.Enable();
        playerInputActions.Keyboard.Arrows.performed += Arrows_performed;
        // playerInputActions.Keyboard.LongArrows.performed += Long_Arrows_performed;
        // leftPositionProperty = new MRTemplateInputActions();
        m_ControllerPositionGUI.SetActive(false);
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
                TogglePassthrough();
            }
            if (keyPressed.name == "downArrow")
            {
                Debug.Log("Down Arrow long-pressed!");
                ToggleSlideVisibility();
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
        if (context.performed)
        {
            InputControl keyPressed = context.control;
            if (keyPressed.name == "upArrow")
            {
                Debug.Log("Up Arrow pressed!");
                TogglePassthrough();
                ToggleSlideVisibility();
            }
            if (keyPressed.name == "downArrow")
            {
                Debug.Log("Down Arrow pressed!");
                NextFolder();
            }
            if (keyPressed.name == "leftArrow")
            {
                Debug.Log("Left Arrow pressed!");
                PreviousSlide();
            }
            if (keyPressed.name == "rightArrow")
            {
                Debug.Log("Right Arrow pressed!");
                NextSlide();
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
        // vondoste - This clause determines if the app is running on a computer
        // or on another type of device, and selects either the Unity laptop 
        // root directory, or the root filepath of the Quest3 device.
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            folderRoot = desktopFolderRoot;
        }
        else
        {
            // folderRoot = questFolderRoot;
            folderRoot = desktopFolderRoot;
        }

        //Initialize the card with the first image from the current folder
        // vondoste - these lines count the number of files is the filder specified.
        // System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
        // int filecount = dir.GetFiles().Length;
        // currentFolderSlideCount = filecount / 2;
        passthroughMode = false;
        slideVisible = true;
        Debug.Log(" vondoste - device type: " + SystemInfo.deviceType);
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex], folderRoot);
    }

    /// <summary>
    /// Method <c>Update()</c> is called once per frame - tasks that need to be 
    /// constantly updated go here.
    /// </summary>
    void Update()
    {
        // vondoste - this line is querying the keyboard to see if 5 was pressed.  
        // not going to use this, but it's instructive.
        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            Debug.Log("5 key pressed!");
            //TogglePassthrough();
        }

        //position = leftPositionProperty.LeftHand.PalmPosition.ReadValue<Vector3>();
        //Debug.Log(m_text.GetChild(0).GetComponent<TextMeshPro>().text);
        //m_text.transform.GetComponent<TextMeshPro>().text = position.ToString();

    }

    /// <summary>
    /// Method <c>Showslide()</c> loads the image file Slide[slideIndex]
    /// </summary>
    /// <param name="slideIndex">The index of the current slide.</param>
    /// <param name="folderName">The name of the folder that contains the current test.</param>
    /// <param name="folderRoot"></param>
    void ShowSlide(int slideIndex, string folderName, string folderRoot)
    {
        // vondoste - hard-coding for testing purposes
        // DeviceType deviceType = (SystemInfo.deviceType;
        DeviceType deviceType = DeviceType.Desktop;

        if  (deviceType == DeviceType.Desktop)
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
                Debug.LogError("vondoste - Image not Found: " + folderPath);

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
        if (currentFolderIndex >= folderNames.Length)
        {
            currentFolderIndex = 0;
        }
        currentSlideIndex = 1;
        //folderPath = folderRoot + folderNames[currentFolderIndex];
        // System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
        // int filecount = dir.GetFiles().Length;
        // currentFolderSlideCount = filecount / 2;
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
            currentFolderIndex = folderNames.Length - 1;
        }
        currentSlideIndex = 1;
        /*folderPath = folderRoot + folderNames[currentFolderIndex];*/
        // System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
        // int filecount = dir.GetFiles().Length;
        // currentFolderSlideCount = filecount / 2;
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
            m_ControllerPositionGUI.SetActive(false);
            passthroughMode = false;
        }
        else
        {
            m_FadeMaterial.FadeSkybox(true);
            m_ControllerPositionGUI.SetActive(true);
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
        }
        else
        {
            this.transform.GetComponentInChildren<CanvasGroup>().alpha = 1;
            slideVisible = true;
        }
    }
}
