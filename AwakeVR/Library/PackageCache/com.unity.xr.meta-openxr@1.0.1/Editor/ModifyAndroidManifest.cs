using System;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Build.Reporting;

using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features.Meta;

namespace UnityEditor.XR.OpenXR.Features.Meta
{
    internal class ModifyAndroidManifestMeta : OpenXRFeatureBuildHooks
    {
        public override int callbackOrder => 1;

        public override Type featureType => typeof(ARSessionFeature);

        protected override void OnPreprocessBuildExt(BuildReport report)
        {
        }

        protected override void OnPostGenerateGradleAndroidProjectExt(string path)
        {
            var androidManifest = new AndroidManifest(GetManifestPath(path));
            androidManifest.AddMetaData();
            androidManifest.Save();
        }

        protected override void OnPostprocessBuildExt(BuildReport report)
        {
        }

        string _manifestFilePath;

        string GetManifestPath(string basePath)
        {
            if (!string.IsNullOrEmpty(_manifestFilePath)) return _manifestFilePath;

            var pathBuilder = new StringBuilder(basePath);
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
            _manifestFilePath = pathBuilder.ToString();

            return _manifestFilePath;
        }

        class AndroidXmlDocument : XmlDocument
        {
            string m_Path;
            protected XmlNamespaceManager nsMgr;
            public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";

            public AndroidXmlDocument(string path)
            {
                m_Path = path;
                using (var reader = new XmlTextReader(m_Path))
                {
                    reader.Read();
                    Load(reader);
                }

                nsMgr = new XmlNamespaceManager(NameTable);
                nsMgr.AddNamespace("android", AndroidXmlNamespace);
            }

            public string Save()
            {
                return SaveAs(m_Path);
            }

            public string SaveAs(string path)
            {
                using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
                {
                    writer.Formatting = Formatting.Indented;
                    Save(writer);
                }

                return path;
            }
        }

        class AndroidManifest : AndroidXmlDocument
        {
            readonly XmlElement ManifestElement;

            public AndroidManifest(string path) : base(path)
            {
                ManifestElement = SelectSingleNode("/manifest") as XmlElement;
            }

            void UpdateOrCreateAttribute(XmlElement xmlParentElement, string tag, string name, params (string name, string value)[] attributes)
            {
                var xmlNodeList = xmlParentElement.SelectNodes(tag);
                XmlElement targetNode = null;

                // Check all XmlNodes to see if a node with matching name already exists.
                foreach (XmlNode node in xmlNodeList)
                {
                    XmlAttribute nameAttr = (XmlAttribute)node.Attributes.GetNamedItem("name", AndroidXmlNamespace);
                    if (nameAttr != null && nameAttr.Value.Equals(name))
                    {
                        targetNode = (XmlElement)node;
                        break;
                    }
                }

                // If node exists, update the attribute values if they are present or create new ones as requested. Else, create new XmlElement.
                if (targetNode != null)
                {
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        XmlAttribute attr = (XmlAttribute)targetNode.Attributes.GetNamedItem(attributes[i].name, AndroidXmlNamespace);
                        if (attr != null)
                        {
                            attr.Value = attributes[i].value;
                        }
                        else
                        {
                            targetNode.SetAttribute(attributes[i].name, AndroidXmlNamespace, attributes[i].value);
                        }
                    }
                }
                else
                {
                    XmlElement newElement = CreateElement(tag);
                    newElement.SetAttribute("name", AndroidXmlNamespace, name);
                    for (int i = 0; i < attributes.Length; i++)
                        newElement.SetAttribute(attributes[i].name, AndroidXmlNamespace, attributes[i].value);
                    xmlParentElement.AppendChild(newElement);
                }
            }

            internal void AddMetaData()
            {
                var androidOpenXRSettings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Android);
                AddPassthroughMetaData(androidOpenXRSettings);
                AddAnchorMetaData(androidOpenXRSettings);
                AddSceneMetaData(androidOpenXRSettings);
            }

            void AddPassthroughMetaData(OpenXRSettings androidOpenXRSettings)
            {
                var arCameraFeature = androidOpenXRSettings.GetFeature<ARCameraFeature>();

                if (arCameraFeature == null || !arCameraFeature.enabled)
                    return;

                UpdateOrCreateAttribute(ManifestElement,
                    "uses-feature",
                    "com.oculus.feature.PASSTHROUGH",
                    ("required", "true")
                );
            }

            void AddAnchorMetaData(OpenXRSettings androidOpenXRSettings)
            {
                var arAnchorFeature = androidOpenXRSettings.GetFeature<ARAnchorFeature>();

                if (arAnchorFeature == null || !arAnchorFeature.enabled)
                    return;

                UpdateOrCreateAttribute(ManifestElement,
                    "uses-permission",
                    "com.oculus.permission.USE_ANCHOR_API"
                );
            }

            void AddSceneMetaData(OpenXRSettings androidOpenXRSettings)
            {
                var arPlaneFeature = androidOpenXRSettings.GetFeature<ARPlaneFeature>();

                if (arPlaneFeature == null || !arPlaneFeature.enabled)
                    return;

                UpdateOrCreateAttribute(ManifestElement,
                    "uses-permission",
                    "com.oculus.permission.USE_SCENE"
                );

            }
        }
    }
}
