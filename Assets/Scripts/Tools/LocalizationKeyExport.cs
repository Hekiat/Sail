using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace sail.tools
{
    public class LocalizationKeyExport
    {
        public static string ExportNamespace = projectName().ToLower() + ".localization";

        [MenuItem("Sail/Localization/Export Keys")]
        public static void exportLocalizationKeys()
        {
            var path = Application.dataPath + Path.Combine("/Scripts/Generated/Localization/LocalizationKeys.cs");
            

            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
            {
                System.IO.Directory.CreateDirectory(fi.DirectoryName);
            }

            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.WriteLine("using UnityEngine.Localization;");
                outputFile.WriteLine("");

                outputFile.WriteLine($"namespace {ExportNamespace}");
                outputFile.WriteLine("{");

                var tables = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollections();
                foreach (var table in tables)
                {
                    outputFile.WriteLine($"\tpublic static class {table.name}");
                    outputFile.WriteLine("\t{");

                    foreach (var entry in table.SharedData.Entries)
                    {
                        outputFile.WriteLine($"\t\tpublic static LocalizedString {entry.Key} = new LocalizedString() {{ TableReference = \"{table.name}\", TableEntryReference = \"{entry.Key}\"}};");
                        //outputFile.WriteLine("\t\t" + entry.Key + ",");
                    }

                    outputFile.WriteLine("\t}");
                }
                outputFile.WriteLine("}");

                Debug.Log($"Exported: {path}");
            }
        }


        private static string projectName()
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            return projectName;
        }
    }
}
