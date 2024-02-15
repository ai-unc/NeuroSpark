---
uid: meta-openxr-display-utilities
---
# Display Utilities

The Meta Quest Display Utilities feature enables you to:
1. Get the supported display refresh rates for the device.
2. Request a selected display refresh rate.

## Enable Display Utilities

To enable Meta Quest Display Utilities in your app:

1. Go to **Project Settings** > **XR Plug-in Management** > **OpenXR**.
2. Under **OpenXR Feature Groups**, select the **Meta Quest** feature group.
3. If disabled, enable the **Meta Quest Display Utilities** OpenXR feature.

As a standalone feature of this package, **Meta OpenXR Display Utilities** solely depends on **Meta Quest Support** and does not require that you enable any other feature in the **Meta Quest** feature group.

## Code sample

Once enabled, Meta Quest Display Utilities adds additional capabilities to Unity's [XRDisplaySubsystem](xref:UnityEngine.XR.XRDisplaySubsystem) using C# extension methods: [TryGetSupportedDisplayRefreshRates](xref:UnityEngine.XR.OpenXR.Features.Meta.MetaOpenXRDisplaySubsystemExtensions.TryGetSupportedDisplayRefreshRates*) and [TryRequestDisplayRefreshRate](xref:UnityEngine.XR.OpenXR.Features.Meta.MetaOpenXRDisplaySubsystemExtensions.TryRequestDisplayRefreshRate*) 

> [!IMPORTANT]
> These extension methods will always return false if you did not [Enable Meta Quest Display Utilities](#enable-meta-quest-display-utilities) in **XR Plug-in Management**.

The code sample below demonstrates how to use these extension methods:

[!code-cs[request_display_refreshRate](../../Tests/CodeSamples/MetaQuestDisplayUtilitiesSample.cs#request_display_refreshRate)]
