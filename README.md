# AwakeVR

*** This page is a work in Progress ***<br>
Virtual Reality for Awake Cranis.

This project will use a VR headset to streamline Neuromonitoring during intracranial surgical procedures.

# Table of Contents
1. [Project Overview](#project-overview)
2. [Operating Instructions](#operating-instructions)
3. [Setting up the Meta Quest 3](#setting-up-the-meta-quest-3)
4. [Setting up Unity dev environment](#setting-up-unity)
5. [Loading and modifying the test content](#loading-and-modifying-tests)
6. [Required Software](#Required-Software)
7. [Resources](#resources)

# Project Overview
[Back to the Top](#table-of-contents)

Often, during brain or spinal surgery, it is beneficial for the patient to be conscious and interacting with the neurosurgeon so that the team can have validation that the surgery is proceeding safely and effectively. This interaction between the patient and the surgical team for the purposes of verifying cognitive and physical functioning is a part of a process known as neuromonitoring. While this can also include EEG’s, EMG’s and other measurements, our project is focused on the patient – doctor interaction.

We will develop a system that will present prompts for the patient on a VR headset. The system will present images, words, and other cues that are deemed appropriate for the surgery that the patient is undergoing. Initially, these tests will be manually selected by the surgical team, but the long term plan will be to have a system that learns which prompts to present for the specific moment of the specific procedure.

Our project will be based around the Meta Quest 3 VR headset. We will develop a virtual environment in Unity that will enable the headset to display text and images for the patient to see and respond to in accordance with the neurosurgeons prompts. We will start off simple with a floating screen that can display messages and images, and will progress to content that is selectable through a Bluetooth foot peddle “page turner” device.

# Operating Instructions
[Back to the Top](#table-of-contents) <br>

These instructions are to shows users how to install the application on a VR Headset, and how the application is intended to work on the headset. Our application does use device permissions which the steps will go through to make sure they are enabled. This is needed to access the File System on the device since AwakeVR reads the images on the device. 

### How to start the application:

1. It strongly recommended completed the "Setting up the Meta Quest 3" section of this video, as the developer mode is needed and to have Meta Quest Developer Hub (MQDH) installed.
2. Login into MQDH and connect your VR headset through type C cable. You most likely will need to put on the headset and enable permissions to access the devices files.
3. Once the device has been connected you can take the built APK file and drag and drop it onto the headset. MQDH will begin installing it for you and throw an error if anything goes wrong. Once the APK is installed you can switch over to the headset device.
4. On the Headset ensure the installed application has the necessary permissions. You can check this by going to Settings> App> Installed Apps> [Application Name]> Permissions. 
5. You can open your application now which you might find under "unknown sources" section in your list of apps.

(https://youtu.be/HtkfqgOoCUs)

### Connecting Necessary Devices:
1. AirTurn BT500|S-4 - configuration and pairing instructions
2. Text
3. Text


### What the application does:
1. Ensure the necessary devices are connected. 
2. Text
3. Text

# Setting up the Meta Quest 3
[Back to the Top](#table-of-contents)<br>
Content from [Device Setup | Oculus Developers](https://developer.oculus.com/documentation/native/android/mobile-device-setup/)

This topic shows how to set up a Meta Android device for running, debugging, and testing applications. The currently supported Meta devices that run Android are Meta Quest, Meta Quest 2, Meta Quest Pro, and Meta Quest 3.

Note: To set up a Meta Quest 3t for development, see the Rift Native SDK documentation.

1. Link Quest 3 with Meta Account
2. Put quest 3 in development mode, this requires a Meta developer account.
3. Install APK file, make sure that the app has access to the local file-system and the pre-defined boundaries.
4. Copy test file directories to the /Documents folder in the Quest 3 file system.
5. Pre-define the boundaries in the empty Operating Room, you won't be able to do this when the room is full of people and equipment.
6. Pair the Quest 3 with the Bluetooth footswitch device (this device emulates a bluetooth keyboard).
7. Calibrate the floor level before you put the device on the patient. This will impact the perceived height of the controllers above the floor and affect the range of motion display. 

To begin development locally, you must enable developer mode for the Meta device in the companion app on your mobile phone. Before you can put your device in developer mode, you must belong to (or have created) a developer organization on the Dashboard. If developing on Windows, you will also need to install drivers to use Android Device Bridge (ADB).

# Setting up Unity
[Back to the Top](#table-of-contents)

The following steps are to get any developer up and running with a development environment. To make the process easier its recommended to install all the necessary software first. 

1. Make sure that you have at least 15 Gigs of free disk space before you begin setting this up.
2. Pull the repository into a directory
3. Open Unity Hub and hit “add project from disk”, then locate the inner “AwakeVR” directory inside the repository.
4. Open the project in Unity and then in the "Project" tab navigate to /Assets/Scenes and open SampleScene.unity.
5. Within the packages locate the “XR Plugin Management” packages then the xrmanifest.xml file. Update it with the contents of “AndroidManifest.xml” that is on github.
6. Go to build settings, swith platforms to Android, then build the project. This should generate the APK file that can be put on the VR headset.

(https://www.youtube.com/watch?v=nROCxYJsI2Q)

# Loading and Modifying tests
[Back to the Top](#table-of-contents)<br>
This project has an amazing feature that lets users upload their own Slide Sets and watch them appear in the application! This feature is dynamic allowing the user to have as many Slide Sets as they like as long as they follow the intended format. The main intention for this feature is for users to create Powerpoint Slides or Google Slides and then have them appear on screen for users to follow along. It is both helpful for developers and future users to have in this format otherwise it would require developers to constantly rebuild the application with the new content in it. This repository has 4 core slidesets that are intended to load with the application, and a 5th for testing purposes. 

### General Format
* A Slide Set must be in a folder and all the images must be in .JPG file format.
* It does not matter what the name of the folder is for an individual Slide Set but it is recommended to follow the intended naming scheme of "/SlideSet<number>/". Ex. SlideSet1/.
* The images within a folder must be in the name scheme of /slide<number>.jpg. It was important to have some order in deciding which image will go first so we decided to have these images be read in order of their index.  Ex. /SlideSet1/Slide1.jpg or /SlideSet3/Slide10.jpg.
* It does not matter what the image size is in the folder, they get rescaled to fit the UI card.
* When loading the Slide Sets on to the headset they need to be in the Documents Directory.

** NOTE: You can convert Powerpoint Slides to JPG images and it will automatically create a folder with the images already numbered! The only thing you might have to do is rename the folder but otherwise these can be loaded on the VR headset. **

### Developer Format
When devloping on the project you are going to generally want to follow the format above but when working in Unity's Game Scene things are a bit different. Now, Unity does allow you to run application directly on the VR Headset before finalizing a build but there can be long load times which can be an issue, and developers might not always have the VR headset with them. You should recognize in the code "Card Controller" Script that the code checks for which device its being ran on and reads directories accordingly. 

* Within Unity desktop runs read the Assets > Resources folder for Slide Sets
* The function is called InitializeDirectory()
* There  are two Lists in which the folders and file count get stored into. Modify these to include the new folder names and amount of files for that folder:
  * folderNames
  * SlideCounts

# Required Software
[Back to the Top](#table-of-contents) <br>

This section list out all the software that is used in any other part of the README. It is recommended thay you install these before beginning any other setup. The other setup sections are meant to account for cases that are ran into with this particular project but won't go through the actual installation of them. 

Install Git: https://git-scm.com/downloads

Install the Unity Installer which should provide: https://unity.com/download
* Unity Hub
* Unity Editor version 2022.3.19f1
* Install Visual Studio and its plugin

Install Meta Quest Developer Hub: https://developer.oculus.com/downloads/package/oculus-developer-hub-win/

# Resources
[Back to the Top](#table-of-contents)<br>
Below is a list of resources that were mentioned above or resources that could provide more guidance. Its strongly recommended that if the videos weren't helpful or if this readme wasn't enought that you go through these.

Device Setup: https://developer.oculus.com/documentation/native/android/mobile-device-setup/

Connect the Headset to MQDH: https://developer.oculus.com/documentation/unity/ts-odh/

ADB logging: https://developer.oculus.com/documentation/unity/ts-logcat/ or https://developer.android.com/studio/debug/logcat

XML file permissions (application permissions): https://source.android.com/docs/core/permissions/perms-allowlist

