using UnityEngine.XR.OpenXR.Features.Meta;
using UnityEngine.XR.OpenXR.Features.MetaQuestSupport;

namespace UnityEditor.XR.OpenXR.Features.Meta
{
    [OpenXRFeatureSet(
        FeatureIds = new[]
        {
            ARSessionFeature.featureId,
            ARAnchorFeature.featureId,
            ARCameraFeature.featureId,
            ARPlaneFeature.featureId,
            ARRaycastFeature.featureId,
            MetaQuestFeature.featureId,
            DisplayUtilitiesFeature.featureId,
        },
        DefaultFeatureIds = new []
        {
            ARSessionFeature.featureId,
            ARAnchorFeature.featureId,
            ARCameraFeature.featureId,
            ARPlaneFeature.featureId,
            ARRaycastFeature.featureId,
            MetaQuestFeature.featureId,
            DisplayUtilitiesFeature.featureId,
        },
        RequiredFeatureIds = new[]
        {
            MetaQuestFeature.featureId,
        },
        UiName = "Meta Quest",
        FeatureSetId = featureSetId,
        SupportedBuildTargets = new[] { BuildTargetGroup.Android }
    )]
    class MetaFeatureSet {
        const string featureSetId = "com.unity.openxr.featureset.meta";
    }
}
