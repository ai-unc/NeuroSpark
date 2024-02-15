---
uid: meta-openxr-camera
---
# Camera

On Meta Quest devices, AR Foundation's Camera subsystem controls Meta [Passthrough](https://www.meta.com/help/quest/articles/in-vr-experiences/oculus-features/passthrough/). Enable the [AR Camera Manager component](xref:arfoundation-camera-components#ar-camera-manager-component) to enable Passthrough, and disable it to disable Passthrough.

This page is a supplement to the AR Foundation [Camera](xref:arfoundation-camera) manual. The following sections only contain information about APIs where Meta Quest exhibits unique platform-specific behavior.

[!include[](../snippets/arf-docs-tip.md)]

## AR Camera Background component

Meta Passthrough does not require the [AR Camera Background component](xref:arfoundation-camera-components#ar-camera-background-component). If `ARCameraBackground` is in your scene, it will have no effect on Meta Quest devices. If your scene only targets Meta Quest devices, you can safely delete the AR Camera Background component from your XR Origin's **Main Camera** GameObject.

## Scene setup

Meta Passthrough requires that your Camera's **Clear Flags** are set to **Solid Color**, with the **Background** alpha channel value set to zero. See [scene setup](xref:meta-openxr-project-setup#scene-setup) to learn how to set the **Background** alpha channel value.

## Image capture

This package does not support AR Foundation [image capture](xref:arfoundation-image-capture) via CPU images.
