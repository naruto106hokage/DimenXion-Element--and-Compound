using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(SpeechTesting))]
public class SpeechTestingEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		EditorGUILayout.Space ();

		SpeechTesting speechTesting = (SpeechTesting)target;

		if (GUILayout.Button ("Ask ALEXA ! ")) {
			speechTesting.AskAlexa ();
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Say ALEXA ! ")) {
			speechTesting.SayAlexa ();
			speechTesting.userResponse = "";
		}
	}
}
