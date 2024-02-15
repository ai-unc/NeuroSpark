---
uid: meta-openxr-raycasts
---
# Raycasts

This page is a supplement to the AR Foundation [Raycasts](xref:arfoundation-raycasts) manual. The following sections only contain information about APIs where Meta Quest exhibits unique platform-specific behavior.

[!include[](../snippets/arf-docs-tip.md)]

## Raycast architecture

This package defines an implementation of AR Foundation's [XRRaycastSubsystem.Provider](xref:UnityEngine.XR.ARSubsystems.XRRaycastSubsystem.Provider), but the implementation does not use any OpenXR functionality. In fact, the raycast provider in this pacakge is completely empty. By including an empty raycast provider, this package enables a fallback Unity-world-space raycast implementation in AR Foundation's `ARRaycastManager`.

If your app uses AR raycasts, you should use the [ARRaycastManager](xref:UnityEngine.XR.ARFoundation.ARRaycastManager) APIs. Do not access `MetaOpenXRRaycastSubsystem` directly.
