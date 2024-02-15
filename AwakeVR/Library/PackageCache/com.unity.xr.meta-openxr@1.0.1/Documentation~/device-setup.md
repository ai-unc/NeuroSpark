---
uid: meta-openxr-device-setup
---
# Device setup

To build and run your AR app on a Meta Quest device, some setup steps are required. The sections below explain the necessary steps to prepare your Meta Quest device for AR.

## Getting started

If you are setting up a Meta Quest device for the first time, refer to Meta's [Set up Development Environment and Headset](https://developer.oculus.com/documentation/unity/unity-env-device-setup/) article to learn how to set your headset into Developer Mode and deploy your app via Android Debug Bridge (ADB).

## Meta software update

For the best experience on your Meta Quest device, consider updating to the latest version of the Meta Quest software. This will ensure that you have access to Meta's latest enhancements and bug fixes. You can update your Meta Quest software by going to **Settings** > **System** > **Software Update** in the universal menu on your device.

## Space Setup

Some AR Foundation features require that you first run Space Setup on your Meta Quest device before you launch your app. For example, if your app uses plane detection, AR Foundation will not be able to detect any planes unless you first run Space Setup.

To run Space Setup, follow the steps below:

1. Press ![the universal menu button](images/universal-menu.png) on your right Touch controller to access the universal menu.
2. Go to **Settings** > **Physical Space** > **Space Setup** and select the **Set up** button.
3. Follow the prompts in your headset to complete Space Setup.

> [!NOTE]
> If **Space Setup** is not an available option in the **Settings** > **Physical Space** menu on your device, you must update to a more recent version of the Meta Quest software for this option to appear.
