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
6. [Resources](#resources)

# Project Overview
[Back to the Top](#table-of-contents)

Often, during brain or spinal surgery, it is beneficial for the patient to be conscious and interacting with the neurosurgeon so that the team can have validation that the surgery is proceeding safely and effectively. This interaction between the patient and the surgical team for the purposes of verifying cognitive and physical functioning is a part of a process known as neuromonitoring. While this can also include EEG’s, EMG’s and other measurements, our project is focused on the patient – doctor interaction.

We will develop a system that will present prompts for the patient on a VR headset. The system will present images, words, and other cues that are deemed appropriate for the surgery that the patient is undergoing. Initially, these tests will be manually selected by the surgical team, but the long term plan will be to have a system that learns which prompts to present for the specific moment of the specific procedure.

Our project will be based around the Meta Quest 3 VR headset. We will develop a virtual environment in Unity that will enable the headset to display text and images for the patient to see and respond to in accordance with the neurosurgeons prompts. We will start off simple with a floating screen that can display messages and images, and will progress to content that is selectable through a Bluetooth foot peddle “page turner” device.

# Operating Instructions
[Back to the Top](#table-of-contents) <br>

These instructions are to shows users how to install the application on a VR Headset, and how the application is intended to work on the headset. Our application does use device permissions which the steps will go through to make sure they are enabled. This is needed to access the File System on the device since AwakeVR reads the images on the device. 

How to start the application:

1. It strongly recommended completed the "Setting up the Meta Quest 3" section of this video, as the developer mode is needed and to have Meta Quest Developer Hub (MQDH) installed.
2. Login into MQDH and connect your VR headset through type C cable. You most likely will need to put on the headset and enable permissions to access the devices files.
3. Once the device has been connected you can take the built APK file and drag and drop it onto the headset. MQDH will begin installing it for you and throw an error if anything goes wrong. Once the APK is installed you can switch over to the headset device.
4. On the Headset ensure the installed application has the necessary permissions. You can check this by going to settings> App> Installed Apps> [Application Name]> Permissions. 
5. You can open your application now which you might find under "unknown sources" section in your list of apps.

(https://youtu.be/HtkfqgOoCUs)

Connecting Necessary Devices:
1. Text
2. Text
3. Text


What the application does:
1. Ensure the necessary devices are connected. 
2. Text
3. Text

# Setting up the Meta Quest 3
[Back to the Top](#table-of-contents)<br>
Content from [Device Setup | Oculus Developers](https://developer.oculus.com/documentation/native/android/mobile-device-setup/)

This topic shows how to set up a Meta Android device for running, debugging, and testing applications. The currently supported Meta devices that run Android are Meta Quest, Meta Quest 2, Meta Quest Pro, and Meta Quest 3.

Note: To set up an Oculus Rift for development, see the Rift Native SDK documentation.

To begin development locally, you must enable developer mode for the Meta device in the companion app on your mobile phone. Before you can put your device in developer mode, you must belong to (or have created) a developer organization on the Dashboard. If developing on Windows, you will also need to install drivers to use Android Device Bridge (ADB).

# Setting up Unity
[Back to the Top](#table-of-contents)

The following steps are to get any developer up and running with a development environment. To make the process easier its recommended to install all the necessary software first. 

1. Pull the repository into a directory
2. Open Unity Hub and hit “add project from disk”, then locate the “AwakeVR” directory from the repository.
3. Open the project in Unity and then load in the game scene.
4. Within the packages locate the “XR Plugin Management” packages then the xrmanifest.xml file. Update it with the contents of “AndroidManifest.xml” that is on github.
5. Go to build settings, swith platforms to Android, then build the project. This should generate the APK file that can be put on the VR headset.

(https://www.youtube.com/watch?v=nROCxYJsI2Q)

# Loading and Modifying tests
[Back to the Top](#table-of-contents)

# Resources
[Back to the Top](#table-of-contents)<br>
Below is a list of resources that were mentioned above or resources that could provide more guidance. Its strongly recommended that if the videos weren't helpful or if this readme wasn't enought that you go through these.

Device Setup: https://developer.oculus.com/documentation/native/android/mobile-device-setup/

Connect the Headset to MQDH: https://developer.oculus.com/documentation/unity/ts-odh/

ADB logging: https://developer.oculus.com/documentation/unity/ts-logcat/ or https://developer.android.com/studio/debug/logcat

XML file permissions (application permissions): https://source.android.com/docs/core/permissions/perms-allowlist

