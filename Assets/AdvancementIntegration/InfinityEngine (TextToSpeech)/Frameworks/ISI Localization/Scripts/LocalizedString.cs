/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                            *                                                                                                          *
*************************************************************************************************************************************/


using UnityEngine;
using InfinityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using InfinityEngine.Attributes;
using InfinityEngine.Utils;

namespace InfinityEngine.Localization
{

    /// <summary>
    ///     Localized string component.
    ///     
    ///     Add This component to a GameObject which has an <see cref="Text"/> or a <see cref="TextMesh"/> TextMesh Component to Translate the text at runtime 
    /// </summary>
    [AddComponentMenu("InfinityEngine/Localization/Localized String")]
    public class LocalizedString : MonoBehaviour
    {
        /// <summary>
        /// The type of component to localize.
        /// </summary>
        private enum TextComponentType {

            /// <summary>
            /// UnityEngine.UI.Text
            /// </summary>
            Text,
     
            /// <summary>
            /// UnityEngine.TextMesh
            /// </summary>
            TextMesh
        }

        [SerializeField] private TextComponentType type;

        [Popup(R3.strings.Names, PopupValueTypes.String, true)]
        [SerializeField] private string key;

        private Text label;
        private TextMesh textMesh;
        private static List<LocalizedString> LocalizedStrings;

        void Start()
        {
            if (type == TextComponentType.Text)
            {
                label = GetComponent<Text>();
                if (label == null)
                {
                    Debugger.LogError("There is no UnityEngine.UI.Text component attached to this GameObject", this);
                    return;
                }
            }
            else
            {
                textMesh = GetComponent<TextMesh>();
                if (textMesh == null)
                {
                    Debugger.LogError("There is no UnityEngine.TextMesh component attached to this GameObject", this);
                    return;
                }
            }

            if (ISILocalization.LoadIfNotInScene())
            {
                Infinity.When(()=> ISILocalization.IsInitialized, _OnLanguageChanged);
            }

            if (LocalizedStrings == null)
            {
                LocalizedStrings = new List<LocalizedString>();
            }
            if (!LocalizedStrings.Contains(this))
            {
                LocalizedStrings.Add(this);
            }
        }

        private void _OnLanguageChanged()
        {
            if (label == null)
            {
                textMesh.text = ISILocalization.GetValueOf(key);
            }
            else
            {
                label.text = ISILocalization.GetValueOf(key);
            }
        }

        /// <summary>
        /// Callback function invoked when the application language change.
        /// </summary>
        public static  void OnLanguageChanged()
        {
            if (LocalizedStrings != null)
            {
                LocalizedStrings.ForEach(arg => arg._OnLanguageChanged());
            }
        }
    }


}