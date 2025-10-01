using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditorInternal.VR;

public class AdvancementIntegrationEditor : EditorWindow
{

	static bool enabled_1;
	static bool enabled_2;
	static bool enabled_3;


	//const string MENU_NAME_SpeechEngine_1 = "ProjectSetup/Speech Engine/Setup &E";
	//const string MENU_NAME_SpeechEngine_2 = "ProjectSetup/Speech Engine/Documentation";
	const string MENU_NAME_SpeechEngine_3 = "ProjectSetup/Asset Bundle/Setup &S";


	static string Datapath = Application.dataPath ;
	static int Count = "Assets".Length;

	static AdvancementIntegrationEditor()
	{
		//enabled_1 = EditorPrefs.GetBool (MENU_NAME_SpeechEngine_1, false);
		//enabled_2 = EditorPrefs.GetBool (MENU_NAME_SpeechEngine_2, false);
		enabled_3 = EditorPrefs.GetBool (MENU_NAME_SpeechEngine_3, false);
	
	}


	//............................................................................For speech engine....................................................

	public float secs = 1f;
	public float startVal = 0f;
	public float progress = 0f;

	//[MenuItem(AdvancementIntegrationEditor.MENU_NAME_SpeechEngine_1)]
	//static void enableSpeechEngine(){

	//	enabled_1 = true;
	//	Menu.SetChecked (MENU_NAME_SpeechEngine_1, enabled_1);

	//	SpeechEngineEditor.speechEngine_Presist ();
	//}

	//[MenuItem(AdvancementIntegrationEditor.MENU_NAME_SpeechEngine_2)]
	//static void readMe(){

	//	enabled_2 = true;
	//	Menu.SetChecked (MENU_NAME_SpeechEngine_2, enabled_2);

	//	EditorUtility.DisplayDialog ("Documentation", "See console for info. :)", "OK");

	//	string path = "Assets/Resources/ReadMe.txt";

	//	//Read the text from directly from the test.txt file
	//	StreamReader reader = new StreamReader(path); 

	//	Debug.Log (reader.ReadToEnd ());

	//	reader.Close();
	//}


	//............................................................................For asset bundle....................................................
	[MenuItem(AdvancementIntegrationEditor.MENU_NAME_SpeechEngine_3)]
	static	void OnPrefabInvocation(){

		enabled_3 = true;
		Menu.SetChecked (MENU_NAME_SpeechEngine_3,enabled_3);

		if (!AssetDatabase.IsValidFolder ("Assets/AssetBundles")) {
			string guid = AssetDatabase.CreateFolder("Assets", "AssetBundles");
			string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
		}

		Object Prefab_AssetBundle = AssetDatabase.LoadAssetAtPath("Assets/AdvancementIntegration/AssetBundleManager/Prefabs/Prefab_AssetBundle.prefab", typeof(GameObject));

		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {

			EditorSceneManager.OpenScene (EditorBuildSettings.scenes [i].path, OpenSceneMode.Additive);
			if (i == 0) {
		
				if (!GameObject.Find ("Prefab_AssetBundle")) {
					EditorUtility.DisplayDialog ("Status", "Setup is completed For AssetBundler", "OK");
					var p1 = (GameObject)PrefabUtility.InstantiatePrefab (Prefab_AssetBundle, SceneManager.GetSceneByBuildIndex (i));
				} else {
					EditorUtility.DisplayDialog ("Status", "Setup is already exists For AssetBundler", "OK");
				}
			}
			EditorSceneManager.SaveScene (SceneManager.GetSceneByBuildIndex (i));
			EditorSceneManager.CloseScene (SceneManager.GetSceneByBuildIndex (i), true);
		}
	
	}


	[MenuItem("ProjectSetup/Clean Project")]
	static void CleanProject()
	{
		Debug.Log("Cleaning the project...");
		AssetDatabase.DeleteAsset("Assets/Plugins/Mono.Data.Sqlite.dll");
		AssetDatabase.DeleteAsset("Assets/Plugins/Mono.Data.SqliteClient.dll");
		AssetDatabase.DeleteAsset("Assets/Plugins/System.Configuration.dll");
		AssetDatabase.DeleteAsset("Assets/Plugins/System.EnterpriseServices.dll");
		AssetDatabase.DeleteAsset("Assets/Plugins/System.Security.dll");
		AssetDatabase.DeleteAsset("Assets/Database/Scripts/DBScripts/DatabaseConnection.cs");
		AssetDatabase.DeleteAsset("Assets/CurvedUI Menu/Scripts/HandleSyncButton.cs");
		AssetDatabase.DeleteAsset("Assets/Plugins/Mono.Data.dll");
		AssetDatabase.DeleteAsset("Assets/Plugins/System.Data.dll");
		AssetDatabase.DeleteAsset("Assets/Plugins/sqlite3.dll");
		AssetDatabase.DeleteAsset("Assets/Plugins/sqlite3.def");

		Debug.Log("Project cleaned!");
	}



}



