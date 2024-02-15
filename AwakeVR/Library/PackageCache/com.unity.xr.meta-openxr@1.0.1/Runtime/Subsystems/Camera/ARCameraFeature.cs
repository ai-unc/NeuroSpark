using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using Unity.XR.CoreUtils;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.OpenXR.Features;
#endif
#if MODULE_URP_ENABLED
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#endif

namespace UnityEngine.XR.OpenXR.Features.Meta
{
    /// <summary>
    /// Enables AR Foundation passthrough support via OpenXR for Meta Quest devices.
    /// </summary>
#if UNITY_EDITOR
    [OpenXRFeature(UiName = "Meta Quest: AR Camera (Passthrough)",
        BuildTargetGroups = new[] { BuildTargetGroup.Android },
        Company = Constants.k_CompanyName,
        Desc = "AR Foundation camera support on Meta Quest devices",
        DocumentationLink = Constants.DocsUrls.k_CameraUrl,
        OpenxrExtensionStrings = k_OpenXRRequestedExtensions,
        Category = FeatureCategory.Feature,
        FeatureId = featureId,
        Version = "0.1.0")]
#endif
    public class ARCameraFeature : OpenXRFeature
    {
        /// <summary>
        /// The feature id string. This is used to give the feature a well known id for reference.
        /// </summary>
        public const string featureId = "com.unity.openxr.feature.arfoundation-meta-camera";

        /// <summary>
        /// The set of OpenXR spec extension strings to enable, separated by spaces.
        /// For more information, refer to
        /// <see href="https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.6/manual/features.html#enabling-openxr-spec-extension-strings"/>.
        /// </summary>
        const string k_OpenXRRequestedExtensions =
            Constants.OpenXRExtensions.k_XR_FB_passthrough + " " +
            Constants.OpenXRExtensions.k_XR_FB_composition_layer_alpha_blend;

        static List<XRCameraSubsystemDescriptor> s_CameraDescriptors = new();

        /// <summary>
        /// Instantiates Meta OpenXR Camera subsystem instances, but does not start it.
        /// (Start/Stop is typically handled by AR Foundation managers.)
        /// </summary>
        protected override void OnSubsystemCreate()
        {
            CreateSubsystem<XRCameraSubsystemDescriptor, XRCameraSubsystem>(
                s_CameraDescriptors,
                MetaOpenXRCameraSubsystem.k_SubsystemId);
        }

        /// <summary>
        /// Destroys the camera subsystem.
        /// </summary>
        protected override void OnSubsystemDestroy()
        {
            DestroySubsystem<XRCameraSubsystem>();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Validation Rules for ARCameraFeature.
        /// </summary>
        protected override void GetValidationChecks(List<ValidationRule> rules, BuildTargetGroup targetGroup)
        {
            var AdditionalRules = new ValidationRule[]
            {
#if MODULE_URP_ENABLED
                new ValidationRule(this)
                {
                    message = "Vulkan supports the most setting configurations to enable Passthrough on Meta Quest when using URP.",
                    checkPredicate = () =>
                    {
                        if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset)
                        {
                            var graphicsApis = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
                            return graphicsApis.Length > 0 && graphicsApis[0] == GraphicsDeviceType.Vulkan;
                        }
                        return true;
                    },
                    fixItAutomatic = true,
                    fixItMessage = "Go to Project Settings > Player Settings > Android. In the list of 'Graphics APIs', make sure that " +
                                    "'Vulkan' is listed as the first API.",
                    fixIt = () =>
                    {
                        var currentGraphicsApis = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
                        int apiLength = currentGraphicsApis.Length;
                        apiLength += Array.Exists(currentGraphicsApis, element => element == GraphicsDeviceType.Vulkan) ? 0 : 1;
                        GraphicsDeviceType[] correctGraphicsApis = new GraphicsDeviceType[apiLength];
                        correctGraphicsApis[0] = GraphicsDeviceType.Vulkan;
                        var id = 1;
                        for (var i = 0; i < currentGraphicsApis.Length; ++i)
                        {
                            if (currentGraphicsApis[i] != GraphicsDeviceType.Vulkan)
                            {
                                correctGraphicsApis[id] = currentGraphicsApis[i];
                                id++;
                            }
                        }
                        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, correctGraphicsApis);
                    },
                    error = false,
                },
#endif
                new ValidationRule(this)
                {
                    message = "Passthrough requires Camera clear flags set to solid color with alpha value zero.",
                    checkPredicate = () =>
                    {
                        var xrOrigin = FindAnyObjectByType<XROrigin>();
                        if (xrOrigin == null || !xrOrigin.enabled) return true;

                        var camera = xrOrigin.Camera;
                        if (camera == null || camera.GetComponent<ARCameraManager>() == null) return true;

                        return camera.clearFlags == CameraClearFlags.SolidColor && Mathf.Approximately(camera.backgroundColor.a, 0);
                    },
                    fixItAutomatic = true,
                    fixItMessage = "Set your XR Origin camera's Clear Flags to solid color with alpha value zero.",
                    fixIt = () =>
                    {
                        var xrOrigin = FindAnyObjectByType<XROrigin>();
                        if (xrOrigin != null || xrOrigin.enabled)
                        {
                            var camera = xrOrigin.Camera;
                            if (camera != null || camera.GetComponent<ARCameraManager>() != null)
                            {
                                camera.clearFlags = CameraClearFlags.SolidColor;
                                Color clearColor = camera.backgroundColor;
                                clearColor.a = 0;
                                camera.backgroundColor = clearColor;
                            }
                        }
                    },
                    error = false
                },
                new ValidationRule(this)
                {
                    message = "AR Camera Manager component should be enabled for Passthrough to function correctly.",
                    checkPredicate = () =>
                    {
                        var cameraManager = FindAnyObjectByType<ARCameraManager>();
                        return cameraManager != null && cameraManager.enabled;
                    },
                    fixItAutomatic = true,
                    fixItMessage = "Find the object with ARCameraManager component and enable it.",
                    fixIt = () =>
                    {
                        var cameraManager = FindAnyObjectByType<ARCameraManager>();
                        if (cameraManager != null)
                            cameraManager.enabled = true;
                    },
                    error = false
                }
            };

            rules.AddRange(AdditionalRules);
            rules.AddRange(SharedValidationRules.EnableARSessionValidationRules(this));
        }
#endif
    }
}
