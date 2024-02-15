using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Unity.Tutorials.Core.Editor;

namespace Unity.Tutorials.Authoring.Editor
{
    // Initialize on load to surface potential reflection issues immediately
    [InitializeOnLoad]
    static class TutorialExporter
    {
        const string k_PackagesDirector = "Packages";
        const string k_ManifestPath = "Packages/manifest.json";
        const string k_ManifestTempPath = "Packages/original-manifest.json";
        const string k_ExportedManifestPath = "Packages/exported-manifest.json";
        const string k_RiderPluginPath = "Assets/Plugins/Editor/JetBrains";

        static MethodInfo s_ExportPackageAndPackageManagerManifestMethod;
        static string s_PackagePath;
        static DateTime s_PackagePathLastWriteTime;
        static Action<bool> s_ExportPackageCallback;

        public static bool exportInProgress { get; private set; }

        static TutorialExporter()
        {
            const string typeName = "UnityEditor.PackageUtility";
            var packageUtilityType = typeof(PackageInfo).Assembly.GetType(typeName);
            if (packageUtilityType == null)
            {
                Debug.LogError($"Cannot find type {typeName}");
                return;
            }

            const string methodName = "ExportPackageAndPackageManagerManifest";
            s_ExportPackageAndPackageManagerManifestMethod = packageUtilityType.GetMethod(methodName,
                BindingFlags.Static | BindingFlags.Public);
            if (s_ExportPackageAndPackageManagerManifestMethod == null)
                Debug.LogError($"Cannot find method {methodName} on type {typeName}");
        }

        static bool ExportPackageAndPackageManagerManifest(string[] assetPathNames, string fileName)
        {
            // Convert asset paths to GUIDs
            var guids = new string[assetPathNames.Length];
            for (var i = 0; i < assetPathNames.Length; i++)
            {
                var assetPath = assetPathNames[i];
                var guid = AssetDatabase.AssetPathToGUID(assetPath);
                if (string.IsNullOrEmpty(guid))
                {
                    Debug.LogError($"Invalid GUID for asset path: {assetPath}");
                    return false;
                }

                guids[i] = guid;
            }

            s_ExportPackageAndPackageManagerManifestMethod.Invoke(null, new object[] { guids, fileName });

            return true;
        }

        internal static void ExportPackageForTutorial(string packagePath, Tutorial tutorial, IEnumerable<string> additionalAssets, Action<bool> callback)
        {
            if (exportInProgress)
            {
                Debug.LogError($"Package export already in progress.");
                callback(false);
                return;
            }

            if (!File.Exists(k_ManifestPath))
            {
                Debug.LogError($"Could not find package manifest: {k_ManifestPath}");
                callback(false);
                return;
            }

            if (File.Exists(k_ManifestTempPath))
            {
                Debug.LogError($"Existing file at temporary manifest path: {k_ManifestTempPath}");
                callback(false);
                return;
            }

            if (!File.Exists(k_ExportedManifestPath))
            {
                Debug.LogError($"Could not find exported package manifest: {k_ExportedManifestPath}");
                callback(false);
                return;
            }

            EditorApplication.LockReloadAssemblies();

            var packageAssets = new HashSet<string>();

            // Include tutorial asset and its dependencies
            var tutorialAssetPath = AssetDatabase.GetAssetPath(tutorial);
            var tutorialDependencies = AssetDatabase.GetDependencies(tutorialAssetPath);
            packageAssets.Add(tutorialAssetPath);
            packageAssets.UnionWith(tutorialDependencies);

            // Include any additional assets
            packageAssets.UnionWith(additionalAssets);

            // Exclude assets in packages
            packageAssets.RemoveWhere(path => path.StartsWith(k_PackagesDirector + "/"));

            // Exclude Rider plugin
            packageAssets.RemoveWhere(path => path.StartsWith(k_RiderPluginPath + "/"));

            // Prepare exported package manifest
            File.Move(k_ManifestPath, k_ManifestTempPath);
            File.Copy(k_ExportedManifestPath, k_ManifestPath);

            // Record last write time to potential existing file at output path
            var packagePathLastWriteTime = new DateTime(0);
            if (File.Exists(packagePath))
                packagePathLastWriteTime = File.GetLastWriteTime(packagePath);

            if (!ExportPackageAndPackageManagerManifest(packageAssets.ToArray(), packagePath))
            {
                EditorApplication.UnlockReloadAssemblies();
                callback(false);
                return;
            }

            // Exporting package is asynchronous
            // Wait until the package has been written to disk before restoring package manifest
            exportInProgress = true;
            s_PackagePath = packagePath;
            s_PackagePathLastWriteTime = packagePathLastWriteTime;
            s_ExportPackageCallback = callback;

            EditorApplication.update += CleanUpWhenPackageHasBeenExported;
        }

        static void CleanUpWhenPackageHasBeenExported()
        {
            if (File.Exists(s_PackagePath) && File.GetLastWriteTime(s_PackagePath) > s_PackagePathLastWriteTime)
            {
                EditorApplication.update -= CleanUpWhenPackageHasBeenExported;

                // Restore package manifest
                File.Delete(k_ManifestPath);
                File.Move(k_ManifestTempPath, k_ManifestPath);

                EditorApplication.UnlockReloadAssemblies();

                exportInProgress = false;
                s_ExportPackageCallback(true);
            }
        }
    }
}
