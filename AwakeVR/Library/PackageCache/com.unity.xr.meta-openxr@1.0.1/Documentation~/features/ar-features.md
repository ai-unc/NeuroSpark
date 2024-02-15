---
uid: meta-openxr-ar-features
---
# AR features

AR features implement [AR Foundation](xref:arfoundation-manual) interfaces. These features are documented in the AR Foundation package manual, so this manual only includes information regarding APIs where Meta's OpenXR runtime exhibits unique platform-specific behavior.

This package implements the following AR features:

| Feature | Description |
| :------ | :---------- |
| [Session](xref:meta-openxr-session) | Enable, disable, and configure AR on the target platform. |
| [Camera](xref:meta-openxr-camera) | Render images from device cameras and perform light estimation. |
| [Plane Detection](xref:meta-openxr-plane-detection) | Detect and track flat surfaces. |
| [Anchors](xref:meta-openxr-anchors) | Track arbitrary points in space. |
| [Raycasts](xref:meta-openxr-raycasts) | Cast rays against tracked items. |

[!include[](../snippets/arf-docs-tip.md)]
