using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Unity.Tutorials.Core.Editor;

namespace Unity.Tutorials.Authoring.Editor
{
    static class TutorialStructureExtractor
    {
        [MenuItem(MenuItems.AuthoringMenuPath + "Localization/Extract Tutorial Structure of the Project...")]
        static void Extract()
        {
            const string fileExt = "txt";
            var filepath = EditorUtility.SaveFilePanel(
                Localization.Tr("Extract Tutorial Structure as Text"),
                "",
                $"{Application.productName} Tutorial Structure {LocalizationDatabaseProxy.currentEditorLanguage}.{fileExt}",
                fileExt
            );
            if (filepath.IsNullOrEmpty())
                return;

            using (var sw = new StreamWriter(filepath, append: false, new System.Text.UTF8Encoding()))
            {
                int indentLevel = 0;
                foreach (var project in FindAssets<TutorialProjectSettings>())
                {
                    var pg = project.WelcomePage;
                    if (pg)
                    {
                        var titleLength = WriteField(sw, indentLevel, "Welcome Dialog");
                        sw.WriteLine(Underlining(indentLevel, titleLength));
                        ++indentLevel;
                        WriteField(sw, indentLevel, nameof(pg.WindowTitle), pg.WindowTitle);
                        WriteField(sw, indentLevel, nameof(pg.Title), pg.Title);
                        WriteField(sw, indentLevel, nameof(pg.Description), pg.Description);

                        WriteField(sw, indentLevel, "Buttons:");
                        ++indentLevel;
                        foreach (var btn in pg.Buttons)
                        {
                            WriteField(sw, indentLevel, btn.Text);
                        }
                        --indentLevel;

                        --indentLevel;
                        sw.WriteLine();
                    }
                }

                // NOTE Section doesn't expose its Tutorial publicly so must use lesson IDs for figuring out the order.

                var tutorialIds = FindAssets<TutorialContainer>()
                    .FirstOrDefault()
                    ?.Sections
                    .Where(s => s.IsTutorial)
                    .Select(s => s.TutorialId)
                    .ToList()
                    ?? new List<string>();

                foreach (var tutorial in FindAssets<Tutorial>())
                {
                    var tutorialNumber = tutorialIds.FindIndex(id => id == tutorial.LessonId) + 1;
                    var titleLength = WriteField(sw, indentLevel, $"Tutorial {tutorialNumber}/{tutorialIds.Count}", tutorial.TutorialTitle);
                    sw.WriteLine(Underlining(indentLevel, titleLength));

                    int pageNumber = 0;

                    foreach (var pg in tutorial.PagesCollection)
                    {
                        WriteField(sw, indentLevel, $"Page {++pageNumber}/{tutorial.PageCount}:");
                        ++indentLevel;
                        // first paragraph (index 0) is the mandatory media header
                        var narrative = pg.Paragraphs.ElementAtOrDefault(1);
                        // Instruction paragraph is optional
                        var instruction = pg.Paragraphs.ElementAtOrDefault(2);
                        // Tutorial switch paragraph is optional
                        var switchTutorial = pg.Paragraphs.ElementAtOrDefault(3);

                        WriteField(sw, indentLevel, "Narrative Title", narrative.Title);
                        WriteField(sw, indentLevel, "Narrative Text", narrative.Text);

                        if (instruction != null)
                        {
                            WriteField(sw, indentLevel, "Instruction Title", instruction.Title);
                            WriteField(sw, indentLevel, "Instruction Text", instruction.Text);
                        }

                        if (switchTutorial != null)
                            WriteField(sw, indentLevel, "Tutorial Button", switchTutorial.Text);

                        --indentLevel;
                        sw.WriteLine();
                    }
                }
            }
        }

        static string Indentation(int depth)
        {
            const char symbol = ' ';
            const int symbolsPerIndent = 2;
            return new string(symbol, depth * symbolsPerIndent);
        }

        static string Underlining(int depth, int numChars)
        {
            const char symbol = '-';
            return Indentation(depth) + new string(symbol, numChars);
        }

        // Writes a line with indentation.
        // returns the number of written characters, not counting line ending or indentation.
        static int WriteField(StreamWriter stream, int depth, string name, string value = null)
        {
            stream.Write(Indentation(depth));
            if (value != null)
                value = $": {value}";
            var line = $"{name}{value}";
            stream.WriteLine(line);
            return line.Length;
        }

        static IEnumerable<T> FindAssets<T>() where T : UnityEngine.Object =>
            AssetDatabase.FindAssets($"t:{typeof(T).FullName}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>);
    }
}
