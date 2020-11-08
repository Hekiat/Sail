using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace sail
{
    public class CSVLoader
    {
        private TextAsset csvFile;
        private const char lineSeparator = '\n';
        private const char surround = '"';
        private string[] fieldSeparator = { "\",\"" };

        public void loadCSV()
        {
            
        }

        public Dictionary<int, string> getDictionaryValues(string file)
        {
            var dictionary = new Dictionary<int, string>();

            var csvFile = Resources.Load<TextAsset>(file);

            using (TextReader reader = new StringReader(csvFile.text))
            {
                var csvReader = new sail.tool.CsvReader(reader, ",");
                int lc = 0;

                //string s = "";
                while (csvReader.Read())
                {
                    //s += "line " + lc + " : ";
                    //for (int i = 0; i < csvReader.FieldsCount; i++)
                    //{
                    //    string val = csvReader[i];
                    //    s += i + "[ " + val + " ]";
                    //}
                    //s += "\n";

                    if (csvReader.FieldsCount == 0)
                    {
                        continue;
                    }

                    if (csvReader.FieldsCount != 2)
                    {
                        Debug.LogWarning($"CSV Loader: fail to load line {lc} invalid number of field.");
                        continue;
                    }

                    var keyStr = csvReader[0];
                    var key = keyStr.GetHashCode();
                    var value = csvReader[1];
                    dictionary.Add(key, value);
                    //s += $"Key: {keyStr}, KeyHash: {key}, Value: {value}\n";

                    lc++;
                }
                //Debug.Log(s);
            }

            return dictionary;
        }
    }
}