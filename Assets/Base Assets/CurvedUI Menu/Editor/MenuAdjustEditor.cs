using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MenuAdjust))]
public class MenuAdjustEditor : Editor
{
    MenuAdjust myTarget;

    void Awake()
    {
        myTarget = target as MenuAdjust;
    }

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

        if (GUILayout.Button ("Adjust"))
        {
            myTarget.noOfLevels = 0;
            myTarget.Adjust ();
		}
        Undo.RecordObject(myTarget,"Change Adjustment");
	}
}
