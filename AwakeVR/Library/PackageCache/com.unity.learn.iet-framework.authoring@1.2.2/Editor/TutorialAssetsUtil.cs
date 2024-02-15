using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Tutorials.Core.Editor;

using PackageInfo = UnityEditor.PackageManager.PackageInfo;
using static Unity.Tutorials.Core.Editor.TutorialEditorUtils;

namespace Unity.Tutorials.Authoring.Editor
{
    using static Localization;

    internal static class TutorialAssetsUtil
    {
        const string k_Menu = "Assets/Create/Tutorials/";
        const string k_PageMenu = k_Menu + "Tutorial Page/";
        const string k_TutorialMenu = k_Menu + "Tutorial/";

        // Useful for reserializing assets after refactoring
        [MenuItem("Assets/Set Dirty", false, 100)]
        static void SetDirty()
        {
            foreach (UnityEngine.Object o in Selection.objects)
            {
                EditorUtility.SetDirty(o);
            }
        }

        /// <summary>
        /// Ensures that an asset changes are saved to disk
        /// </summary>
        /// <param name="asset"></param>
        static void EnsureAssetChangesAreSaved(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Generate a full tutorial system and flow
        /// </summary>
        /// <returns>The created TutorialWelcomePage, TutorialContainer and TutorialProjectSettings</returns>
        [MenuItem(k_Menu + "Ready-to-Use Tutorial Project")]
        static (TutorialWelcomePage, TutorialContainer, TutorialProjectSettings) CreateReadyToUseTutorialProject()
        {
            var path = GetActiveFolderPath();
            TutorialWelcomePage welcomePage = CreateTutorialWelcomePage($"{path}/Tutorial Welcome Page.asset");

            TutorialContainer container = CreateTutorialContainer($"{path}/Tutorials.asset");
            container.Title = "Title";
            container.Subtitle = "Subtitle";
            CreateTutorialFlow(null, container);

            TutorialProjectSettings tutorialProjectSettings = CreateTutorialProjectSettings($"{path}/Tutorial Project Settings.asset");
            var style = tutorialProjectSettings.TutorialStyle; //this triggers the loading of the default style
            tutorialProjectSettings.WelcomePage = welcomePage;

            EnsureAssetChangesAreSaved(container);

            return (welcomePage, container, tutorialProjectSettings);
        }

        /// <summary>
        /// Generate a tutorial with a minimum set of tutorial pages, ready to be used
        /// </summary>
        [MenuItem(k_TutorialMenu + "Ready-to-Use Tutorial")]
        static void CreateReadyToUseTutorial()
        {
            CreateTutorialFlow(null, null);
        }

        /// <summary>
        /// Generates a full tutorial with the least amount of tutorial pages possible, and adds it to the list of existing tutorials
        /// </summary>
        /// <param name="windowLayout">The layout the tutorial will use</param>
        /// <param name="container">The container to which this tutorial will be added</param>
        /// <returns>The created Tutorial</returns>
        static Tutorial CreateTutorialFlow(UnityEngine.Object windowLayout, TutorialContainer container)
        {
            var path = GetActiveFolderPath();
            TutorialPage startPage = CreateTutorialPageWithNarrative($"{path}/5-StartPage");
            TutorialPage instructivePage = CreateTutorialPageWithInstructions($"{path}/10-TutorialPage");
            TutorialPage tutorialSwitchPage = CreateTutorialPageWithSwitch($"{path}/15-LastPage", null);

            Tutorial tutorial = CreateEmptyTutorial($"{path}/New Tutorial.asset");

            // TODO: this would be the ideal solution to provide a more complete setup experience, 
            // but a bug with Scriptable object prevents Scriptable Objects created "after" to 
            // be referenced from Scriptable Objects created before them...

            //TutorialPage tutorialSwitchPage = CreateTutorialPageWithSwitch("15-LastPage", tutorial);
            Debug.LogWarning(
                Tr($"The created tutorial switch page doesn't have a Tutorial assiged. Please assign the '{tutorial.name}' object to the field 'Next Tutorial', in the Inspector of the object '{tutorialSwitchPage.name}'."),
                tutorialSwitchPage
            );

            tutorial.AddPage(startPage);
            tutorial.AddPage(instructivePage);
            tutorial.AddPage(tutorialSwitchPage);

            tutorial.TutorialTitle = "New Tutorial";
            tutorial.ProgressTrackingEnabled = true;
            tutorial.LessonId = Guid.NewGuid().ToString();

            if (container)
            {
                TutorialContainer.Section lastTutorial = null;
                if (container.Sections.Length > 0)
                {
                    lastTutorial = container.Sections.Where(s => !string.IsNullOrEmpty(s.TutorialId)).Last();
                }

                TutorialContainer.Section section = new TutorialContainer.Section();
                section.OrderInView = 0;
                section.Heading = "Tutorial title";
                section.Text = "Tutorial description";
                section.Tutorial = tutorial;

                TutorialContainer.Section[] updatedTutorials = new TutorialContainer.Section[container.Sections.Length + 1];
                Array.Copy(container.Sections, updatedTutorials, container.Sections.Length);
                updatedTutorials[updatedTutorials.Length - 1] = section;
                container.Sections = updatedTutorials;
            }

            if (windowLayout)
            {
                tutorial.WindowLayout = windowLayout;
            }
            else
            {
                Debug.LogWarning(Tr($"The tutorial '{tutorial.name}' does not have a Window Layout assigned. Please assign one to its 'Window Layout' field in the Inspector if you want to load a new Window Layout when this tutorial starts."), tutorial);
            }
            tutorial.Version = "1";

            EnsureAssetChangesAreSaved(tutorial);
            Debug.LogWarning(
                Tr($"The tutorial '{tutorial.name}' does not have a Scene assigned. Please assign one to its 'Scene' field in the Inspector if you want to load a specific Scene when this tutorial starts"),
                tutorial
            );
            return tutorial;
        }

        // NOTE: typically two functions are provided for creating a particular tutorial asset:
        // 1) CreateXXX() -- creates an asset to currently active path and expects the user to input a new name for the asset
        // 2) CreateXXX(string assetPath) -- creates an asset to arbitrary path, does not expect any user input

        [MenuItem(k_TutorialMenu + "Empty Tutorial")]
        static Tutorial CreateEmptyTutorial() => CreateEmptyTutorial(null);
        static Tutorial CreateEmptyTutorial(string assetPath)
        {
            if (assetPath == null)
            {
                CreateAssetAndStartRenaming<Tutorial>("New Tutorial.asset");
                return null;
            }
            else
            {
                return CreateAsset<Tutorial>(assetPath);
            }
        }

        [MenuItem(k_PageMenu + "Page with Narrative")]
        static TutorialPage CreateTutorialPageWithNarrative() => CreateTutorialPageWithNarrative(null);
        internal static TutorialPage CreateTutorialPageWithNarrative(string assetPath)
        {
            return CreateTutorialPage(assetPath,
                    TutorialParagraph.CreateImageParagraph(),
                    TutorialParagraph.CreateNarrativeParagraph("Page title", "Put your tutorial narrative description here")
            );
        }

        [MenuItem(k_PageMenu + "Page with Narrative and Instruction")]
        static TutorialPage CreateTutorialPageWithInstructions() => CreateTutorialPageWithInstructions(null);
        internal static TutorialPage CreateTutorialPageWithInstructions(string assetPath)
        {
            return CreateTutorialPage(assetPath,
                    TutorialParagraph.CreateImageParagraph(),
                    TutorialParagraph.CreateNarrativeParagraph("Page title", "Put your tutorial narrative description here"),
                    TutorialParagraph.CreateInstructionParagraph("Instruction title", "Put your tutorial instructions here")
            );
        }

        [MenuItem(k_PageMenu + "Page with Narrative, Instruction, and Tutorial Switch")]
        static TutorialPage CreateTutorialPageWithSwitch() => CreateTutorialPageWithSwitch(null, null);
        internal static TutorialPage CreateTutorialPageWithSwitch(string assetPath, Tutorial nextTutorial)
        {
            return CreateTutorialPage(assetPath,
                    TutorialParagraph.CreateImageParagraph(),
                    TutorialParagraph.CreateNarrativeParagraph("Page title", "Put your tutorial narrative description here"),
                    TutorialParagraph.CreateInstructionParagraph("Instruction title", "Put your tutorial instructions here"),
                    TutorialParagraph.CreateTutorialSwitchParagraph(nextTutorial, "Tutorial X: Make the best game!")
            );
        }

        static TutorialPage CreateTutorialPage(string assetPath, params TutorialParagraph[] paragraphs)
        {
            return CreateTutorialPageAsset(TutorialPage.Create(paragraphs), assetPath);
        }

        static TutorialPage CreateTutorialPageAsset(TutorialPage page, string assetPath)
        {
            if (assetPath == null)
            {
                CreateAssetAndStartRenaming("New Tutorial Page.asset", page);
            }
            else
            {
                if (!assetPath.EndsWith(".asset"))
                {
                    assetPath += ".asset";
                }
                CreateAsset(assetPath, page);
            }
            return page;
        }

        [MenuItem(k_Menu + "Tutorial Styles")]
        static TutorialStyles CreateTutorialStyles() => CreateTutorialStyles(null);
        static TutorialStyles CreateTutorialStyles(string assetPath)
        {
            if (assetPath == null)
            {
                CreateAssetAndStartRenaming<TutorialStyles>("New Tutorial Styles.asset");
                return null;
            }
            else

            {
                return CreateAsset<TutorialStyles>(assetPath);
            }
        }

        [MenuItem(k_Menu + "Light Tutorial Style Sheet")]
        static void CreateLightTutorialStyleSheet()
        {
            CreateAssetWithContentAndStartRenaming<StyleSheet>("New Light Style.uss", File.ReadAllText(TutorialStyles.DefaultLightStyleFile));
        }

        [MenuItem(k_Menu + "Dark Tutorial Style Sheet")]
        static void CreateDarkTutorialStyleSheet()
        {
            CreateAssetWithContentAndStartRenaming<StyleSheet>("New Dark Style.uss", File.ReadAllText(TutorialStyles.DefaultDarkStyleFile));
        }

        [MenuItem(k_Menu + "Tutorial Welcome Page")]
        static TutorialWelcomePage CreateTutorialWelcomePage() => CreateTutorialWelcomePage(null);
        static TutorialWelcomePage CreateTutorialWelcomePage(string assetPath)
        {
            var asset = ScriptableObject.CreateInstance<TutorialWelcomePage>();
            asset.m_WindowTitle = "Window Title";
            asset.m_Title = "This Is the Title";
            asset.m_Description = "This is the description.";
            asset.m_Buttons = new[] { TutorialWelcomePage.CreateCloseButton(asset) };
            if (assetPath == null)
            {
                CreateAssetAndStartRenaming("New Tutorial Welcome Page.asset", asset);
                return null;
            }
            else
            {
                CreateAsset(assetPath, asset);
            }
            return asset;
        }

        [MenuItem(k_Menu + "Tutorial Project Settings")]
        static TutorialProjectSettings CreateTutorialProjectSettings() => CreateTutorialProjectSettings(null);
        static TutorialProjectSettings CreateTutorialProjectSettings(string assetPath)
        {
            TutorialProjectSettings asset = null;
            if (assetPath == null)
            {
                CreateAssetAndStartRenaming<TutorialProjectSettings>("Tutorial Project Settings.asset");
            }
            else
            {
                asset = CreateAsset<TutorialProjectSettings>(assetPath);
            }
            TutorialProjectSettings.Instance = asset;
            return asset;
        }

        [MenuItem(k_Menu + "Tutorial Container")]
        static TutorialContainer CreateTutorialContainer() => CreateTutorialContainer(null);
        static TutorialContainer CreateTutorialContainer(string assetPath)
        {
            if (assetPath == null)
            {
                CreateAssetAndStartRenaming<TutorialContainer>("Tutorials.asset");
                return null;
            }
            else
            {
                return CreateAsset<TutorialContainer>(assetPath);
            }
        }

        [MenuItem(k_Menu + "Localization Assets", priority = 1011)]
        internal static void CreateLocalizationAssets() => CreateLocalizationAssets($"{GetActiveFolderPath()}/Localization");
        internal static void CreateLocalizationAssets(string path)
        {
            try
            {
                UserStartupCode.DirectoryCopy(
                    $"{PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly()).assetPath}/.LocalizationAssets",
                    path
                );
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [MenuItem(k_Menu + "Callback Handler", priority = 1012)]
        static void CreatePageCallbackHandler()
        {
            TutorialPageEditor.CreateCallbackHandlerScript("TutorialCallbacks.cs", GetActiveFolderPath());
        }

        static void CreateAssetAndStartRenaming<T>(string assetName) where T : ScriptableObject
        {
            CreateAssetAndStartRenaming(assetName, ScriptableObject.CreateInstance<T>());
        }

        static void CreateAssetAndStartRenaming(string assetName, UnityEngine.Object asset)
        {
            ProjectWindowUtil.CreateAsset(asset, $"{GetActiveFolderPath()}/{assetName}");
        }

        static void CreateAssetWithContentAndStartRenaming<T>(string assetName, string content) where T : ScriptableObject
        {
            CreateAssetWithContentAndStartRenaming(assetName, content, EditorGUIUtilityProxy.FindTexture(typeof(T)));
        }

        static void CreateAssetWithContentAndStartRenaming(string assetName, string content, Texture2D icon)
        {
            ProjectWindowUtil.CreateAssetWithContent($"{GetActiveFolderPath()}/{assetName}", content, icon);
        }

        // Creates a new instance of an asset to the wanted path.
        static T CreateAsset<T>(string assetPath) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            CreateAsset(assetPath, asset);
            return asset;
        }

        // Creates an existing instance of an asset to the wanted path.
        static void CreateAsset(string assetPath, UnityEngine.Object asset)
        {
            AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        }

        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Save Current Layout to Asset...")]
        static void SaveCurrentLayoutToAsset()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Layout", "layout.wlt", "wlt", "Choose the location to save the layout");
            if (path.Length != 0)
            {
                WindowLayoutProxy.SaveWindowLayout(path);
                AssetDatabase.Refresh();
            }
        }

        [MenuItem(MenuItems.AuthoringMenuPath + "Layout/Load Layout...")]
        static void LoadLayout()
        {
            string path = EditorUtility.OpenFilePanelWithFilters("Open Layout", "", new[] { "Layout files", "wlt,dwlt" });
            if (path.Length != 0)
            {
                TutorialModel.LoadWindowLayout(path);
            }
        }

        //[MenuItem(TutorialWindowMenuItem.MenuPath + "Masking/Remove TutorialWindow")]
        static void RemoveTutorialWindowFromMaskingSettings()
        {
            foreach (TutorialPage page in Selection.objects)
            {
                if (page == null)
                    continue;

                var tutorialWindowTypeName = typeof(TutorialWindow).AssemblyQualifiedName;

                var so = new SerializedObject(page);
                var paragraphs = so.FindProperty("m_Paragraphs.m_Items");
                for (int parIdx = 0; parIdx < paragraphs.arraySize; ++parIdx)
                {
                    var paragraph = paragraphs.GetArrayElementAtIndex(parIdx);
                    var unmaskedViews = paragraph.FindPropertyRelative("m_MaskingSettings.m_UnmaskedViews");
                    for (var viewIdx = unmaskedViews.arraySize - 1; viewIdx >= 0; --viewIdx)
                    {
                        var editorWindowTypeName =
                            unmaskedViews.GetArrayElementAtIndex(viewIdx).FindPropertyRelative("m_EditorWindowType.m_TypeName");
                        if (editorWindowTypeName.stringValue == tutorialWindowTypeName)
                        {
                            unmaskedViews.DeleteArrayElementAtIndex(viewIdx);
                        }
                    }
                }
                so.ApplyModifiedProperties();
            }
        }

        //[MenuItem(TutorialWindowMenuItem.MenuPath + "Masking/Configure Play Button")]
        static void AddPlayButtonControlMaskingToParagraphsWithPlayModeCriteria()
        {
            const string playButtonName = "ToolbarPlayModePlayButton";

            foreach (TutorialPage page in Selection.objects)
            {
                if (page == null)
                    continue;

                var so = new SerializedObject(page);
                var paragraphs = so.FindProperty("m_Paragraphs.m_Items");
                for (int parIdx = 0; parIdx < paragraphs.arraySize; ++parIdx)
                {
                    var paragraph = paragraphs.GetArrayElementAtIndex(parIdx);
                    var criteria = paragraph.FindPropertyRelative("m_Criteria.m_Items");
                    if (
                        Enumerable.Range(0, criteria.arraySize).Select(
                            i => criteria.GetArrayElementAtIndex(i).FindPropertyRelative("criterion").objectReferenceValue).Any(c => c is PlayModeStateCriterion)
                    )
                    {
                        var unmaskedViews = paragraph.FindPropertyRelative("m_MaskingSettings.m_UnmaskedViews");
                        var configured = false;
                        var toolbarIdx = -1;
                        for (int viewIdx = 0; viewIdx < unmaskedViews.arraySize; ++viewIdx)
                        {
                            var unmaskedView = unmaskedViews.GetArrayElementAtIndex(viewIdx);
                            var viewTypeName = unmaskedView.FindPropertyRelative("m_ViewType.m_TypeName");
                            if (viewTypeName.stringValue != GUIViewProxy.ToolbarType.AssemblyQualifiedName)
                                continue;

                            toolbarIdx = viewIdx;
                            var unmaskedControls = unmaskedView.FindPropertyRelative("m_UnmaskedControls");
                            for (int ctlIdx = 0; ctlIdx < unmaskedControls.arraySize; ++ctlIdx)
                            {
                                var control = unmaskedControls.GetArrayElementAtIndex(ctlIdx);
                                if (
                                    control.FindPropertyRelative("m_ControlName").stringValue == playButtonName &&
                                    control.FindPropertyRelative("m_SelectorMode").intValue == (int)GuiControlSelector.Mode.NamedControl
                                )
                                {
                                    configured = true;
                                    break;
                                }
                            }
                            if (configured)
                                break;
                        }

                        if (!configured)
                        {
                            SerializedProperty unmaskedToolbar;
                            if (toolbarIdx < 0)
                            {
                                unmaskedViews.InsertArrayElementAtIndex(unmaskedViews.arraySize);
                                unmaskedToolbar = unmaskedViews.GetArrayElementAtIndex(toolbarIdx);
                            }
                            else
                            {
                                unmaskedToolbar = unmaskedViews.GetArrayElementAtIndex(toolbarIdx);
                            }
                            var toolbarControls = unmaskedToolbar.FindPropertyRelative("m_UnmaskedControls");
                            toolbarControls.InsertArrayElementAtIndex(toolbarControls.arraySize);
                            var control = toolbarControls.GetArrayElementAtIndex(toolbarControls.arraySize - 1);
                            control.FindPropertyRelative("m_ControlName").stringValue = playButtonName;
                            control.FindPropertyRelative("m_SelectorMode").intValue = (int)GuiControlSelector.Mode.NamedControl;
                        }
                    }
                }
                so.ApplyModifiedProperties();
            }
        }
    }
}
