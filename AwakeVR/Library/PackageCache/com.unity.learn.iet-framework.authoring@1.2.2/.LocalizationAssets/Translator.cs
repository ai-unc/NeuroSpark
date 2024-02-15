//#define DEBUG_PRINTS

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.Tutorials.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Unity.NAME.Editor
{
    /// <summary>
    /// Monitors changes to PO files in the folder this file resides in and automatically
    /// applies the new translations.
    /// </summary>
    class POFilePostprocessor : AssetPostprocessor
    {
        static readonly string k_MonitoredPath =
            new DirectoryInfo(GetCurrentFilePath()).Parent.FullName
                .Replace("\\", "/")
                .Replace(Application.dataPath, "Assets");

        static string GetCurrentFilePath([System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => fileName;

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (importedAssets.Any(asset =>
                    asset.EndsWith(".po", StringComparison.OrdinalIgnoreCase) &&
                    asset.StartsWith(k_MonitoredPath))
                )
            {
                TutorialWindowUtils.ClearLocalizationCache();
                Translator.TranslateAll();
            }
        }
    }

    /// <summary>
    /// Translates the strings in the same assemby where the project's PO files reside.
    /// </summary>
    [InitializeOnLoad]
    static class Translator
    {
        static Translator()
        {
            // TODO Editor Localization bug: translation not working when InitializeOnLoad(Methods) are called
            // when the Editor is started (works when assemby reload happens after a script modification).
            // Must defer the translation calls to the first EditorApplication update.
            EditorApplication.update += DeferredTranslate;

            SubscribeToModifications();
        }

        static void DeferredTranslate()
        {
            EditorApplication.update -= DeferredTranslate;
            TranslateAll();
        }

        // NOTE msgid "Translate Current Project" must be in the project's PO files, not in IET packages' PO files.
        // "Tutorials" and "Localization" translations are provided by the IET packages.
        // NOTE MenuItem hidden for now as we have the automatic translation updates.
        //[MenuItem("Tutorials/Localization/Translate Current Project")]
        internal static void TranslateAll()
        {
            UnsubscribeFromModifications();

            FindAssets<TutorialWelcomePage>().ToList().ForEach(TranslateTutorialWelcomePage);
            FindAssets<TutorialContainer>().ToList().ForEach(TranslateTutorialContainer);
            FindAssets<Tutorial>().ToList().ForEach(TranslateTutorial);

            SubscribeToModifications();
        }

        static void SubscribeToModifications()
        {
            TutorialWelcomePage.TutorialWelcomePageModified.AddListener(TranslateTutorialWelcomePage);
            TutorialContainer.TutorialContainerModified.AddListener(TranslateTutorialContainer);
            Tutorial.TutorialModified.AddListener(TranslateTutorial);
            TutorialPage.TutorialPageNonMaskingSettingsChanged.AddListener(OnTutorialPageNonMaskingSettingsChanged);
        }

        static void UnsubscribeFromModifications()
        {
            TutorialWelcomePage.TutorialWelcomePageModified.RemoveListener(TranslateTutorialWelcomePage);
            TutorialContainer.TutorialContainerModified.RemoveListener(TranslateTutorialContainer);
            Tutorial.TutorialModified.RemoveListener(TranslateTutorial);
            TutorialPage.TutorialPageNonMaskingSettingsChanged.RemoveListener(OnTutorialPageNonMaskingSettingsChanged);
        }

        static void OnTutorialPageNonMaskingSettingsChanged(TutorialPage pg)
        {
            TranslateTutorialPage(pg);
        }

        static void TranslateTutorialWelcomePage(TutorialWelcomePage welcomePg)
        {
            int numNewTranslations = TranslateObject(welcomePg);
            foreach (var button in welcomePg.Buttons)
            {
                numNewTranslations += TranslateObject(button);
            }

            if (numNewTranslations > 0)
            {
                welcomePg.RaiseModified();
            }
        }

        static void TranslateTutorialContainer(TutorialContainer container)
        {
            int numNewTranslations = TranslateObject(container);
            foreach (var section in container.Sections)
            {
                numNewTranslations += TranslateObject(section);
            }

            if (numNewTranslations > 0)
            {
                container.RaiseModified();
            }
        }

        static void TranslateTutorial(Tutorial tutorial)
        {
            int numNewTranslations = TranslateObject(tutorial);
            foreach (var pg in tutorial.PagesCollection)
            {
                numNewTranslations += TranslateTutorialPage(pg);
            }

            if (numNewTranslations > 0)
            {
                tutorial.RaiseModified();
            }
        }

        static int TranslateTutorialPage(TutorialPage pg)
        {
            int numNewTranslations = TranslateObject(pg);
            foreach (var paragraph in pg.Paragraphs)
            {
                numNewTranslations += TranslateObject(paragraph);
            }

            if (numNewTranslations > 0)
            {
                pg.RaiseNonMaskingSettingsChanged();
            }

            return numNewTranslations;
        }

        static int TranslateObject(object obj)
        {
            const BindingFlags bindedTypes = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var localizableStringType = typeof(LocalizableString);
            int numNewTranslations = 0;
            obj.GetType().GetProperties(bindedTypes)
                .Where(pi => pi.PropertyType == localizableStringType && pi.CanWrite)
                .ToList()
                .ForEach(pi =>
                {
                    var str = pi.GetValue(obj) as LocalizableString;
                    var oldTranslation = str.Translated;
                    str.Translated = Localization.Tr(str.Untranslated);
#if DEBUG_PRINTS
                    Debug.Log($"was '{str.Untranslated}' is '{str.Translated}'");
#endif
                    pi.SetValue(obj, str);

                    if (oldTranslation.IsNotNullOrEmpty() && str.Translated != oldTranslation)
                    {
                        ++numNewTranslations;
                    }
                });

            obj.GetType().GetFields(bindedTypes)
                .Where(fi => fi.FieldType == localizableStringType)
                .ToList()
                .ForEach(fi =>
                {
                    var str = fi.GetValue(obj) as LocalizableString;
                    var oldTranslation = str.Translated;
                    str.Translated = Localization.Tr(str.Untranslated);
#if DEBUG_PRINTS
                    Debug.Log($"was '{str.Untranslated}' is '{str.Translated}'");
#endif
                    fi.SetValue(obj, str);

                    if (oldTranslation.IsNotNullOrEmpty() && str.Translated != oldTranslation)
                    {
                        ++numNewTranslations;
                    }
                });

            return numNewTranslations;
        }

        static IEnumerable<T> FindAssets<T>() where T : UnityEngine.Object =>
            AssetDatabase.FindAssets($"t:{typeof(T).FullName}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>);
    }
}
