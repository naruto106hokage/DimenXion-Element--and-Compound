using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LanguageHandler))]
public class LanguageHandlerEditor : Editor
{
    string[] _choices;

    public override void OnInspectorGUI ()
    {
        LanguageHandler _LH = target as LanguageHandler;
        _choices = new string[_LH.Languages.Count];
        for (int i = 0; i < _LH.Languages.Count; i++)
        {
            _choices[i] = _LH.Languages[i].DisplayName;
        }
        #region Current Language
        DrawDefaultInspector();
        GUILayout.Space(5);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Current Language :");
        EditorGUILayout.LabelField(PlayerPrefs.GetString("currentLanguage"),EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        Undo.RecordObject(target,"Change Default Language");
        #endregion
        #region Default Language
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Space(6);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Deafult Language :");
        EditorGUILayout.LabelField("en-US", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
        EditorGUILayout.EndVertical();

        Undo.RecordObject(target, "defaultLanguage");
        #endregion
    }
}
