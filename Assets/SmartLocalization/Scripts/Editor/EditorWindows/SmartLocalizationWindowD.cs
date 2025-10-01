//
//  SmartLocalizationWindow.cs
//
// Written by Niklas Borglund and Jakob Hillerström
//

namespace SmartLocalization.Editor
{

	using UnityEngine;
	using UnityEditor;
	using SmartLocalization;
	using System.Collections.Generic;
	using SmartLocalization.ReorderableList;
	using System.IO;
	using LitJson;
	using System.Xml;
	using System.Xml.Serialization;

	[System.Serializable]
	public class SmartLocalizationWindowD : EditorWindow
	{
		#region Members

		public static readonly string MicrosoftTranslatorIDSaveKey = "cws_mtClientID";
		public static readonly string MicrosoftTranslatorSecretSaveKey = "cws_mtClientSecret";
		public static readonly string KeepAuthenticatedSaveKey = "cws_mtKeepAuthenticated";

		public IAutomaticTranslator automaticTranslator = null;
		[SerializeField]
		public TranslateLanguageWindow translateLanguageWindow = null;

		[SerializeField]
		SmartCultureInfoCollection allCultures = null;

		[SerializeField]
		SmartCultureInfoCollection availableCultures = null;

		[SerializeField]
		SmartCultureInfoCollection nonAvailableCultures = null;

		[SerializeField]
		Vector2 scrollPosition = Vector2.zero;

		[SerializeField]
		Vector2 createScrollPosition = Vector2.zero;

		[SerializeField]
		bool isInitialized = false;

		[SerializeField]
		HOEditorUndoManager undoManager = null;

		//Windows
		[SerializeField]
		EditRootLanguageFileWindow editRootWindow = null;

		[SerializeField]
		string mtClientID = string.Empty;

		[SerializeField]
		string mtClientSecret = string.Empty;

		[SerializeField]
		bool keepTranslatorAuthenticated = false;

		SmartCultureInfoMenuControl languageListContextMenu;
		SmartCultureInfoListAdaptor languageListAdaptor;
		CreateLanguageMenuControl createListContextMenu;
		CreateLanguageListAdaptor createListAdaptor;
		SettingsMenuControl settingsContextMenu;
		SettingsListAdaptor settingsAdaptor;

		[SerializeField]
		List<string> settingsList = new List<string> ();
		SmartCultureInfo cultureInfo;
		public string jsonPath = "Assets//AdvancementIntegration//LanguageDescription//language.json";

		#endregion

		#region Properties

		public SmartCultureInfoCollection AvailableCultures {
			get {
				return availableCultures;
			}
		}

		#endregion


		void OnInitializeCollectionsCallback ()
		{
			InitializeCultureCollections ();
		}

		public void InitializeCultureCollections (bool reloadAllCultures = false)
		{
			if (reloadAllCultures) {
				allCultures = SmartCultureInfoEx.Deserialize (LocalizationWorkspace.CultureInfoCollectionFilePath ());
			}

			availableCultures = LanguageHandlerEditor.CheckAndSaveAvailableLanguages (allCultures);
			nonAvailableCultures = LanguageHandlerEditor.GetNonAvailableLanguages (allCultures);

			availableCultures.cultureInfos.Sort ((a, b) => {
				return a.englishName.CompareTo (b.englishName);
			});
			nonAvailableCultures.cultureInfos.Sort ((a, b) => {
				return a.englishName.CompareTo (b.englishName);
			});

			availableCultures.cultureInfos.Insert (0, new SmartCultureInfo (string.Empty, "ROOT", "ROOT", false));

			//languageListAdaptor = new SmartCultureInfoListAdaptor(availableCultures.cultureInfos,DrawAvailableLanguageItem, 28);
			languageListContextMenu = new SmartCultureInfoMenuControl ();

			createListAdaptor = new CreateLanguageListAdaptor (nonAvailableCultures.cultureInfos, DrawCreateLanguageItem, 15);
			createListContextMenu = new CreateLanguageMenuControl ();

			//settingsAdaptor = new SettingsListAdaptor(settingsList,DrawSettingsItem, 110);
			settingsContextMenu = new SettingsMenuControl ();
		}

		public SmartCultureInfo DrawCreateLanguageItem (Rect position, SmartCultureInfo info)
		{
			float fullWindowWidth = position.width + 30;
			Rect newPosition = position;
			newPosition.width = fullWindowWidth * 0.5f;
			GUI.Label (newPosition, info.englishName + " - " + info.languageCode);

			float buttonWidth = fullWindowWidth * 0.2f;
			newPosition.width = buttonWidth;
			newPosition.x = fullWindowWidth - newPosition.width;

			if (GUI.Button (newPosition, "Create")) {
				OnCreateLanguageClick (info);
				Debug.Log ("info: " + info);
			}
			newPosition.x -= buttonWidth;
			if (GUI.Button (newPosition, "Import")) {
				LanguageImportWindow.ShowWindow (info, OnInitializeCollectionsCallback);
			}
			return info;
		}

		#region Mono

		public class LanguageDetails
		{
			public string LANG_CULTURE_NAME;
//Shows Culture Code
			public string LANG_NAME;
//Native name of English
			public string LANG_NAME_EN;
// English Name
			public string DIR;
// LanguageTextAlignment
		}

		private XmlDocument doc;
		private XmlNodeList cultureInfoNodeList;

		void OnGUI ()
		{

			if (GUILayout.Button ("Add Languages")) {
 
				string jsn;
				using (StreamReader sr = new StreamReader (jsonPath)) {
					jsn = sr.ReadToEnd ();
				}
				JsonData itemData = JsonMapper.ToObject (jsn);
				LanguageDetails details = new LanguageDetails ();
				LanguageHandler.instance.Languages.Clear ();

				doc = new XmlDocument ();
				doc.Load ("Assets\\SmartLocalizationWorkspace\\Data\\SmartCultureInfoData\\SmartCultureInfo.xml");
				cultureInfoNodeList = doc.SelectNodes ("//CultureInfos/CultureInfo");
				;
				// Debug.Log("Testing"+xmlNodeList[4].FirstChild.InnerText);

				for (int i = 0; i < itemData ["lang"].Count; i++) {
					details.LANG_CULTURE_NAME = itemData ["lang"] [i] ["LANG_CULTURE_NAME"].ToString ();//Shows Culture Code
					details.LANG_NAME_EN = itemData ["lang"] [i] ["LANG_NAME_EN"].ToString ();//English Name 
					details.LANG_NAME = itemData ["lang"] [i] ["LANG_NAME"].ToString ();//Native name of English
					if (itemData ["lang"] [i] ["DIR"].ToString () == "ltr") {
						LanguageHandler.instance.Languages.Add (new LanguageData (details.LANG_NAME_EN, details.LANG_CULTURE_NAME,
							LanguageData.LanguageTextAlignment.LeftToRight));
						AddLanguageInXMLFile (details.LANG_CULTURE_NAME, details.LANG_NAME_EN, details.LANG_NAME, "false");
						SmartCultureInfo newInfo = new SmartCultureInfo (details.LANG_CULTURE_NAME, details.LANG_NAME_EN, details.LANG_NAME, false);
						OnCreateLanguageClick (newInfo);
                     
					} else {
						LanguageHandler.instance.Languages.Add (new LanguageData (details.LANG_NAME_EN, details.LANG_CULTURE_NAME, 
							LanguageData.LanguageTextAlignment.RightToLeft));
						AddLanguageInXMLFile (details.LANG_CULTURE_NAME, details.LANG_NAME_EN, details.LANG_NAME, "true");
						SmartCultureInfo newInfo = new SmartCultureInfo (details.LANG_CULTURE_NAME, details.LANG_NAME_EN, details.LANG_NAME, true);
						OnCreateLanguageClick (newInfo);
                       
					}
				}
				//  Below code is fuctioning only  when the prefab of lang handler is break in main scene
				// PrefabUtility.SetPropertyModifications(LanguageHandler.instance.gameObject,
				//  PrefabUtility.GetPropertyModifications(LanguageHandler.instance.gameObject));
				Object parent = PrefabUtility.GetPrefabParent (LanguageHandler.instance.gameObject);
				PrefabUtility.ReplacePrefab (LanguageHandler.instance.gameObject, parent, ReplacePrefabOptions.ConnectToPrefab);
				EditorUtility.DisplayDialog ("Language setup", "Languages will be added to project", "OK");
				this.Close ();
			}
			if (createListContextMenu == null ||
			             createListAdaptor == null ||
			             settingsContextMenu == null ||
			             settingsAdaptor == null ||
			             languageListAdaptor == null ||
			             languageListContextMenu == null) {
				InitializeCultureCollections (true);
			}
		}

		#endregion

		#region AddLanguageDetailsInCultureInfo

		void AddLanguageInXMLFile (string languageCode, string englishName, string nativeName, string isRightToLeft)
		{
			for (int i = 0; i < cultureInfoNodeList.Count; i++) {
				if (cultureInfoNodeList [i].FirstChild.InnerText == languageCode) {
					Debug.Log ("TestingFileIsAlreadyExits");
					return;
				} else {
					Debug.Log ("TestingProceedingToAdd");
				}
                    
			}
			XmlNode RootNode = doc.SelectSingleNode ("//CultureInfos");

			XmlNode cultureInfo = doc.CreateElement ("CultureInfo");

			XmlNode langCode = doc.CreateElement ("languageCode");

			langCode.InnerText = languageCode;
			cultureInfo.AppendChild (langCode);

			XmlNode engName = doc.CreateElement ("englishName");
			engName.InnerText = englishName;
			cultureInfo.AppendChild (engName);

			XmlNode natName = doc.CreateElement ("nativeName");
			natName.InnerText = nativeName;
			cultureInfo.AppendChild (natName);

			XmlNode alinghMent = doc.CreateElement ("isRightToLeft");
			alinghMent.InnerText = isRightToLeft;
			cultureInfo.AppendChild (alinghMent);
			doc.DocumentElement.AppendChild (cultureInfo);

			RootNode.AppendChild (cultureInfo);
			doc.Save ("Assets\\SmartLocalizationWorkspace\\Data\\SmartCultureInfoData\\SmartCultureInfo.xml");

		}

		#endregion

		#region GUI Sections

		void ShowCreatedLanguages ()
		{
			if (languageListContextMenu == null || languageListAdaptor == null) {
				this.Close ();
			}

			ReorderableListGUI.Title ("Created Languages");
			EditorGUILayout.Space ();
			scrollPosition = GUILayout.BeginScrollView (scrollPosition);
			languageListContextMenu.Draw (languageListAdaptor);
			GUILayout.EndScrollView ();
		}

		#endregion

		#region Event Handlers

		public void OnCreateLanguageClick (SmartCultureInfo info)
		{
			SmartCultureInfo chosenCulture = allCultures.FindCulture (info);
			if (chosenCulture == null) {
				Debug.LogError ("The language: " + info.englishName + " could not be created");
				return;
			}
			if (FileUtility.Exists (LocalizationWorkspace.LanguageFilePath (chosenCulture.languageCode))) {
				Debug.LogError ("The language: " + info.englishName + " is already Exists.");
				return;
			}
			LanguageHandlerEditor.CreateNewLanguage (chosenCulture.languageCode);
			InitializeCultureCollections ();
		}

		#endregion

		[MenuItem ("ProjectSetup/Langauge/Add Languages")]
		public static void ShowWindow ()
		{
			SmartLocalizationWindowD window = new SmartLocalizationWindowD ();
			window.titleContent.text = "Add Language Window";
			window.ShowUtility ();
		}
	}
}