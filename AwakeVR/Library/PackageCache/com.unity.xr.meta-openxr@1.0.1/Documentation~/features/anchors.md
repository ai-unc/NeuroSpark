---
uid: meta-openxr-anchors
---
# Anchors

This page is a supplement to the AR Foundation [Anchors](xref:arfoundation-anchors) manual. The following sections only contain information about APIs where Meta Quest exhibits unique platform-specific behavior.

[!include[](../snippets/arf-docs-tip.md)]

## Trackable ID

Unlike some other AR platforms, Meta's OpenXR runtime only supports creating anchors asynchronously. To fulfill AR Foundation's synchronous API design for [TryAddAnchor](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystem.TryAddAnchor), this package generates monotonically increasing `TrackableId`s for anchors.