using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using Unity.Tutorials.Core.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Unity.Tutorials.Authoring.Editor.Tests
{
    public class TutorialAssetUtilTests
    {
        const string k_LocalizationAssetsPath = "Assets/Localization";
        const string k_TempScenePath = "Assets/UntitledScene.unity";
        const string k_TempTutorialsFolderName = "IETTempTutorials";
        string TempTutorialsFolderPath => Path.Combine("Assets", k_TempTutorialsFolderName);

        // Returns the correct path depending whether we're running the tests locally or as a separate package.
        static string GetTestAssetPath(string relativeAssetPath)
        {
            var packagePath = PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly()).assetPath;
            return Path.Combine($"{packagePath}/Tests/Editor", relativeAssetPath);
        }

        [SetUp]
        public void SetUp()
        {
            // 2021.2.0 and newer will fail if we don't save the current scene
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), k_TempScenePath);
            Assert.That(File.Exists(BuildPath), Is.False, "Existing file at path " + BuildPath);
            Assert.That(Directory.Exists(BuildPath), Is.False, "Existing directory at path " + BuildPath);

            TutorialAssetsUtil.CreateLocalizationAssets(k_LocalizationAssetsPath);

            // For simplicity's sake, reuse the Localization folder and its asmdef for other scripts.
            const string templateFile = "TutorialCallbacks.cs";
            var templatePath = $"Packages/com.unity.learn.iet-framework/.TemplateAssets/{templateFile}";
            File.Copy(templatePath, Path.Combine(k_LocalizationAssetsPath, templateFile), overwrite: true);
            AssetDatabase.CreateFolder("Assets", k_TempTutorialsFolderName);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(BuildPath))
                File.Delete(BuildPath);

            if (Directory.Exists(BuildPath))
                Directory.Delete(BuildPath, true);

            DeleteAllFilesInFolder(k_TempTutorialsFolderName);
            AssetDatabase.DeleteAsset(k_LocalizationAssetsPath);
            AssetDatabase.DeleteAsset(k_TempScenePath);
        }

        static void DeleteAllFilesInFolder(string folderInAssets)
        {
            string[] targetFolders = { $"Assets/{folderInAssets}" };
            foreach (var asset in AssetDatabase.FindAssets(string.Empty, targetFolders))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(asset));
            }
            foreach (var folder in targetFolders)
            {
                AssetDatabase.DeleteAsset(folder);
            }
        }

        static string BuildPath
        {
            get
            {
                // NOTE Use "Builds" subfolder in order to make this test pass locally when
                // using Windows & Visual Studio
                const string buildName = "Builds/BuildPlayerTests_Build";
                if (Application.platform == RuntimePlatform.OSXEditor)
                    return buildName + ".app";
                return buildName;
            }
        }

        static BuildTarget BuildTarget
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                        return BuildTarget.StandaloneOSX;
                    case RuntimePlatform.WindowsEditor:
                        return BuildTarget.StandaloneWindows;
                    case RuntimePlatform.LinuxEditor:
                        // NOTE Universal & 32-bit Linux support dropped after 2018 LTS
                        return BuildTarget.StandaloneLinux64;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [Test]
        public void CreatePage_Narrative_HasCorrectSettings()
        {
            TutorialPage narrativePage = TutorialAssetsUtil.CreateTutorialPageWithNarrative($"{Path.Combine(TempTutorialsFolderPath, "NarrativePage")}");
            Assert.AreEqual(2, narrativePage.Paragraphs.Count);

            TutorialParagraph imageParagraph = narrativePage.Paragraphs[0];
            TutorialParagraph narrativeParagraph = narrativePage.Paragraphs[1];
            Assert.AreEqual(ParagraphType.Image, imageParagraph.Type);
            Assert.AreEqual(ParagraphType.Narrative, narrativeParagraph.Type);

            Assert.IsNotEmpty(narrativeParagraph.Title, "Narrative paragraphs have a default empty title, but they shouldn't");
            Assert.IsNotEmpty(narrativeParagraph.Text, "Narrative paragraphs have a default empty description, but they shouldn't");
        }

        [Test]
        public void CreatePage_Instructive_HasCorrectSettings()
        {
            TutorialPage instructivePage = TutorialAssetsUtil.CreateTutorialPageWithInstructions($"{Path.Combine(TempTutorialsFolderPath, "InstructivePage")}");
            Assert.AreEqual(3, instructivePage.Paragraphs.Count);

            TutorialParagraph imageParagraph = instructivePage.Paragraphs[0];
            TutorialParagraph narrativeParagraph = instructivePage.Paragraphs[1];
            TutorialParagraph instructiveParagraph = instructivePage.Paragraphs[2];
            Assert.AreEqual(ParagraphType.Image, imageParagraph.Type);
            Assert.AreEqual(ParagraphType.Narrative, narrativeParagraph.Type);
            Assert.AreEqual(ParagraphType.Instruction, instructiveParagraph.Type);

            Assert.IsNotEmpty(narrativeParagraph.Title, "Narrative paragraphs have a default empty title, but they shouldn't");
            Assert.IsNotEmpty(narrativeParagraph.Text, "Narrative paragraphs have a default empty description, but they shouldn't");
            Assert.IsNotEmpty(instructiveParagraph.Title, "Instructive paragraphs have a default empty title, but they shouldn't");
            Assert.IsNotEmpty(instructiveParagraph.Text, "Instructive paragraphs have a default empty description, but they shouldn't");
        }

        [Test]
        public void CreatePage_Switch_HasCorrectSettings()
        {
            TutorialPage tutorialSwitchPage = TutorialAssetsUtil.CreateTutorialPageWithSwitch($"{Path.Combine(TempTutorialsFolderPath, "SwitchPage")}", null);
            Assert.AreEqual(4, tutorialSwitchPage.Paragraphs.Count);

            TutorialParagraph imageParagraph = tutorialSwitchPage.Paragraphs[0];
            TutorialParagraph narrativeParagraph = tutorialSwitchPage.Paragraphs[1];
            TutorialParagraph instructiveParagraph = tutorialSwitchPage.Paragraphs[2];
            TutorialParagraph tutorialSwitchParagraph = tutorialSwitchPage.Paragraphs[3];
            Assert.AreEqual(ParagraphType.Image, imageParagraph.Type);
            Assert.AreEqual(ParagraphType.Narrative, narrativeParagraph.Type);
            Assert.AreEqual(ParagraphType.Instruction, instructiveParagraph.Type);
            Assert.AreEqual(ParagraphType.SwitchTutorial, tutorialSwitchParagraph.Type);

            Assert.IsNotEmpty(narrativeParagraph.Title, "Narrative paragraphs have a default empty title, but they shouldn't");
            Assert.IsNotEmpty(narrativeParagraph.Text, "Narrative paragraphs have a default empty description, but they shouldn't");
            Assert.IsNotEmpty(instructiveParagraph.Title, "Instructive paragraphs have a default empty title, but they shouldn't");
            Assert.IsNotEmpty(instructiveParagraph.Text, "Instructive paragraphs have a default empty description, but they shouldn't");
            Assert.IsNotEmpty(tutorialSwitchParagraph.Text, "Tutorial Switch paragraphs have a default empty description, but they shouldn't");
            Assert.IsNull(tutorialSwitchParagraph.m_Tutorial, "Tutorial Switch paragraphs have a non-empty tutorial target, but they shouldn't");
        }

        [Test]
        public void BuildPlayerWithTemplateAssets_Succeeds()
        {
            var buildTarget = BuildTarget;
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, buildTarget))
            {
                // Need to have this for Katana as it doesn't have Player building capabilities
                Assert.Pass();
            }

            var report = BuildPipeline.BuildPlayer(
                new BuildPlayerOptions
                {
                    scenes = new[] { GetTestAssetPath("EmptyTestScene.unity") }, // NOTE could probably pass 'null' here as well
                    locationPathName = BuildPath,
                    targetGroup = BuildTargetGroup.Standalone,
                    target = buildTarget,
                    options = BuildOptions.StrictMode,
                }
            );

            Assert.AreEqual(BuildResult.Succeeded, report.summary.result);
        }
    }
}
