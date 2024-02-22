using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class CardController : MonoBehaviour
{
    public Image slide_Image;
    private int currentSlideIndex = 1; //Current slide number. range [1 - folder amount]
    private int currentFolderIndex = 0; //Current folder index [0-3]
    private string[] folderNames = { "SlideSet1", "SlideSet2", "SlideSet3", "SlideSet4" }; // Names of the folders containing slides
    // Start is called before the first frame update
    void Start()
    {
        //Initialize the card with the first image from the current folder
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);
        // vondoste - these lines count the number of files is the filder specified.
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo("Assets/Resources/SlideSet1");
        int filecount = dir.GetFiles().Length;
        print(filecount);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            PreviousFolder();
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            NextFolder();
        }
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            PreviousSlide();
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            NextSlide();
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
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);
    }

    public void PreviousSlide()
    {
        currentSlideIndex--;
        if (currentSlideIndex < 1)
        {
            currentSlideIndex = 1;
        }
        ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);


    }

    public void NextFolder()
    {
        if(currentFolderIndex < folderNames.Length -1)
        {
            currentFolderIndex++;
            currentSlideIndex = 1;
            ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);
        }
    }

    public void PreviousFolder()
    {
        if (currentFolderIndex > 0)
        {
            currentFolderIndex--;
            currentSlideIndex = 1;
            ShowSlide(currentSlideIndex, folderNames[currentFolderIndex]);
        }
    }
}
