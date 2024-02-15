---
uid: meta-openxr-architecture
---
# Architecture

Unity OpenXR: Meta functions as both an OpenXR Feature Group and an AR Foundation [provider plug-in](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.0?subfolder=/manual/architecture/subsystems.html).

## OpenXR extensions

Meta's OpenXR extensions can be found in the Khronos Group [OpenXR Specification](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html).

This package enables support for the following OpenXR extensions in your project:

| Extension | Usage | Description |
| :-------- | :---- | :---------- |
| [XR_FB_scene_capture](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_FB_scene_capture) | [Session](xref:meta-openxr-session) | This extension allows an application to request that the system begin capturing information about what is in the environment around the user. |
| [XR_FB_passthrough](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_FB_passthrough) | [Camera](xref:meta-openxr-camera) | Passthrough is a way to show a user their physical environment in a light-blocking VR headset. |
| [XR_FB_spatial_entity](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_FB_spatial_entity) | [Anchors](xref:meta-openxr-anchors), [Plane detection](xref:meta-openxr-plane-detection) | This extension enables applications to use spatial entities to specify world-locked frames of reference. It enables applications to persist the real world location of content over time. All Facebook spatial entity and scene extensions are dependent on this one. |
| [XR_FB_spatial_entity_query](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_FB_spatial_entity_query) | [Plane detection](xref:meta-openxr-plane-detection) | This extension enables an application to discover persistent spatial entities in the area and restore them. Using the query system, the application can load persistent spatial entities from storage. |
| [XR_FB_spatial_entity_storage](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_FB_spatial_entity_storage) | [Plane detection](xref:meta-openxr-plane-detection) | This extension enables spatial entities to be stored and persisted across sessions. |
| [XR_FB_scene](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_FB_scene) | [Plane detection](xref:meta-openxr-plane-detection) | This extension expands on the concept of spatial entities to include a way for a spatial entity to represent rooms, objects, or other boundaries in a scene. |
| [XR_FB_display_refresh_rate](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_FB_display_refresh_rate) | [Display Utilities](xref:meta-openxr-display-utilities) | On platforms that support dynamically adjusting the display refresh rate, application developers may request a specific display refresh rate in order to improve the overall user experience. |
