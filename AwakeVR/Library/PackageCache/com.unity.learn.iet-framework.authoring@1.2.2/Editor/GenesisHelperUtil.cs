using System.Linq;
using UnityEditor;
using UnityEngine;
using Unity.Tutorials.Core.Editor;

namespace Unity.Tutorials.Authoring.Editor
{
    using static Localization;

    class GenesisHelperUtils
    {
        [MenuItem(MenuItems.AuthoringMenuPath + "Debug/Progress Tracking/Print All Statuses")]
        static void PrintAllStatuses()
        {
            GenesisHelper.PrintAllTutorials();
        }

        [MenuItem(MenuItems.AuthoringMenuPath + "Debug/Progress Tracking/Clear All Statuses")]
        static void ClearAllStatuses()
        {
            if (EditorUtility.DisplayDialog(
                string.Empty,
                Tr("Do you want to clear progress of every tutorial?"),
                Tr("Clear"),
                Tr("Cancel")))
            {
                GenesisHelper.GetAllTutorials((r) =>
                {
                    var ids = r.Select(a => a.lessonId);
                    foreach (var id in ids)
                    {
                        GenesisHelper.LogTutorialStatusUpdate(id, " ");
                    }
                    Debug.Log("Lesson statuses cleared");
                    // Refresh the window, if it's open.
                    var wnd = TutorialWindow.Instance;
                    if (wnd)
                    {
                        wnd.MarkAllTutorialsUncompleted();
                    }
                });
            }
        }
    }
}
