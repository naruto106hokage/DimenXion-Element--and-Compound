using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AdjustMenu))]
public class AdjustMenuLayout : Editor {
 AdjustMenu myTarget;

    void Awake()
    {
        myTarget = target as AdjustMenu;
    }

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

        if (GUILayout.Button ("SetLayOut"))
        {
           // myTarget.noOfLevels = 0;
			myTarget.SetLayOut ();
		}
        Undo.RecordObject(myTarget,"Change Adjustment");
	}
}
