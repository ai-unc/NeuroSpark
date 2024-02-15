---
uid: meta-openxr-changelog
---
# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2023-11-02

### Changed

- Updated documentation to reflect changes to the **Room Setup** process including it being renamed to **Space Setup** and relocated to **Settings** > **Physical Space**

### Fixed

- Fixed an issue where Meta's OpenXR runtime would log a warning every frame for each `ARPlane` in the scene regarding the use of an older version of their OpenXR API for semantic labels.
- Fixed documentation links for Meta-OpenXR features in `Project Settings` > `XR Plug-in Management` > `OpenXR`.

## [1.0.0] - 2023-09-30

### Added

- Added new extension methods for the `XRDisplaySubsystem` to support getting the supported display refresh rates and requesting a display refresh rate. Refer to [Meta Quest Display Utilities](xref:meta-openxr-display-utilities) for more information.
- Added documentation for recommended settings when using [Universal Render Pipeline](xref:meta-openxr-project-setup#universal-render-pipeline).

### Changed

- Renamed the AR features in the `Meta Quest` OpenXR feature group for brevity and consistency.
- Changed the `ARPlaneFeature` implementation to request the Android permission `com.oculus.permission.USE_SCENE` on Start on OpenXR runtime versions 1.0.31 and newer, as required by the OpenXR specification.
- Changed the `Unity.XR.MetaOpenXR` runtime assembly to only be included on Android and Editor platforms. This was always intended, but previously the assembly had been included on all platforms.

### Fixed

- Fixed an issue where the `ARAnchorFeature` enabled more OpenXR extensions than were necessary for the `XRAnchorSubsystem` to function correctly.

## [0.2.1] - 2023-09-14

### Added

- Added `com.oculus.permission.USE_SCENE` permission to the manifest which will show a permission dialog to get user consent if scene data such as planes is requested.
- Added support for `XRSessionSubsystem`'s `trackingState` and `notTrackingReason`.
- Added support for [Scene capture](xref:meta-openxr-session#scene-capture).
- Added validation rules. Go to **Project Settings** > **XR Plug-in Management** > **Validation Rules** to check if your project settings match the validation rules.

### Changed

- Split **AR Foundation: Meta** Feature into separate OpenXR Features, one for each AR Foundation subsystem. Go to **Project Settings** > **XR Plug-in Management** > **OpenXR** to enable or disable these features.
- Changed minimum Unity version to 2022.3
- Updated linked OpenXR version to 1.0.28 from 1.0.25.
- Changed the package name from **Meta OpenXR Feature** to **Unity OpenXR: Meta**.
- Changed blending factors for blending Unity content over passthrough from [source alpha, 1 - source alpha] to [1, 1 - source alpha] to support rendering additive materials.

### Fixed

- Fixed Session availability API to properly check that an OpenXR session instance exists.

## [0.1.2] - 2023-06-28

### Fixed

- Fixed math operations for comparing structs with single-precision values.
- Fixed an issue where it was possible to incorrectly remove or update planes before they were added in some uncommon situations. This resulted in an `InvalidOperationException` in AR Foundation that is now no longer thrown.

## [0.1.1] - 2023-06-12

### Added

- Added support for getting plane boundary vertices via [XRPlaneSubsystem.GetBoundary](xref:UnityEngine.XR.ARSubsystems.XRPlaneSubsystem.GetBoundary(UnityEngine.XR.ARSubsystems.TrackableId,Unity.Collections.Allocator,Unity.Collections.NativeArray{UnityEngine.Vector2}@)).
- Added support for getting plane alignment via [ARPlane.alignment](xref:UnityEngine.XR.ARFoundation.ARPlane.alignment) as well as [planeAlignmentThreshold](xref:UnityEngine.XR.OpenXR.Features.Meta.MetaOpenXRPlaneSubsystem.planeAlignmentThreshold). Refer to [Plane alignment](xref:meta-openxr-plane-detection#plane-alignment) for more information.
- Added support for [plane classification](xref:UnityEngine.XR.ARSubsystems.PlaneClassification).
- Added support for planes to be updated and removed.
- Added a custom Editor for [ARCameraManager](xref:UnityEngine.XR.ARFoundation.ARCameraManager) to clarify that its serialized properties have no effect on Meta Passthrough.
- Added a custom Editor for [ARCameraBackground](xref:UnityEngine.XR.ARFoundation.ARCameraBackground) to clarify that it has no effect on Meta Quest devices.
- Added support for [requestedDetectionMode](xref:UnityEngine.XR.ARFoundation.ARPlaneManager.requestedDetectionMode) to filter horizontal and vertical planes.

### Changed

- Changed the `trackableId` property of planes to be backed by OpenXR's `uuid` field, instead of a monotonically increasing counter.
- Changed the display name of the AR Foundation Meta Quest feature.
- Grouped the Meta Quest OpenXR features in an OpenXR feature group.
- Passthrough layer is now composited behind AR content rather than alpha blended in front of it. This fixes an issue where AR opaques always appeared semi-transparent.

### Fixed

- Fixed an issue where the `trackingState` property of planes could be incorrectly reported in some rare cases.
- Fixed an issue where the center of a plane in plane space was incorrectly reported as a value other than (0, 0).
- Fixed an issue where `ARPlane`s were not removed if the `Redo Walls` action or the `Clear Room Set Up` action was performed through the Meta Quest menu.

## [0.1.0] - 2023-04-04

- This is the first release of Meta OpenXR Plugin <com.unity.xr.meta-openxr>.