using UnityEngine.XR.ARFoundation;

namespace UnityEditor.XR.OpenXR.Features.Meta
{
    [CustomEditor(typeof(ARCameraManager))]
    class ARCameraManagerEditor : Editor
    {
        const string k_FoldoutLabel = "Properties For Other Build Targets";

        bool m_ShowProperties;

        public override void OnInspectorGUI()
        {
            if (!XRManagerEditorUtility.IsMetaOpenXRTheActiveBuildTarget())
            {
                DrawDefaultInspector();
                return;
            }

            EditorStyles.helpBox.richText = true;

            EditorGUILayout.HelpBox(
                "This component controls Passthrough on Meta Quest devices. " +
                "Enable this component to enable Passthrough, and disable it to disable Passthrough. " +
                "Properties such as <b>Auto Focus</b> and <b>Facing Direction</b> have no effect on Meta Quest devices.",
                MessageType.Info);

            EditorStyles.helpBox.richText = false;

            EditorGUI.indentLevel++;

            m_ShowProperties = EditorGUILayout.Foldout(m_ShowProperties, k_FoldoutLabel);
            if (m_ShowProperties)
            {
                EditorGUI.indentLevel++;
                DrawDefaultInspector();
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
    }
}
