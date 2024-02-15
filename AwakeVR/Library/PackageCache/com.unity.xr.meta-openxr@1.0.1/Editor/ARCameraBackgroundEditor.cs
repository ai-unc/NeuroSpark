using UnityEngine.XR.ARFoundation;

namespace UnityEditor.XR.OpenXR.Features.Meta
{
    [CustomEditor(typeof(ARCameraBackground))]
    class ARCameraBackgroundEditor : Editor
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

            EditorGUILayout.HelpBox(
                "This component has no effect on Meta Quest devices.",
                MessageType.Info);

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
