#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayPauseSimulation))]
public class PlayPauseSimulationEditor : Editor
{
    bool play = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayPauseSimulation myScript = (PlayPauseSimulation)target;

        play = myScript.isPaused;

        if (Application.isPlaying && !play)
        {
            if (GUILayout.Button("Pause"))
            {
                myScript.OnPause();
                play = true;
            }
        }
        else if (Application.isPlaying && play)
        {
            if (GUILayout.Button("Play"))
            {
                myScript.OnPlay();
                play = false;
            }
        }
    }
}
#endif