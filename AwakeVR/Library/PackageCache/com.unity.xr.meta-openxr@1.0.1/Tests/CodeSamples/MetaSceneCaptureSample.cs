using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.OpenXR.Features.Meta.Tests
{
    public class MetaSceneCaptureSample
    {
        public void ExampleRequestSceneCapture()
        {
#region meta_scene_capture
            // Getting a reference to the ARSession at runtime is not optimal.
            // For better runtime performance, reuse a saved reference to the ARSession instead
            var arSession = Object.FindAnyObjectByType<ARSession>();

            // To access the scene capture API,
            // cast the ARSession's subsystem to MetaOpenXRSessionSubsystem
            var success = (arSession.subsystem as MetaOpenXRSessionSubsystem)
                .TryRequestSceneCapture();
#endregion
        }
    }
}
