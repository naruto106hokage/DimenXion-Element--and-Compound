#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.


/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using InfinityEngine;
using InfinityEngine.Localization;
using InfinityEngine.Extensions;
using InfinityEngine.Utils;
using InfinityEngine.Interpolations;

namespace InfinityEditor
{


    /// <summary>
    ///   Custom editor window for <see cref="ISILocalization"/> Component
    /// </summary>
    [Serializable]
    public class ISILocalizationEditor : EditorWindow
    {

        #region Fields

        private const float MENU_HEIGHT = 20;
        private const float FOOTER_HEIGHT = 110;
        private const int MAX_SHOWING_KEY = 20;
        private const string StringGetterPropertyFormat = "public static ISIString {0} = new ISIString(\"{0}\");";
        private const string AudioClipGetterPropertyFormat = "public static {0} {1} {{ get {{ return ISILocalization.GetAudio(\"{1}\");}} }}";
        private const string SpriteGetterPropertyFormat = "public static {0} {1} {{ get {{ return ISILocalization.GetSprite(\"{1}\");}} }}";


        private static ISILocalizationEditor Instance;
        private static int NumberOfAvailableLanguage = Enum.GetNames(typeof(Language)).Length;
        private static string[] LanguageEnumNames = Enum.GetNames(typeof(Language));
        private static string[] toolbarItems = { "Strings", "Audio", "Sprites" };
        private static int selectedTab;

        private static Dictionary<string, List<CodeGenerator>> CodeGenerators;
        private static string[] ClassUsings = new string[] { "UnityEngine", "InfinityEngine.Localization" };


        private ISILocalization script;
        private SerializedObject serializedObject;
        private SerializedProperty currentKey;

        private ISILocalization selection;

        private ReorderableList stringReordableList;
        private ReorderableList audioReordableList;
        private ReorderableList spriteReordableList;

        private SplitPane splitPane;
        private Vector2 leftComponentScroll;
        private Vector2 rightSplitScroll;

        private Dictionary<Language, SimpleAccordion> accordions = new Dictionary<Language, SimpleAccordion>();

        private SerializedProperty[] showingIndices; 
        private List<string> showingKeyList;
      

        private SerializedProperty selectedTabShowingIndices;
        private SerializedProperty selectedTabMinIndice;
        private SerializedProperty selectedTabMaxIndice;

        private SimpleAccordion accordion;
        private string errorMessage;
        private int indiceMin;
        private int indiceMax;
        private Rect toolbarRect;
        private Language languageToAdd;
        private LocalizedLanguage localizedLanguage;
        #endregion Fields

        #region Methods

        #region Unity

        private void OnEnable()
        {
            EditorApplication.update += Repaint;
        }

        private void OnDisable()
        {
            EditorApplication.update -= Repaint;
        }

        private void OnGUI()
        {
       
            EditorGUILayout.BeginVertical();

            if (Selection.activeGameObject != null)
            {
                selection = Selection.activeGameObject.GetComponent<ISILocalization>();
            }

            if (selection != null)
            {
                if (script == null)
                {
                    script = Selection.activeGameObject.GetComponent<ISILocalization>();
                }
                if (serializedObject == null)
                {
                    serializedObject = new SerializedObject(script);
                }

                Initialize();
            }

            if (serializedObject != null && script != null)
            {
                serializedObject.Update();
                Draw();
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.EndVertical();
        }

        #endregion Unity

        public static void OpenWindow()
        {
            Instance = EditorWindow.GetWindow<ISILocalizationEditor>("ISI Localization", true);
            //Instance.position.size = new Vector2(920, 500);
            //Instance.minSize = new Vector2(920, 500);
            //Instance.maxSize = Instance.minSize;
        }

        private void Initialize()
        {
            stringReordableList = InitializeReordableList(stringReordableList, "m_stringKeys");
            audioReordableList  = InitializeReordableList(audioReordableList, "m_audiosKeys");
            spriteReordableList =  InitializeReordableList(spriteReordableList, "m_spriteKeys");
            showingIndices = new SerializedProperty[] {
                serializedObject.FindProperty("m_stringKeysRange"),
                serializedObject.FindProperty("m_spriteKeysRange"),
                serializedObject.FindProperty("m_spriteKeysRange"),
            };
        }

        private ReorderableList InitializeReordableList(ReorderableList list, string propertyName)
        {
            if (list == null)
            {
                list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName));
            }

            AddCallbackToReordableList(list);
            return list;
        }

        private void AddCallbackToReordableList(ReorderableList reorderableList)
        {
            reorderableList.drawHeaderCallback = (rect) => 
            {
                EditorGUI.LabelField(rect, string.Concat(Strings.ISILocalizationDictionary, "(", reorderableList.count, ")"));

            };
            
            reorderableList.elementHeightCallback = (index) =>
            {
                if(IndiceIsInShowingRange(index))
                {
                    return EditorGUI.GetPropertyHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index));
                }
                return 0;
            };

            reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (IndiceIsInShowingRange(index))
                {
                    currentKey = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    errorMessage = CodeGenerator.CheckFieldName(currentKey.stringValue);
                    EditorGUI.PropertyField(rect, currentKey, GUIContent.none);
                    Utils.ShowMessage(EditorLocalization.GetLocalizedString(errorMessage), MessageType.Error, 
                        !string.IsNullOrEmpty(errorMessage) && errorMessage != CodeGenerator.EMPTY_FIELD_NAME_ERROR);
                }
            };

        }

        /// <summary>
        /// Initializes the split pane of the window if it is sets not null
        /// </summary>
        private void CreateSplitPaneIfNot()
        {
            if (splitPane == null)
            {
                splitPane = SplitPane.Create(SplitPaneOrientations.Vertical, 5);
                splitPane.DividerLocation = 0.4f;
                splitPane.DividendLimit = .9f;
            }
        }

        /// <summary>
        /// Draw the window
        /// </summary>
        private void Draw()
        {
            if(Event.current.type == EventType.MouseDown)
            {
                GUIUtility.keyboardControl = 0;
            }
            CreateSplitPaneIfNot();

            splitPane.Location = new Rect(0, 0, position.width, position.height);

            splitPane.onDrawLeftComponent  = DrawLeftComponent;
            splitPane.onDrawRightComponent = DrawRightComponent;

            toolbarRect = GUILayoutUtility.GetRect(0, MENU_HEIGHT, GUILayout.Width(250));
            selectedTab = GUI.Toolbar(toolbarRect, selectedTab, toolbarItems, EditorStyles.toolbarButton);

            ControlShowingIndices();

            splitPane.Draw();

        }
   
        /// <summary>
        /// Draws the left component of the split pane
        /// </summary>
        private void DrawLeftComponent()
        {
            var keyAreaHeight = position.height - MENU_HEIGHT - FOOTER_HEIGHT;
            var drawingArea = new Rect(0, MENU_HEIGHT, splitPane.LeftRect.width, keyAreaHeight);
            
            GUILayout.BeginArea(drawingArea, EditorStyles.helpBox);
            leftComponentScroll = GUILayout.BeginScrollView(leftComponentScroll);
            DrawKeysOfSelectedTab();
            GUILayout.EndScrollView();

            GUILayout.EndArea();

            drawingArea.y = keyAreaHeight + 30;
            drawingArea.height = (position.height - keyAreaHeight)+30;

            GUILayout.BeginArea(drawingArea);
            if (showingKeyList.Count >= MAX_SHOWING_KEY)
            {
                EditorGUILayout.PropertyField(selectedTabShowingIndices, GUIContent.none);
            }
            else
            {
                selectedTabMinIndice.floatValue = 0;
                selectedTabMaxIndice.floatValue = showingKeyList.Count;
            }

            DrawLeftComponentButtons();

            GUILayout.EndArea();

        }
    
        /// <summary>
        /// Draws the buttons at the bottom of the left side of the window
        /// </summary>
        private void DrawLeftComponentButtons()
        {
            var style = EditorStyles.miniButton;
            style.font = AssetReferences.FontAwesomeFont;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 18;
            style.normal.textColor = Color.gray;

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(FA.save, style))
            {
                ShowNotification(new GUIContent(Save()));
            }
            if (GUILayout.Button(FA.refresh, style))
            {
                ShowNotification(new GUIContent(Load()));
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(FA.remove, style))
            {
                showingKeyList.Clear();
            }

            if (GUILayout.Button(FA.sort_alpha_asc, style))
            {
                showingKeyList.Sort();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

        }

        /// <summary>
        /// Display the keys of the localized string, sprite or audio depending to the selected tab
        /// </summary>
        private void DrawKeysOfSelectedTab()
        {
            switch (selectedTab)
            {
                case 0:
                    stringReordableList.DoLayoutList();
                    showingKeyList = script.StringKeys;
                    break;
                case 1:
                    audioReordableList.DoLayoutList();
                    showingKeyList = script.AudioKeys;
                    break;
                case 2:
                    spriteReordableList.DoLayoutList();
                    showingKeyList = script.SpriteKeys;
                    break;
            }

        }

        /// <summary>
        /// Draws the right component of the split pane
        /// </summary>
        private void DrawRightComponent()
        {

            string key;
            switch (selectedTab)
            {
                case 0:
                    DrawLocalizedContents(DrawLocalizedStrings, true);
                    break;
                case 1:
                    DrawLocalizedContents(DrawLocalizedAudioClips, false);
                    break;
                case 2:
                    DrawLocalizedContents(DrawLocalizedSprites, false);
                    break;
            }
        }

        /// <summary>
        /// Draws the localized string keys of the given LocalizedLanguage object.
        /// </summary>
        /// <param name="model">The LocalizedLanguage object</param>
        private void DrawLocalizedStrings(LocalizedLanguage model)
        {
            var numberOfKeys = script.StringKeys.Count;
            var firstIndice = Mathf.Max(0, indiceMin - 1);
            var lastIndice = Mathf.Min(indiceMax, numberOfKeys);
            var key = string.Empty;
            for (var indice = firstIndice; indice < lastIndice; indice++)
            {
                try
                {
                    key = script.StringKeys[indice];
                    EditorGUILayout.BeginHorizontal();
                    if (!string.IsNullOrEmpty(key))
                    {
                        EditorGUILayout.LabelField(key, GUILayout.MaxWidth(150));
                        model.SetString(key, EditorGUILayout.TextArea(model.GetString(key), GUI.skin.textArea));
						if(model.Language != script.LocalizedLanguages[0].Language){
							if (GUILayout.Button(AssetReferences.GoogleTranslateIcon, EditorStyles.label, GUILayout.Width(24), GUILayout.Height(24)))
							{
								AutoTranslateKeyOfLanguage(key, model);
							}
						}
                    }
                    EditorGUILayout.EndHorizontal();
                }
                catch { }
            }
        }

        /// <summary>
        /// Draws the localized sprites keys of the given LocalizedLanguage object.
        /// </summary>
        /// <param name="model">The LocalizedLanguage object</param>
        private void DrawLocalizedSprites(LocalizedLanguage model)
        {
            var numberOfKeys = script.SpriteKeys.Count;
            var firstIndice = Mathf.Max(0, indiceMin - 1);
            var lastIndice = Mathf.Min(indiceMax, numberOfKeys);
            var key = string.Empty;
            for (var indice = firstIndice; indice < lastIndice; indice++)
            {
                try
                {
                    key = script.SpriteKeys[indice];
                    EditorGUILayout.BeginHorizontal();
                    if (!string.IsNullOrEmpty(key))
                    {
                        EditorGUILayout.LabelField(key, GUILayout.MaxWidth(150));
                        model.SetSprite(key, (Sprite)EditorGUILayout.ObjectField(model.GetSprite(key), typeof(Sprite), false));
                    }
                    EditorGUILayout.EndHorizontal();
                }
                catch { }
            }
        }

        /// <summary>
        /// Draws the localized audio clip keys of the given LocalizedLanguage object.
        /// </summary>
        /// <param name="model">The LocalizedLanguage object</param>
        private void DrawLocalizedAudioClips(LocalizedLanguage model)
        {
            var numberOfKeys = script.AudioKeys.Count;
            var firstIndice = Mathf.Max(0, indiceMin - 1);
            var lastIndice = Mathf.Min(indiceMax, numberOfKeys);
            var key = string.Empty;
            for (var indice = firstIndice; indice < lastIndice; indice++)
            {
                try
                {
                    key = script.AudioKeys[indice];
                    EditorGUILayout.BeginHorizontal();
                    if (!string.IsNullOrEmpty(key))
                    {
                        EditorGUILayout.LabelField(key, GUILayout.MaxWidth(150));
                        model.SetAudio(key, (AudioClip)EditorGUILayout.ObjectField(model.GetAudio(key), typeof(AudioClip), false));
                    }
                    EditorGUILayout.EndHorizontal();
                }
                catch { }
            }
        }

        /// <summary>
        /// Draws the localized contents of all created localized languages.
        /// </summary>
        /// <param name="drawElementsCallback">The function whichs will draw the localized contents when the draw area is expanded</param>
        /// <param name="drawRemoveButton">Is sets to <c>true</c> the function draw a remove button whichs allow to delete the localized language</param>
        private void DrawLocalizedContents(Action<LocalizedLanguage> drawElementsCallback, bool drawRemoveButton = false)
        {
            Rect bounds = new Rect(10, 0, splitPane.RightRect.width - 20, position.height-0);
            GUI.Box(bounds, GUIContent.none, EditorStyles.helpBox);
            GUILayout.BeginArea(bounds);
            rightSplitScroll = GUILayout.BeginScrollView(rightSplitScroll);

            if (script.LanguageCount < NumberOfAvailableLanguage)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(AssetReferences.PlusIcon, EditorStyles.label, GUILayout.Width(32), GUILayout.Height(32)))
                {
                    var l = new LocalizedLanguage();
                    l.Language = languageToAdd;
                    script.LocalizedLanguages.Add(l);
                }
                languageToAdd = (Language)EditorGUILayout.EnumPopup(languageToAdd, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
 
            GUILayout.Space(10);

            var count = script.LocalizedLanguages.Count;

            for (int i = 0; i < count; i++)
            {
                localizedLanguage = script.LocalizedLanguages[i];

                if(!accordions.TryGetValue(localizedLanguage.Language, out accordion))
                {
                    accordion = new SimpleAccordion();
                    accordion.IsExpanded = localizedLanguage.isExpanded;
                    accordion.expandStateChangeCallback = (acc) => localizedLanguage.isExpanded = acc.IsExpanded;
                    accordions.Add(localizedLanguage.Language, accordion);

                }

                accordion.drawHeaderCallback = () => { return DrawLocalizedLanguageHeader(localizedLanguage, i, count, drawRemoveButton); };


                if (script.LocalizedLanguages.Count(item => item.Language == localizedLanguage.Language) > 1)
                {
                    EditorGUILayout.HelpBox(Strings.ISILocalizationDuplicatedLanguage, MessageType.Error);
                }

                accordion.drawCallback = () =>
                {
                    GUILayout.Space(15);
                    drawElementsCallback.Invoke(localizedLanguage);
                };

                accordion.OnGUI(EditorStyles.helpBox);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private Rect DrawLocalizedLanguageHeader(LocalizedLanguage localizedLanguage, int index, int languagesCount, bool drawRemoveButton = false)
        {
            var icon = accordion.IsExpanded ? FA.angle_double_down : FA.angle_double_right;
  
            var rect  = EditorGUILayout.BeginVertical();

            var title = new GUIContent(localizedLanguage.LanguageName);

            GUILayout.Box(title, AssetReferences.AccordionHeader, GUILayout.ExpandWidth(true), GUILayout.Height(20));

            DrawerHelper.FAIcon(rect, icon, FAOption.TextAnchor(TextAnchor.UpperLeft), FAOption.FontSize(20), FAOption.Padding(new RectOffset(5, 0, 0, 0)));

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(100);
            // draw translation progress bar
            if (localizedLanguage.isBeingAutoTranslated)
            {
                var r = GUILayoutUtility.GetRect(250, 32);
                EditorGUI.ProgressBar(r, localizedLanguage.translationProgress, string.Empty);
            }

            GUILayout.FlexibleSpace();
        
            // draw google translate button
            if (index > 0 && selectedTab == 0)
            {
                if (GUI.Button(new Rect(rect.width - 82, rect.y, 24, rect.height), AssetReferences.GoogleTranslateIcon, EditorStyles.label))
                {
                    AutoTranslateAllKeyOfLanguage(localizedLanguage);
                }

            }

            GUILayout.Space(10);

            // draw up arrow
            if (index > 0 && DrawerHelper.FAButton(new Rect(rect.width - 58, rect.y, 24, rect.height), FA.arrow_up))
            {
                SwapLanguagesPosition(index, index - 1);
            }

            GUILayout.Space(10);

            // draw down arrao
            if (index < languagesCount - 1 && DrawerHelper.FAButton(new Rect(rect.width - 34, rect.y, 24, rect.height), FA.arrow_down))
            {

                SwapLanguagesPosition(index, index + 1);
            }

            GUILayout.Space(10);

            // draw remove button
            if (drawRemoveButton && DrawerHelper.FAButton(new Rect(rect.width - 10, rect.y, 24, rect.height), FA.trash, FAOption.TextColor(Color.red)))
            {
                script.LocalizedLanguages.Remove(localizedLanguage);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
    
            return rect;
        }

        private void SwapLanguagesPosition(int srcIndex, int dstIndex)
        {
            var tmp = script.LocalizedLanguages[srcIndex];
            script.LocalizedLanguages[srcIndex] = script.LocalizedLanguages[dstIndex];
            script.LocalizedLanguages[dstIndex] = tmp;
        }

        private void AutoTranslateKeyOfLanguage(string key, LocalizedLanguage model)
        {
            var sourceLang = script.LocalizedLanguages[0];
            var sourceLangCode = sourceLang.Language.Code();

            var sourceText = sourceLang.GetString(key);
            var targetLangCode = model.Language.Code();
            if (!string.IsNullOrEmpty(sourceText))
            {
                GoogleTranslateEditor.Translate(sourceLangCode, targetLangCode, sourceText, (info) => {
                    if (info.IsValid)
                    {
                        model.SetString(key, info.Text);
                    }
                    else
                    {
                        Debug.LogError("Failed to translate the key " + key);
                    }
                });
            }
        }

        private void AutoTranslateAllKeyOfLanguage(LocalizedLanguage model)
        {
            var sourceModel = script.LocalizedLanguages[0];
            var sourceLangCode = sourceModel.Language.Code();

            var targetLangCode = model.Language.Code();
            var totalSteps = sourceModel.LocalizedStrings.Count;
            model.isBeingAutoTranslated = true;

            foreach (var keyValue in sourceModel.LocalizedStrings)
            {
                if (!string.IsNullOrEmpty(keyValue.Value))
                {
                    GoogleTranslateEditor.Translate(sourceLangCode, targetLangCode, keyValue.Value, (info) => {
                        if (info.IsValid)
                        {
                            model.SetString(keyValue.Key, info.Text);
                        }
                        else
                        {
                            Debug.LogError(string.Concat("Failed to translate the key ", keyValue.Key, " for the language ", info.TargetLanguage));
                        }
                        model.UpdateTranslationProgress(totalSteps);
                    });
                }
                else
                {
                    model.UpdateTranslationProgress(totalSteps);
                }
            }
           
        }

        private void ControlShowingIndices()
        {
            selectedTabShowingIndices = showingIndices[selectedTab];

            selectedTabMinIndice = selectedTabShowingIndices.FindPropertyRelative("min");
            selectedTabMaxIndice = selectedTabShowingIndices.FindPropertyRelative("max");

            indiceMin = (int)selectedTabMinIndice.floatValue;
            indiceMax = (int)selectedTabMaxIndice.floatValue;
           
        }

        private bool IndiceIsInShowingRange(int indice)
        {
            return (indice >= indiceMin - 1 && indice < indiceMax);
        }

        #region Save & Load

        private void RemoveUnusedKeys()
        {
            for (int i = 0; i < script.LocalizedLanguages.Count; i++)
            {
                script.Validate();
            }
        }
        
        private bool HasErrorInKeys(List<string> keys)
        {
            bool res = false;
            string error = null;
            for (int i = 0; i < keys.Count; i++)
            {
                if (script.StringKeys.Count(keys[i]) > 1)
                {
                    Debugger.LogError(string.Format("Key error at {0} => {1}", i, Strings.Format(Strings.__ISILocalizationDuplicatedKey, keys[i])));
                    res = true;
                    continue;
                }
                error = CodeGenerator.CheckFieldName(keys[i]);
                if (error != null)
                {
                    Debugger.LogError(string.Format("Key error at {0} => {1}", i, EditorLocalization.GetLocalizedString(error)));
                    res = true;
                }
            }
            return res;
        }

        public string Save()
        {
            if (HasErrorInKeys(script.StringKeys))
                return Strings.ISILocalizationCheckError;

            RemoveUnusedKeys();

            XmlDocument doc;
            XmlNode docNode;
            XmlNode resourceNode;
            XmlNode stringNode;
            XmlAttribute nameAttribute;

            string path = "";
            string directoryPath = "";

            foreach (LocalizedLanguage model in script.LocalizedLanguages)
            {
                doc = new XmlDocument();
                docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                resourceNode = doc.CreateElement("resources");

                directoryPath = string.Format("Assets/{0}", string.Format(ISILocalization.LanguagePaths, model.Language)).Replace("/strings.xml", "");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                path = Path.Combine(Application.dataPath, string.Format(ISILocalization.LanguagePaths, model.Language));
                model.LocalizedStrings.For(keyValue =>
                {
                    stringNode = doc.CreateElement("strings");
                    nameAttribute = doc.CreateAttribute("name");
                    nameAttribute.Value = keyValue.Key;
                    stringNode.Attributes.Append(nameAttribute);
                    stringNode.AppendChild(doc.CreateTextNode(keyValue.Value));
                    resourceNode.AppendChild(stringNode);

                });

                doc.AppendChild(resourceNode);

                using (var stream = new StreamWriter(new FileStream(path, FileMode.Create), Encoding.UTF8))
                {
                    doc.Save(stream);
                    stream.Close();
                }
            }

            GenerateClass();
            var type = PrefabUtility.GetPrefabType(script.gameObject);
            if (type == PrefabType.None || type == PrefabType.PrefabInstance)
            {
                PrefabUtility.CreatePrefab(ISILocalization.PrefabPath, script.gameObject);
            }
            else
            {
                return "Impossible to export data from the prefab, you must place an instance in the scene and export the data of the instance.";
            }

            AssetDatabase.Refresh();
            return Strings.ISILocalizationUpdated;
        }

        public string Load()
        {
            var type = PrefabUtility.GetPrefabType(script.gameObject);
            if (type != PrefabType.None && type != PrefabType.PrefabInstance)
            {
                return "Impossible to import data from the prefab, you must place an instance in the scene and import the data of the instance.";
            }

            XmlDocument doc = null;
            XmlNode resourceNode = null;
            string path = "";
            string key = "";
            string value = "";
            string errorMessage = "";

            bool hasError = false;
            bool isFirstFile = true;

            List<string> keyList = new List<string>();
            List<LocalizedLanguage> existingLanguages = new List<LocalizedLanguage>();

            List<StringKV> keyValues;
            LocalizedLanguage currentLoadedLanguage;

            foreach (var language in LanguageEnumNames)
            {
                path = Path.Combine(Application.dataPath, string.Format(ISILocalization.LanguagePaths, language));

                if (File.Exists(path))
                {
                    keyValues = new List<StringKV>();
                    doc = new XmlDocument();
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        doc.Load(stream);
                        stream.Close();
                    }
                    // get all xml nodes child of the node 'resources'
                    resourceNode = doc["resources"];
                    var index = 1;

                    // remove xml comments
                    var children = resourceNode.ChildNodes.Cast<XmlNode>().Where(n => n.NodeType != XmlNodeType.Comment);

                    foreach (XmlNode node in children)
                    {
                        key = node.Attributes[0].Value;
                        value = node.FirstChild == null ? "" : node.FirstChild.Value;

                        keyValues.Add(new StringKV(key, value));

                        if (isFirstFile)
                            keyList.Add(key);

                        errorMessage = CodeGenerator.CheckFieldName(key);
                        if (errorMessage != null)
                        {
                            hasError = true;
                            Debugger.LogError(string.Format("{0} => {1} ", language, EditorLocalization.GetLocalizedString(errorMessage)));
                        }
                        index++;
                    }
                    isFirstFile = false;
                    currentLoadedLanguage = new LocalizedLanguage(keyValues);
                    currentLoadedLanguage.Language = (Language)Enum.Parse(typeof(Language), language);
                    existingLanguages.Add(currentLoadedLanguage);
                }
            }

            if (keyList.Count > 0)
            {
                if (!hasError)
                {
                    script.StringKeys.Clear();

                    foreach (var k in keyList)
                    {
                        script.StringKeys.Add(k);
                    }
                    foreach (var language in existingLanguages)
                    {
                        script.ReplaceLocalizedLanguage(language);
                    }

                    GenerateClass();

                    PrefabUtility.CreatePrefab(ISILocalization.PrefabPath, script.gameObject);
                    AssetDatabase.Refresh();

                    return Strings.ISILocalizationUpdated;
                }
                else
                {
                    return Strings.ISILocalizationCheckError;
                }
            }
            else
            {
                return Strings.ISILocalizationMissingLanguage;
            }
        }

        private void GenerateClass()
        {
            CodeGenerators = new Dictionary<string, List<CodeGenerator>>();

            CodeGenerator field;
            string names = "";

            
            script.Keys.ForEach((key, array) =>
            {

            CodeGenerators.Add(key, new List<CodeGenerator>());
            names = string.Join(",", array.ToArray());
            field = CodeGenerator.New("Names")
                                 .Body("public const string Names = \"{0}\";", names)
                                 .Summary("All {0} keys separated by ','. You can created an array by using string.split() method", key);

                CodeGenerators[key].Add(field);

                foreach (string k in array)
                {
                    field = CodeGenerator.New(k);
                    switch (key)
                    {
                        case "strings":
                            field.Body(StringGetterPropertyFormat, k).Summary("Value of the string with the key {0}", k);
                            break;
                        case "audios":
                            field.Body(AudioClipGetterPropertyFormat, "AudioClip", k).Summary("get property of the AudioClip with the key {0}", k);
                            break;
                        case "sprites":
                            field.Body(SpriteGetterPropertyFormat, "Sprite", k).Summary("get property of the Sprite with the key {0}", k);
                            break;
                    }
                    CodeGenerators[key].Add(field);
                }
            });

            Utils.CheckGenFolder();

            var generator = new ClassGenerator("public static class R3");
            generator.Usings = ClassUsings;
            generator.Summary = Strings.ClassCommentary;
            generator.Usings = ClassUsings;

            generator.Namespace = "InfinityEngine.Localization";

            ClassGenerator subClass;
            generator.Generate(Utils.GenScriptFolder + "R3.cs", (writer) =>
            {
                CodeGenerators.ForEach((key, value) =>
                {
                    subClass = new ClassGenerator("public static class {0}", key);

                    ClassGenerator.WriteSubClass(subClass, writer, () =>
                    {
                        ClassGenerator.WriteFields(value, writer);
                    });
                });
            });

            AssetDatabase.Refresh();
        }

        #endregion Save & Load

        #region Menu Items

        [MenuItem("Tools/Infinity Engine/Localization/ISI Localization/Find Dependencies In Scene")]
        private static void FindDependencies()
        {
            Utils.FindAllComponentsOfType(typeof(ISILocalization), typeof(LocalizedAudio), typeof(LocalizedSprite), typeof(LocalizedString), typeof(Flag));
        }

        [MenuItem("Tools/Infinity Engine/Localization/ISI Localization/Default Language")]
        private static void DebugLanguage()
        {
            var frame = Frame.Create(new Vector2(400, 150), new GUIContent("ISI Localization", Resources.Load<Texture2D>("InfinityInteractive")));
            var language = (Language)Enum.Parse(typeof(Language), PlayerPrefs.GetString(ISILocalization.DefaultLanguagePreferenceKey, Language.English.ToString()));
            frame.onGUI = () =>
            {
                GUILayout.Space(10);
                language = (Language)EditorGUILayout.EnumPopup(language);
                GUILayout.Space(20);

                if (GUILayout.Button(Strings.Confirm))
                {
                    PlayerPrefs.SetString(ISILocalization.DefaultLanguagePreferenceKey, language.ToString());
                    PlayerPrefs.Save();
                }
            };
            frame.Show();
        }

        #endregion Menu Items

        #endregion Methods

    }
}