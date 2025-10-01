using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class SpeechEngineEditor : EditorWindow
{
	
	public static void speechEngine_Presist()
	{
		
		Object prefab_AppManager = AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/AppManager.prefab", typeof(GameObject));
		Object prefab_SpeechEngineAllScene = AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/SpeechEngineAllScene.prefab", typeof(GameObject));
	
		if (!File.Exists ("Assets/Prefabs/AppManager.prefab") || !File.Exists ("Assets/Prefabs/SpeechEngineAllScene.prefab")) 
		{
			Debug.LogError ("Place Prefabs folder in right path");
			return;
		}

		for (int i = SceneManager.sceneCountInBuildSettings - 1; i >= 0 ; i--)
		{
			EditorSceneManager.OpenScene (EditorBuildSettings.scenes [i].path, OpenSceneMode.Additive);

			if (i == 0) 
			{
				if (!GameObject.Find ("AppManager")) 
				{
					var p1 = (GameObject)PrefabUtility.InstantiatePrefab (prefab_AppManager, SceneManager.GetSceneByBuildIndex (i));
					var p3 = (GameObject)PrefabUtility.InstantiatePrefab (prefab_SpeechEngineAllScene, SceneManager.GetSceneByBuildIndex (i));
				}
				else
				{
					Debug.LogError ("Setup has already made");
				}
			} 
			else
			{
				if(!GameObject.Find ("SpeechEngineAllScene") && !GameObject.Find ("AppManager")) 
				{

					var p3 = (GameObject)PrefabUtility.InstantiatePrefab (prefab_SpeechEngineAllScene, SceneManager.GetSceneByBuildIndex (i));
				} 
				else
				{
					Debug.LogError("Setup has already made");
				}
			}
			EditorSceneManager.SaveScene (SceneManager.GetSceneByBuildIndex (i));
			EditorSceneManager.CloseScene (SceneManager.GetSceneByBuildIndex (i), true);
		}
	}
}




