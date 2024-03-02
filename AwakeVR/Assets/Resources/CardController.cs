using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class CardController : MonoBehaviour
{
    [SerializeField]
    FadeMaterial m_FadeMaterial;

    [SerializeField]
    Toggle m_PassthroughToggle;

    public Image slide_Image;
    private int currentSlideIndex = 1; //Current slide number. range [1 - folder amount]
    // private int currentFolderSlideCount = 5;
    private int currentFolderIndex = 0; //Current folder index [0-3]
    private string[] folderNames = { "SlideSet1", "SlideSet2", "SlideSet3", "SlideSet4" }; // Names of the folders containing slides
    private int[] slideCounts = { 12, 6, 54, 29 };
    private string folderRoot = "Assets/Resources/";
    private string folderPath = string.Empty;
    private bool passthroughMode = false;
    private bool slideVisible = true;
    private ArrowKeys playerInputActions;

    private void Awake() {
        playerInputActions = new ArrowKeys();
        playerInputActions.Keyboard.Enable();
        playerInputActions.Keyboard.Arrows.performed += Arrows_performed;
        playerInputActions.Keyboard.LongArrows.performed += Long_Arrows_performed;
    }

    private void Long_Arrows_performed(InputAction.CallbackContext context) {
            if (context.performed) {            
            InputControl keyPressed = context.control;
            if (keyPressed.name == "upArrow") {
                Debug.Log("Up Arrow long-pressed!");
                TogglePassthrough();
            }
            if (keyPressed.name == "downArrow") {
                Debug.Log("Down Arrow long-pressed!");
                ToggleSlideVisibility();
            }
            if (keyPressed.name == "leftArrow") {
                Debug.Log("Left Arrow long-pressed!");                
            }
            if (keyPressed.name == "rightArrow") {
                Debug.Log("Right Arrow long-pressed!");                
            }
        }
    }

    private void Arrows_performed(InputAction.CallbackContext context) {
        if (context.performed) {             
            InputControl keyPressed = context.control;
            if (keyPressed.name == "upArrow" ) {
                Debug.Log("Up Arrow pressed!");
                PreviousFolder();
            }
            if (keyPressed.name == "downArrow") {
                Debug.Log("Down Arrow pressed!");
                NextFolder();
            }
            if (keyPressed.name == "leftArrow") {
                Debug.Log("Left Arrow pressed!");
                PreviousSlide();
            }
            if (keyPressed.name == "rightArrow") {
                Debug.Log("Right Arrow pressed!");
                NextSlide();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the card with the first image from the current folder
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);
        // vondoste - these lines count the number of files is the filder specified.
        folderPath = folderRoot + folderNames[currentFolderIndex];
        // System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
        // int filecount = dir.GetFiles().Length;
        // currentFolderSlideCount = filecount / 2;
        passthroughMode = false;
        
    }
    
    // Update is called once per frame
    void Update() {       
        if (Keyboard.current.digit5Key.wasPressedThisFrame ) {
            Debug.Log("5 key pressed!");
            TogglePassthrough();
        }

    }

    void ShowSlide(int slideIndex, string folderName) 
    {
        string imageName = "Slide" + slideIndex;
        string folderPath = folderName + "/" + imageName;
        
        //Load the image from the specified folder
        Sprite spriteImage = Resources.Load<Sprite>(folderPath); //NOTE: NEED A RESOURCES FOLDER IN ASSETS

        if(spriteImage != null){
            //Set the image on the card
            slide_Image.sprite = spriteImage;
        }else{
            Debug.LogError("Image not Found: " + folderPath);
        }
    } 
    public void NextSlide()
    {
        currentSlideIndex++;
        if (currentSlideIndex > slideCounts[currentFolderIndex]) {
            currentSlideIndex = 1;
        }
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);
    }

    public void PreviousSlide()
    {
        currentSlideIndex--;
        if (currentSlideIndex < 1)
        {
            currentSlideIndex = slideCounts[currentFolderIndex];
        }
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);


    }

    public void NextFolder()
    {
        currentFolderIndex++;
        if (currentFolderIndex >= folderNames.Length) {
            currentFolderIndex = 0;
        }
        currentSlideIndex = 1;
        folderPath = folderRoot + folderNames[currentFolderIndex];
        // System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
        // int filecount = dir.GetFiles().Length;
        // currentFolderSlideCount = filecount / 2;
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);
        
    }

    public void PreviousFolder()
    {
        currentFolderIndex--;
        if (currentFolderIndex < 0) {
            currentFolderIndex = folderNames.Length - 1;
        }
        currentSlideIndex = 1;
        folderPath = folderRoot + folderNames[currentFolderIndex];
        // System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
        // int filecount = dir.GetFiles().Length;
        // currentFolderSlideCount = filecount / 2;
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);
    }

    public void TogglePassthrough() {
        Debug.Log("TogglePassthrough called!");
        // m_PassthroughToggle.isOn = false;
        if (passthroughMode) {
            m_FadeMaterial.FadeSkybox(false);
            // m_PassthroughToggle.enabled = false;
            passthroughMode = false;
        } else {
            m_FadeMaterial.FadeSkybox(true);
            // m_PassthroughToggle.enabled = true;
            passthroughMode = true;
            // this.transform.GetComponentInChildren<Canvas>().enabled = passthroughMode;
        }
    }

    public void ToggleSlideVisibility() {
        if (slideVisible) {
            this.transform.GetComponentInChildren<CanvasGroup>().alpha = 0;
            slideVisible = false;
        } else {
            this.transform.GetComponentInChildren<CanvasGroup>().alpha = 1;
            slideVisible = true;
        }
    }
}
