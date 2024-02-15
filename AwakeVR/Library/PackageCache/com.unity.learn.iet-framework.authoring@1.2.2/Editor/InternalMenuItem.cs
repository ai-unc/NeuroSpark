using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Unity.Tutorials.Core.Editor;

using static Unity.Tutorials.Core.Editor.POFileUtils;

namespace Unity.Tutorials.Authoring.Editor
{
    //note: localization does not work when called from this class
    static class InternalMenuItem
    {
        [MenuItem(MenuItems.AuthoringMenuPath + "Debug/Restart Editor")]
        static void RestartEditor() => UserStartupCode.RestartEditor();

        [MenuItem(MenuItems.AuthoringMenuPath + "Localization/Extract Localizable Strings for the Project...")]
        static void ExtractLocalizableStrings()
        {
            string poFolderPath = EditorUtility.OpenFolderPanel(
                "Choose Folder for the Translation Files",
                Application.dataPath,
                string.Empty
            );
            if (poFolderPath.IsNullOrEmpty())
            {
                return;
            }
            else if (!ContainsTranslatorFile(poFolderPath))
            {
                Debug.LogError("Could not find Translator.cs in the selected folder. Did you create Localization Assets through the  'Create > Tutorials > Localization Assets' menu already? Full instructions here: https://docs.unity3d.com/Packages/com.unity.learn.iet-framework.authoring@1.0/manual/authoring-guide.html#rich-text-support");
                return;
            }

            if (SupportedLanguages.Values.Any(code => File.Exists($"{poFolderPath}/{code}.po")))
            {
                var title = "Localization";
                var msg = "The folder contains translation files already, they will be overwritten.";
                var ok = "Continue";
                var cancel = "Cancel";
                if (!EditorUtility.DisplayDialog(title, msg, ok, cancel))
                    return;
            }

            var entries = new List<POEntry>();

            foreach (var container in FindAssets<TutorialContainer>())
            {
                entries.AddRange(ExtractFieldsAndPropertiesForLocalization(container));
                foreach (var section in container.Sections)
                {
                    entries.AddRange(ExtractFieldsAndPropertiesForLocalization(section));
                }
            }

            foreach (var welcomePg in FindAssets<TutorialWelcomePage>())
            {
                entries.AddRange(ExtractFieldsAndPropertiesForLocalization(welcomePg));

                foreach (var button in welcomePg.Buttons)
                {
                    entries.AddRange(ExtractFieldsAndPropertiesForLocalization(button));
                }
            }

            foreach (var tutorial in FindAssets<Tutorial>())
                entries.AddRange(ExtractFieldsAndPropertiesForLocalization(tutorial));

            foreach (var pg in FindAssets<TutorialPage>())
            {
                entries.AddRange(ExtractFieldsAndPropertiesForLocalization(pg));

                foreach (var paragraph in pg.Paragraphs)
                {
                    entries.AddRange(ExtractFieldsAndPropertiesForLocalization(paragraph));
                }
            }

            // Get rid of duplicate msgids, omit empty msgids as they can cause problems for some PO editors
            var uniqueEntries = entries
                .Where(entry => entry.IsValid())
                .GroupBy(entry => entry.ID)
                .Select(group => group.FirstOrDefault());

            foreach (var code in SupportedLanguages.Values)
            {
                WritePOFile(Application.productName, Application.version, code, uniqueEntries, $"{poFolderPath}/{code}.po");
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static bool ContainsTranslatorFile(string directory)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                if (Path.GetFileName(file) == "Translator.cs")
                {
                    return true;
                }
            }
            return false;
        }

        static void DEV_ConvertIETLocalizationFilesFromV2ToV3()
        {
            POFileUtils.ConvertIETLocalizationFileFromV2ToV3("ja");
            POFileUtils.ConvertIETLocalizationFileFromV2ToV3("ko");
            POFileUtils.ConvertIETLocalizationFileFromV2ToV3("zh-hans");
            POFileUtils.ConvertIETLocalizationFileFromV2ToV3("zh-hant");
        }

        /// <summary>
        /// This method is used by IET developers when they update the english file and want to sync other files accordingly.
        /// It is not supposed to be used by normal users.
        /// Uncomment when needed (I.E: before a release)
        /// </summary>
        //[MenuItem(MenuItems.AuthoringMenuPath + "Localization/(DEV) Sync IET Localization Files With English")]
        static void DEV_SyncIETLocalizationFilesWithEnglish()
        {
            POFileUtils.SyncIETLocalizationFileWithEnglish("ja");
            POFileUtils.SyncIETLocalizationFileWithEnglish("ko");
            POFileUtils.SyncIETLocalizationFileWithEnglish("zh-hans");
            POFileUtils.SyncIETLocalizationFileWithEnglish("zh-hant");
        }

        [MenuItem(MenuItems.AuthoringMenuPath + "Debug/Run Startup Code")]
        static void ExecuteFirstLaunchExperience()
        {
            UserStartupCode.RunStartupCode(TutorialProjectSettings.Instance);
        }

        // These resolutions cover 87 % of our new macOS and Windows users (spring 2020)
        // <resolution> -- <ratio>
        // 1366 x 768   ‭-- 1.778645833333333‬
        // 1440 x 900   -- 1.6
        // 1600 x 900   ‭-- 1.777777777777778‬
        // 1680 x 1050  -- 1.6
        // 1280 x 800   -- 1.6
        // 1920 x 1080  -- ‭1.777777777777778‬
        // 2560 x 1440  -- ‭1.777777777777778‬
        // 3840 x 2160  -- ‭1.777777777777778‬
        // Looking at the ratios, we could maybe drop the many of these and keep only single one of each ration.
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/1366 x 768")]
        static void SetWindowSize_1366x768() => SetMainWindowSize(1366, 768);
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/1440 x 900")]
        static void SetWindowSize_1440x900() => SetMainWindowSize(1440, 900);
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/1600 x 900")]
        static void SetWindowSize_1600x900() => SetMainWindowSize(1600, 900);
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/1680 x 1050")]
        static void SetWindowSize_1680x1050() => SetMainWindowSize(1680, 1050);
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/1280 x 800")]
        static void SetWindowSize_1280x800() => SetMainWindowSize(1280, 800);
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/1920 x 1080")]
        static void SetWindowSize_1920x1080() => SetMainWindowSize(1920, 1080);
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/2560 x 1440")]
        static void SetWindowSize_2560x1440() => SetMainWindowSize(2560, 1440);
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/3840 x 2160")]
        static void SetMainWindowSize_3840x2160() => SetMainWindowSize(3840, 2160);
        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Window Size/Arbitrary...")]
        static void SetMainWindowSize_Arbitrary() => SetWindowSizeDialog.ShowAsUtility();

        internal static void SetMainWindowSize(int w, int h)
        {
            var pos = EditorWindowUtils.GetEditorMainWindowPos();
            pos.width = w;
            pos.height = h;
            EditorWindowUtils.SetEditorMainWindowPos(pos);
        }

        static IEnumerable<T> FindAssets<T>() where T : Object =>
            AssetDatabase.FindAssets($"t:{typeof(T).FullName}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>);

        static IEnumerable<POEntry> ExtractFieldsAndPropertiesForLocalization(object obj)
        {
            const BindingFlags bindedTypes = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var localizableStringType = typeof(LocalizableString);

            var localizableProps = obj.GetType().GetProperties(bindedTypes)
                .Where(pi => pi.PropertyType == localizableStringType && pi.CanWrite)
                .Select(pi => new POEntry
                {
                    Reference = $"{obj.GetType().Name}.{pi.Name}",
                    ID = (pi.GetValue(obj) as LocalizableString).Untranslated
                });

            var localizableFields = obj.GetType().GetFields(bindedTypes)
                .Where(fi => fi.FieldType == localizableStringType)
                .Select(fi => new POEntry
                {
                    Reference = $"{obj.GetType().Name}.{fi.Name}",
                    ID = (fi.GetValue(obj) as LocalizableString).Untranslated
                });

            return localizableProps.Concat(localizableFields);
        }
    }

    class SetWindowSizeDialog : EditorWindow
    {
        int m_MainWinWidth, m_MainWinHeight;

        public static void ShowAsUtility()
        {
            var wnd = CreateInstance<SetWindowSizeDialog>();
            EditorWindowUtils.CenterOnMainWindow(wnd);
            wnd.ShowUtility();
        }

        void OnEnable()
        {
            titleContent.text = "Set Window Size";
            minSize = maxSize = new Vector2(300, 100);
        }

        void OnGUI()
        {
            m_MainWinWidth = EditorGUILayout.IntField("Width", m_MainWinWidth);
            // the min. width and height for main window size found by trial-and-error
            m_MainWinWidth = Mathf.Max(875, m_MainWinWidth);
            m_MainWinHeight = EditorGUILayout.IntField("Height", m_MainWinHeight);
            m_MainWinHeight = Mathf.Max(542, m_MainWinHeight);

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Set"))
            {
                InternalMenuItem.SetMainWindowSize(m_MainWinWidth, m_MainWinHeight);
            }

            GUILayout.FlexibleSpace();
        }
    }
}
