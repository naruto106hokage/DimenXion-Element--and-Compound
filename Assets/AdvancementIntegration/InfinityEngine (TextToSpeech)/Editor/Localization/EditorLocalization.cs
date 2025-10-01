/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;
using InfinityEngine.Extensions;
using InfinityEngine.Utils;

namespace InfinityEditor
{
  
    public static class EditorLocalization
    {
        private static int selectedLanguage;
        private static Dictionary<string, string> localizedStrings;
        private static string[] languages;
        private static string[] Languages
        {
            get
            {
                var path = Path.Combine(Application.dataPath, "InfinityEngine/Editor/Localization/");
                if (languages == null)
                {
                    languages = Directory.GetDirectories(path).Select(elem => elem.Replace(path, string.Empty));

                }
                return languages;
            }

        }
        
        private static XmlDocument GetXMLDocument()
        {
            var path = Path.Combine(Application.dataPath, string.Format("InfinityEngine/Editor/Localization/{0}/strings.xml", PlayerPrefs.GetString("___EDITOR_LANGUAGE_PREF___", "en")));
            var doc = new XmlDocument();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                doc.Load(stream);
                stream.Close();
            }
            return doc;
        }

        private static void CheckResources()
        {
            if (localizedStrings == null)
            {
                localizedStrings = new Dictionary<string, string>();
                var doc = GetXMLDocument();
                var resources = doc["resources"];
                var key = string.Empty;
                var value = string.Empty;
                var children = resources.ChildNodes.Cast<XmlNode>().Where(n => n.NodeType != XmlNodeType.Comment);
                foreach (XmlNode node in children)
                {
                    key = node.Attributes[0].Value;
                    value = node.FirstChild.Value;
                    if (!localizedStrings.ContainsKey(key))
                    {
                        localizedStrings.Add(key, value);
                    }
                    else
                    {
                        Debugger.LogError("Editor Localization : The key {0} is already used", key);
                    }
                }
            }
        }

        private static void ChangeLanguage()
        {
            languages = null;
            PlayerPrefs.SetInt("___EDITOR_SELECTED_LANGUAGE___", selectedLanguage);
            PlayerPrefs.SetString("___EDITOR_LANGUAGE_PREF___", Languages[selectedLanguage]);
            localizedStrings = null;
            CheckResources();
        }

        public static string GetLocalizedString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            CheckResources();

            string value;
            if (localizedStrings.TryGetValue(key, out value))
            {
                return value.Trim();
            }

            Debug.LogError("There is no localized string with the key " +  key);
            return null;
        }

        public static string GetLocalizedString(string key, params object[] args)
        {
            CheckResources();
            string value;

            if (localizedStrings.TryGetValue(key, out value))
            {
                return string.Format(value, args).Trim();
            }

            Debugger.LogError("There is no key with the name " + key);
            return null;
        }

        public static void OpenEditor()
        {
            Frame frame = Frame.Create(new Vector2(200, 40), new GUIContent(Strings.LANGUAGE));
            selectedLanguage = PlayerPrefs.GetInt("___EDITOR_SELECTED_LANGUAGE___", 0);
            frame.onGUI = () =>
            {
                GUILayout.Space(10);
                selectedLanguage = EditorGUILayout.Popup(selectedLanguage, Languages);
                GUILayout.Space(20);
                if (GUILayout.Button(Strings.Confirm))
                {
                    ChangeLanguage();
                    PlayerPrefs.SetInt("___EDITOR_SELECTED_LANGUAGE___", selectedLanguage);
                }
            };
            frame.Show();
        }
    }

}