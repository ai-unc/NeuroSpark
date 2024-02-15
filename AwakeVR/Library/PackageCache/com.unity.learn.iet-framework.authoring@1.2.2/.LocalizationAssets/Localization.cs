#if UNITY_2020_1_OR_NEWER
[assembly: UnityEditor.Localization]
#elif UNITY_2019_3_OR_NEWER
[assembly: UnityEditor.Localization.Editor.Localization]
#endif

namespace Unity.NAME.Editor
{
    static class Localization
    {
        public static string Tr(string str) =>
#if UNITY_2020_1_OR_NEWER
            UnityEditor.L10n.Tr(str);
#elif UNITY_2019_3_OR_NEWER
            UnityEditor.Localization.Editor.Localization.Tr(str);
#else
            str;
#endif
    }
}
