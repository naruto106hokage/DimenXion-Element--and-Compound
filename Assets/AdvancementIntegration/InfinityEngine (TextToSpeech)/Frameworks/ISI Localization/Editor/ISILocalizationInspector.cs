/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEditor;
using InfinityEngine.Localization;
using InfinityEngine.Utils;

namespace InfinityEditor
{
    /// <summary>
    ///  Custom inspector for <see cref="ISILocalization"/> Component
    /// </summary>
    [CustomEditor(typeof(ISILocalization))]
    public class ISILocalizationInspector : Editor
    {

        #region Fields

        private SerializedProperty nextScene;
        private SerializedProperty loadSceneDelay;
        public bool showHelp;

        #endregion Fields

        #region Overriden Methods

        /// <summary>
        /// This method is called after Unity OnEnable callback.
        /// </summary>
        void OnEnable()
        {
            nextScene = serializedObject.FindProperty("m_nextScene");
            loadSceneDelay = serializedObject.FindProperty("m_loadSceneDelay");

        }

        /// <summary>
        /// Called when the component is drawed
        /// </summary>
        public override void OnInspectorGUI()
        {
                serializedObject.Update();
                showHelp = Utils.DrawHeader(showHelp, @"http://www.mceinc-engine.com.s3-website.eu-west-2.amazonaws.com/Manual-ISI%20Localization.html");
                Utils.ShowMessage(Strings.ISILocalizationHelp, MessageType.Info, showHelp);

                EditorGUILayout.PropertyField(nextScene);
                EditorGUILayout.PropertyField(loadSceneDelay);

                GUILayout.Space(10);
                var rect = Utils.GetCenteredRect(120, 40);
                if (GUI.Button(rect, AssetReferences.EditIcon, GUI.skin.label))
                {
                    ISILocalizationEditor.OpenWindow();
                }
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
                serializedObject.ApplyModifiedProperties();

        }

        #endregion  Overriden Methods
    }
}