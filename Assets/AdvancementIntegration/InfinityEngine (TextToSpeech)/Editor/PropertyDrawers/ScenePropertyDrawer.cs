/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                            *                                                                                                          *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine;
using UnityEditor;

namespace InfinityEditor.PropertyDrawers
{
    /// <summary>
    ///     Custom attribute drawer of <see cref="Scene"/>
    /// </summary>
    [CustomPropertyDrawer(typeof(Scene))]
    public class ScenePropertyDrawer : PropertyDrawer
    {

        
        private SerializedProperty sceneAsset;
        private SerializedProperty sceneName;
        private Object value;

        /// <summary>
        /// Called when unity draws the property.
        /// </summary>
        /// <param name="position">The position where to draw the property</param>
        /// <param name="property">The SerializedProperty of the property</param>
        /// <param name="label">The label of the property</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            sceneAsset = property.FindPropertyRelative("sceneAsset");
            sceneName = property.FindPropertyRelative("sceneName");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (sceneAsset != null)
            {
                EditorGUI.BeginChangeCheck();

                value = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
                if (EditorGUI.EndChangeCheck())
                {
                    sceneAsset.objectReferenceValue = value;
                    if (sceneAsset.objectReferenceValue != null)
                    {
                        sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
                    }
                }
            }
            EditorGUI.EndProperty();
        }
    }
}