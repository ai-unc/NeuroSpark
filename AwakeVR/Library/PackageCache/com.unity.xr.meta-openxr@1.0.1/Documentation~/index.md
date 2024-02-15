---
uid: meta-openxr-manual
---
# Unity OpenXR: Meta

Unity OpenXR: Meta enables Meta Quest device support for your AR Foundation projects and provides a C# interface for Meta's OpenXR runtime. This package depends on both [AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1) and the [OpenXR Plug-in](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.7).

## Getting started

To enable the features in this package, go to **Project Settings** > **XR Plug-in Management** > **OpenXR** > **OpenXR Feature Groups** and enable the **Meta Quest** feature group as shown below:

![Unity's Project Settings window is open to XR Plug-in Management > OpenXR, showing a list of enabled features in the Meta Quest feature group](images/openxr-features-all.png)<br/>*The Meta Quest OpenXR feature group, shown with all features enabled*

> [!IMPORTANT]
> To use any feature in this package, you must enable the corresponding OpenXR Feature in the **Meta Quest** feature group. Otherwise, your app cannot access these features.

### Setup instructions

Refer to [Device setup](xref:meta-openxr-device-setup) and [Project setup](xref:meta-openxr-project-setup), respectively, for more detailed information about how to set up your Meta Quest device and Unity project to use this package.

## Features

[!include[](snippets/features-table.md)]

### Unsupported AR features

This package does not implement the following AR features:

| Feature | Description |
| :------ | :---------- |
| [Image tracking](xref:arfoundation-image-tracking) | Detect and track 2D images. |
| [Object tracking](xref:arfoundation-object-tracking) | Detect and track 3D objects. |
| [Face tracking](xref:arfoundation-face-tracking) | Detect and track human faces. |
| [Body tracking](xref:arfoundation-body-tracking) | Detect and track a human body. |
| [Point clouds](xref:arfoundation-point-clouds) | Detect and track feature points. |
| [Meshing](xref:arfoundation-meshing) | Generate meshes of the environment. |
| [Environment probes](xref:arfoundation-environment-probes) | Generate cubemaps of the environment. |
| [Occlusion](xref:arfoundation-occlusion) | Occlude AR content with physical objects and perform human segmentation. |
| [Participants](xref:arfoundation-participant-tracking) | Track other devices in a shared AR session. |

## A quick primer on OpenXR

OpenXR is an open-source standard that defines an interface between XR apps and platform runtimes. The OpenXR specification contains two categories of features: _core features_, which are present on every platform, and _extensions_, which are optional and may not be implemented by some platforms.

Unity's [OpenXR Plug-in](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.7) integrates core features, while this package integrates Meta-specific vendor extensions. Refer to [Architecture](xref:meta-openxr-architecture) for a full list of OpenXR extensions that this package uses.

## Requirements

To use Unity OpenXR: Meta, your project must meet the following requirements:

* Unity 2022.3 or newer
* AR Foundation 5.1.0-pre.6 or newer
* OpenXR 1.7 or newer

When you install this package, if you don't already have AR Foundation or OpenXR in your project, the Package Manager will automatically install these dependencies.
