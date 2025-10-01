using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomExclamation))]
public class RandomExclamationEditor : Editor
{
    RandomExclamation _Ex;
    int selGridInt;
    string[] selStrings = {" Well Done", " Try Again"};

    void Awake()
    {
        _Ex = target as RandomExclamation;
    }

    void OnEnable()
    {
        if(_Ex!=null)
        _Ex.GetComponent<LocaliseTextAndVoiceOver>().enabled = false;
    }

    void OnDisable()
    {
        if(_Ex!=null)
        _Ex.GetComponent<LocaliseTextAndVoiceOver>().enabled = false;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (_Ex._WellDone)
            selGridInt = 0;
        else
            selGridInt = 1;
        
        GUILayout.Space(10);
        GUILayout.BeginVertical("Box");
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 2,EditorStyles.radioButton);
        GUILayout.Space(5);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.EndVertical();

        if (selGridInt == 0)
        {
            _Ex._WellDone = true;

            EditorGUILayout.Space();
//            EditorGUILayout.BeginVertical("Box");
//            EditorGUILayout.LabelField("Well Done Key 1 :", _Ex.Wkey1, EditorStyles.boldLabel);
//            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Well Done Key 2 :", _Ex.Wkey2, EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Well Done Key 3 :", _Ex.Wkey3, EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
        }
        else if (selGridInt == 1)
        {
            _Ex._WellDone = false;

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Try Again Key 1 :", _Ex.Tkey1, EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Try Again Key 2 :", _Ex.Tkey2, EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Try Again Key 3 :", _Ex.Tkey3, EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
        }

        Undo.RecordObject(_Ex,"Change Exclamation type");
    }
}