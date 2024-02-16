# AwakeVR
Virtual Reality for Awake Cranis

This project will use a VR headset to streamline Neuromonitoring during intracranial surgical procedures.

# Table of Contents
1. [Project Overview](#project-overview)
2. [Setting up the Meta Quest 3](#setting-up-the-meta-quest-3)
3. Setting up Unity dev environment

# Project Overview
[Back to the Top](#table-of-contents)

Often, during brain or spinal surgery, it is beneficial for the patient to be conscious and interacting with the neurosurgeon so that the team can have validation that the surgery is proceeding safely and effectively. This interaction between the patient and the surgical team for the purposes of verifying cognitive and physical functioning is a part of a process known as neuromonitoring. While this can also include EEG’s, EMG’s and other measurements, our project is focused on the patient – doctor interaction.

We will develop a system that will present prompts for the patient on a VR headset. The system will present images, words, and other cues that are deemed appropriate for the surgery that the patient is undergoing. Initially, these tests will be manually selected by the surgical team, but the long term plan will be to have a system that learns which prompts to present for the specific moment of the specific procedure.

Our project will be based around the Meta Quest 3 VR headset. We will develop a virtual environment in Unity that will enable the headset to display text and images for the patient to see and respond to in accordance with the neurosurgeons prompts. We will start off simple with a floating screen that can display messages and images, and will progress to content that is selectable through a Bluetooth foot peddle “page turner” device.

# Setting up the Meta Quest 3
[Back to the Top](#table-of-contents)
Content from [Device Setup | Oculus Developers](https://developer.oculus.com/documentation/native/android/mobile-device-setup/)

This topic shows how to set up a Meta Android device for running, debugging, and testing applications. The currently supported Meta devices that run Android are Meta Quest, Meta Quest 2, Meta Quest Pro, and Meta Quest 3.

Note: To set up an Oculus Rift for development, see the Rift Native SDK documentation.

To begin development locally, you must enable developer mode for the Meta device in the companion app on your mobile phone. Before you can put your device in developer mode, you must belong to (or have created) a developer organization on the Dashboard. If developing on Windows, you will also need to install drivers to use Android Device Bridge (ADB).
