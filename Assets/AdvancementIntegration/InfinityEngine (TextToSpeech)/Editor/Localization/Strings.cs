/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *
*************************************************************************************************************************************/

namespace InfinityEditor
{

    /// <summary>
    ///  Static references to localized strings
    /// </summary>
    internal class Strings
    {

        #region Code

        /// <summary>
        /// 
        /// </summary>
        public const string ClassCommentary = "\tThis class is generated automaticaly by InfinityEngine, it contains constants used by many scripts.  DO NOT EDIT IT !";
        public static string R2ClassBaseCode =
            string.Concat(
                "\tpublic struct PrefGetSet<T>{\n",
                "\t\tprivate string mKey;\n\n",
                "\t\t///<summary> The key of the encapsulated preference</summary>\n",
                "\t\tpublic string Key { get { return mKey; } }\n\n",
                "\t\t///<summary> The value of the encapsulated preference</summary>\n",
                "\t\tpublic T Value { get { return VP.Get<T>(mKey); } set { VP.Set(mKey, value); } }\n\n",
                "\t\t///<summary>Creates new instance of this struct</summary>\n",
                "\t\tpublic PrefGetSet(string key)\n",
                "\t\t {\n",
                "\t\t\t mKey = key;\n",
                "\t\t }\n",
                "\t}\n"
            );

        #endregion Code

        #region GLOBAL

        public static string HELP { get { return EditorLocalization.GetLocalizedString("HELP"); } }
        public static string HIDE { get { return EditorLocalization.GetLocalizedString("HIDE"); } }
        public static string Settings { get { return EditorLocalization.GetLocalizedString("SETTING"); } }
        public static string CLEAR { get { return EditorLocalization.GetLocalizedString("CLEAR"); } }
        public static string Name { get { return EditorLocalization.GetLocalizedString("NAME"); } }
        public static string Value { get { return EditorLocalization.GetLocalizedString("VALUE"); } }
        public static string Create { get { return EditorLocalization.GetLocalizedString("CREATE"); } }
        public static string Cancel { get { return EditorLocalization.GetLocalizedString("CANCEL"); } }
        public static string Confirm { get { return EditorLocalization.GetLocalizedString("CONFIRM"); } }
        public static string FILE { get { return EditorLocalization.GetLocalizedString("FILE"); } }
        public static string SHOW { get { return EditorLocalization.GetLocalizedString("SHOW"); } }
        public static string LANGUAGE { get { return EditorLocalization.GetLocalizedString("LANGUAGE"); } }
        public static string NEW { get { return EditorLocalization.GetLocalizedString("NEW"); } }
        public static string IMPORT { get { return EditorLocalization.GetLocalizedString("IMPORT"); } }
        public static string EXPORT { get { return EditorLocalization.GetLocalizedString("EXPORT"); } }
        public static string Save { get { return EditorLocalization.GetLocalizedString("SAVE"); } }
        public static string TaskPerformed { get { return EditorLocalization.GetLocalizedString("TASK_PERFORMED"); } }
        public static string UPDATE { get { return EditorLocalization.GetLocalizedString("UPDATE"); } }
        public static string Load { get { return EditorLocalization.GetLocalizedString("LOAD"); } }
        public static string OpenEditor { get { return EditorLocalization.GetLocalizedString("OPEN_EDITOR"); } }
        public static string Type { get { return EditorLocalization.GetLocalizedString("TYPE"); } }
        public static string Resources { get { return EditorLocalization.GetLocalizedString("RESOURCES"); } }

        public static string Enable { get { return EditorLocalization.GetLocalizedString("ENABLE"); } }
        public static string Duration { get { return EditorLocalization.GetLocalizedString("DURATION"); } }
        public static string Delay { get { return EditorLocalization.GetLocalizedString("DELAY"); } }

        public static string Ease { get { return EditorLocalization.GetLocalizedString("EASE"); } }
        public static string RepeatCount { get { return EditorLocalization.GetLocalizedString("REPEAT_COUNT"); } }
        public static string RepeatInterval { get { return EditorLocalization.GetLocalizedString("REPEAT_INTERVAL"); } }
        public static string RepeatType { get { return EditorLocalization.GetLocalizedString("REPEAT_TYPE"); } }


        public static string From { get { return EditorLocalization.GetLocalizedString("FROM"); } }
        public static string To { get { return EditorLocalization.GetLocalizedString("TO"); } }

        
        public static string CSharpReversedWord { get { return EditorLocalization.GetLocalizedString("CSHARP_RESERVED_WORD_ERROR"); } }
        public static string EmptyFieldName { get { return EditorLocalization.GetLocalizedString("EMPTY_FIELD_NAME_ERROR"); } }
        public static string SpaceError { get { return EditorLocalization.GetLocalizedString("SPACE_ERROR"); } }
        public static string UnauthorizedStartCharError { get { return EditorLocalization.GetLocalizedString("UNAUTHORIZED__START_CHAR_ERROR"); } }
        public static string UnauthorizedCharError { get { return EditorLocalization.GetLocalizedString("UNAUTHORIZED_CHAR_ERROR"); } }

        public static string DragAndDrop { get { return EditorLocalization.GetLocalizedString("DRAG_AND_DROP_FILE"); } }
        public static string Sort { get { return EditorLocalization.GetLocalizedString("SORT"); } }
        public static string Options { get { return EditorLocalization.GetLocalizedString("OPTIONS"); } }
        public static string Missing { get { return EditorLocalization.GetLocalizedString("MISSING"); } }
        public static string Empty { get { return EditorLocalization.GetLocalizedString("EMPTY"); } }
        public static string Mode { get { return EditorLocalization.GetLocalizedString("MODE"); } }

        #endregion GLOBAL

        #region ISI Resource

        public static string __DatabaseResDuplication = "__DATABASE_RES_DUPLICATION";
        public static string __DatabaseSearchOf = "__DATABASE_SEARCH_OF";
        public static string __DatabaseDefaultPath = "__DATABASE_DEFAULT_PATH";
        
        public static string Database { get { return EditorLocalization.GetLocalizedString("DATABASE"); } }
        public static string DatabaseResGeneration { get { return EditorLocalization.GetLocalizedString("DATABASE_RES_GENERATION"); } }
        public static string DatabaseUpdated { get { return EditorLocalization.GetLocalizedString("DATABASE_UPDATED"); } }
        public static string DatabaseIncludeFolders { get { return EditorLocalization.GetLocalizedString("DATABASE_INCLUDE_FOLDERS"); } }
        public static string DatabaseExcludeFolders { get { return EditorLocalization.GetLocalizedString("DATABASE_EXCLUDE_FOLDERS"); } }
        public static string DatabaseAlreadyIncluded { get { return EditorLocalization.GetLocalizedString("DATABASE_ALREADY_INCLUDED"); } }

        public static string ExclusionArea { get { return EditorLocalization.GetLocalizedString("EXCLUSION_AREA"); } }
        public static string InclusionArea { get { return EditorLocalization.GetLocalizedString("INCLUSION_AREA"); } }

        public static string DatabaseUpdateHelp { get { return EditorLocalization.GetLocalizedString("DATABASE_UPDATE_HELP"); } }
        public static string DatabaseIncludeHelp { get { return EditorLocalization.GetLocalizedString("DATABASE_INCLUDE_HELP"); } }
        public static string DatabaseExcludeHelp { get { return EditorLocalization.GetLocalizedString("DATABASE_EXCLUDE_HELP"); } }
        public static string DatabaseResHelp { get { return EditorLocalization.GetLocalizedString("DATABASE_RES_HELP"); } }

        public static string PoolManagerHelp { get { return EditorLocalization.GetLocalizedString("POOL_MANAGER_HELP"); } }
        public static string PoolManagerNoPrefab { get { return EditorLocalization.GetLocalizedString("POOL_MANAGER_NO_PREFAB"); } }

        #endregion ISI Resource

        #region Sequencer

        public static string SelectedAnim { get { return EditorLocalization.GetLocalizedString("SELECTED_ANIM"); } }
        public static string ComponentToAnimate { get { return EditorLocalization.GetLocalizedString("COMPONENT_TO_ANIMATE"); } }
        public static string AnimatedComponentHelp { get { return EditorLocalization.GetLocalizedString("ANIMATED_COMPONENT_HELP"); } }
        public static string AnimMissingTransform { get { return EditorLocalization.GetLocalizedString("ANIM_MISSING_TRANSFORM"); } }
        public static string AnimMissingFadable { get { return EditorLocalization.GetLocalizedString("ANIM_MISSING_FADABLE"); } }
        public static string AnimMissingColorable { get { return EditorLocalization.GetLocalizedString("ANIM_MISSING_COLORABLE"); } }
        public static string SequencerHelp { get { return EditorLocalization.GetLocalizedString("SEQUENCER_HELP"); } }
        public static string SequenceNameWarning { get { return EditorLocalization.GetLocalizedString("SEQUENCE_NAME_WARNING"); } }
        public static string DisableOnHide { get { return EditorLocalization.GetLocalizedString("DISABLE_ON_HIDE"); } }
        public static string DisableOnHideHelp { get { return EditorLocalization.GetLocalizedString("DISABLE_ON_HIDE_HELP"); } }
        public static string DisableOnPause { get { return EditorLocalization.GetLocalizedString("DISABLE_ON_PAUSE"); } }
        public static string DisableOnPauseHelp { get { return EditorLocalization.GetLocalizedString("DISABLE_ON_PAUSE_HELP"); } }
        public static string SetAsOther { get { return EditorLocalization.GetLocalizedString("SET_AS_OTHER"); } }
        public static string SetAsOtherHelp { get { return EditorLocalization.GetLocalizedString("SET_AS_OTHER_HELP"); } }


        public static string SequenceHeadingHelp { get { return EditorLocalization.GetLocalizedString("SEQUENCE_HEADING_HELP"); } }       
        public static string SequenceDurationWarning{ get { return EditorLocalization.GetLocalizedString("SEQUENCE_DURATION_WARNING"); } }
        public static string Timeline { get { return EditorLocalization.GetLocalizedString("TIMELINE"); } }
        public static string Time { get { return EditorLocalization.GetLocalizedString("TIME"); } }
        public static string TimeHelp { get { return EditorLocalization.GetLocalizedString("TIME_HELP"); } }

        public static string AnimValueHelp { get { return EditorLocalization.GetLocalizedString("ANIM_VALUE_HELP"); } }
        public static string Current { get { return EditorLocalization.GetLocalizedString("CURRENT"); } }

        public static string AnimEaseHelp { get { return EditorLocalization.GetLocalizedString("ANIM_EASE_HELP"); } }
        public static string AnimLoopHelp { get { return EditorLocalization.GetLocalizedString("ANIM_LOOP_HELP"); } }
   
        public static string RotationMode { get { return EditorLocalization.GetLocalizedString("ROTATION_MODE"); } }
        public static string RotationModeHelp { get { return EditorLocalization.GetLocalizedString("ROTATION_MODE_HELP"); } }

        public static string DisableGameObjectAtEnd { get { return EditorLocalization.GetLocalizedString("DISABLE_GAMEOBJECT_AT_END"); } }
        public static string DisableGameObjectAtEndHelp { get { return EditorLocalization.GetLocalizedString("DISABLE_GAMEOBJECT_AT_END_HELP"); } }


        public static string OnStartCallback { get { return EditorLocalization.GetLocalizedString("ON_START_CALLBACK"); } }
        public static string OnUpdateCallback { get { return EditorLocalization.GetLocalizedString("ON_UPDATE_CALLBACK"); } }
        public static string OnCompleteCallback { get { return EditorLocalization.GetLocalizedString("ON_COMPLETE_CALLBACK"); } }
        public static string OnTerminateCallback { get { return EditorLocalization.GetLocalizedString("ON_TERMINATE_CALLBACK"); } }
        public static string AnimationCallbackHelp { get { return EditorLocalization.GetLocalizedString("ANIM_CALLBACK_HELP"); } }
        public static string PlayOnStart { get { return EditorLocalization.GetLocalizedString("PLAY_ON_START"); } }
        public static string SequenceToPlay { get { return EditorLocalization.GetLocalizedString("SEQUENCE_TO_PLAY"); } }
        public static string SequenceToPlayHelp { get { return EditorLocalization.GetLocalizedString("SEQUENCE_TO_PLAY_HELP"); } }
 
        public static string StartSound { get { return EditorLocalization.GetLocalizedString("ON_START_SOUND"); } }
        public static string CompleteSound { get { return EditorLocalization.GetLocalizedString("ON_COMPLETE_SOUND"); } }
        public static string AnimationSoundHelp { get { return EditorLocalization.GetLocalizedString("ANIM_SOUND_HELP"); } }
       
        public static string MeshRendererMissingMaterialWarning { get { return EditorLocalization.GetLocalizedString("MESH_RENDERER_MISSING_MATERIAL"); } }
        public static string __AnimFadeWarning = "__ANIM_FADE_WARNING";
        public static string __AnimColorWarning = "__ANIM_COLOR_WARNING";


        #endregion Sequencer

        #region GPG

        public static string GPGUnlockAtRuntimeHelp { get { return EditorLocalization.GetLocalizedString("UNLOCK_AT_RUNTIME_HELP"); } }
        public static string GPGSidsHelp { get { return EditorLocalization.GetLocalizedString("GPGSIDS_HELP"); } }
        public static string GPGReachSeveralTimesHelp { get { return EditorLocalization.GetLocalizedString("REACH_SEVERAL_TIMES_HELP"); } }
        public static string GPGReachInARowHelp { get { return EditorLocalization.GetLocalizedString("IN_A_ROW_HELP"); }}
        public static string __GPGUnlockPrefHelp = "__UNLOCK_PREF_HELP";
        public static string __GPGUnlockValueHelp = "__UNLOCK_VALUE_HELP";

        public static string __GPGUnlockCase01 = "__UNLOCK_CASE_01";
        public static string __GPGUnlockCase02 = "__UNLOCK_CASE_02";
        public static string __GPGUnlockCase03 = "__UNLOCK_CASE_03";
        public static string __GPGUnlockCase04 = "__UNLOCK_CASE_04";
        

        #endregion GPG

        #region ISI Localization

        public static string ISILocalizationDictionary { get { return EditorLocalization.GetLocalizedString("ISI_LOCALIZATION_DICTIONARY"); } }
        public static string ISILocalizationTranslation { get { return EditorLocalization.GetLocalizedString("ISI_LOCALIZATION_TRANSLATION"); } }
        public static string ISILocalizationHelp { get { return EditorLocalization.GetLocalizedString("ISI_LOCALIZATION_HELP"); } }

        public static string ISILocalizationMissingLanguage { get { return EditorLocalization.GetLocalizedString("ISI_LOCALIZATION_MISSING_LANGUAGE"); } }
        public static string ISILocalizationCheckError { get { return EditorLocalization.GetLocalizedString("ISI_LOCALIZATION_CHECK_ERROR"); } }
        public static string ISILocalizationUpdated { get { return EditorLocalization.GetLocalizedString("ISI_LOCALIZATION_UPDATED"); } }
        public static string ISILocalizationDuplicatedLanguage { get { return EditorLocalization.GetLocalizedString("ISI_LOCALIZATION_DUPLICATED_LANGUAGE"); } }

        public static string __ISILocalizationErrorAt = "__ISI_LOCALIZATION_DUPLICATED_KEY";
        public static string __ISILocalizationDuplicatedKey = "__ISI_LOCALIZATION_DUPLICATED_KEY";

        #endregion ISI LOCALIZATION

        /// <summary>
        /// Gets a formated version of the given localized string
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="data">The data to replaces</param>
        /// <returns>The localized string of with the given key</returns>
        public static string Format(string key, params object[] data)
        {
            return EditorLocalization.GetLocalizedString(key, data);
        }

    }

}