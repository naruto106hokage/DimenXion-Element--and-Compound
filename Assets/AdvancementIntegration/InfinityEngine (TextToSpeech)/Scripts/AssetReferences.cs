/*************************************************
* Developed by Mamadou Cisse                     *
* Mail => mciissee@gmail.com                     *
* Twitter => http://www.twitter.com/IncMce       *
* Unity Asset Store catalog: http://u3d.as/riS	 *
*************************************************/


namespace InfinityEngine.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Static reference to our resources placed in Resources folder.
    /// </summary>
    public static class AssetReferences
    {

        #region Fields


        #region Textures

        private static Texture2D m_logo;
        private static Texture2D m_helpIconEnable;
        private static Texture2D m_helpIconDisable;
        private static Texture2D m_searchIcon;
        private static Texture2D m_minusIcon;
        private static Texture2D m_plusIcon;
        private static Texture2D m_SaveIcon;
        private static Texture2D m_LoadIcon;
        private static Texture2D m_EditIcon;
        private static Texture2D m_fbICon;
        private static Texture2D m_twitterIcon;
        private static Texture2D m_docIcon;
        private static Texture2D m_rateIcon;
        private static Texture2D m_moreIcon;
        private static Texture2D m_supportIcon;
        private static Texture2D m_achievementIconEnable;
        private static Texture2D m_leaderboardIconEnable;
        private static Texture2D m_achievementIconDisable;
        private static Texture2D m_leaderboardIconDisable;
        private static Texture2D m_google_translate_icon;

        #endregion Textures

        private static GUISkin m_infinityEditorSkin;
        private static Font m_fontAwesomeFont;

        #endregion Fields

        #region Properties

        #region Textures

        public static Texture2D Logo
        {
            get
            {
                if (m_logo == null)
                    m_logo = Resources.Load<Texture2D>("Textures/logo");
                return m_logo;
            }
        }
        public static Texture2D HelpIconEnable
        {
            get
            {
                if (m_helpIconEnable == null)
                    m_helpIconEnable = Resources.Load<Texture2D>("Textures/help_icon_enable");
                return m_helpIconEnable;
            }
        }
        public static Texture2D HelpIconDisable
        {
            get
            {
                if (m_helpIconDisable == null)
                    m_helpIconDisable = Resources.Load<Texture2D>("Textures/help_icon_disable");
                return m_helpIconDisable;
            }
        }
        public static Texture2D SearchIcon
        {
            get
            {
                if (m_searchIcon == null)
                    m_searchIcon = Resources.Load<Texture2D>("Textures/search_icon");
                return m_searchIcon;
            }
        }
        public static Texture2D MinusIcon
        {
            get
            {
                if (m_minusIcon == null)
                    m_minusIcon = Resources.Load<Texture2D>("Textures/minus_icon");

                return m_minusIcon;
            }
        }
        public static Texture2D PlusIcon
        {
            get
            {
                if (m_plusIcon == null)
                    m_plusIcon = Resources.Load<Texture2D>("Textures/plus_icon");

                return m_plusIcon;
            }
        }
        public static Texture2D SaveIcon
        {
            get
            {
                if (m_SaveIcon == null)
                    m_SaveIcon = Resources.Load<Texture2D>("Textures/save_icon");
                return m_SaveIcon;
            }
        }
        public static Texture2D LoadIcon
        {
            get
            {
                if (m_LoadIcon == null)
                    m_LoadIcon = Resources.Load<Texture2D>("Textures/load_icon");
                return m_LoadIcon;
            }
        }
        public static Texture2D EditIcon
        {
            get
            {
                if (m_EditIcon == null)
                    m_EditIcon = Resources.Load<Texture2D>("Textures/edit_icon");
                return m_EditIcon;
            }
        }
        public static Texture2D FbICon
        {
            get
            {
                if (m_fbICon == null)
                    m_fbICon = Resources.Load<Texture2D>("Textures/facebook_icon");
                return m_fbICon;
            }
        }
        public static Texture2D TwitterIcon
        {
            get
            {
                if (m_twitterIcon == null)
                    m_twitterIcon = Resources.Load<Texture2D>("Textures/twitter_icon");
                return m_twitterIcon;
            }
        }
        public static Texture2D DocIcon
        {
            get
            {
                if (m_docIcon == null)
                    m_docIcon = Resources.Load<Texture2D>("Textures/doc_icon");
                return m_docIcon;
            }
        }
        public static Texture2D RateIcon
        {
            get
            {
                if (m_rateIcon == null)
                    m_rateIcon = Resources.Load<Texture2D>("Textures/rate_icon");
                return m_rateIcon;
            }
        }
        public static Texture2D MoreIcon
        {
            get
            {
                if (m_moreIcon == null)
                    m_moreIcon = Resources.Load<Texture2D>("Textures/more_games_icon");

                return m_moreIcon;
            }
        }
        public static Texture2D SupportIcon
        {
            get
            {
                if (m_supportIcon == null)
                    m_supportIcon = Resources.Load<Texture2D>("Textures/support_icon");
                return m_supportIcon;
            }
        }

        public static Texture2D AchievementIconEnable
        {
            get
            {
                if (m_achievementIconEnable == null)
                    m_achievementIconEnable = Resources.Load<Texture2D>("Textures/achievement_enable");
                return m_achievementIconEnable;
            }
        }
        public static Texture2D LeaderboardIconEnable
        {
            get
            {
                if (m_leaderboardIconEnable == null)
                    m_leaderboardIconEnable = Resources.Load<Texture2D>("Textures/leaderboard_enable");
                return m_leaderboardIconEnable;
            }
        }
        public static Texture2D AchievementIconDisable
        {
            get
            {
                if (m_achievementIconDisable == null)
                    m_achievementIconDisable = Resources.Load<Texture2D>("Textures/achievement_disable");
                return m_achievementIconDisable;
            }
        }
        public static Texture2D LeaderboardIconDisable
        {
            get
            {
                if (m_leaderboardIconDisable == null)
                    m_leaderboardIconDisable = Resources.Load<Texture2D>("Textures/leaderboard_disable");
                return m_leaderboardIconDisable;
            }
        }
        public static Texture2D GoogleTranslateIcon
        {
            get
            {
                if (m_google_translate_icon == null)
                    m_google_translate_icon = Resources.Load<Texture2D>("Textures/google_translate_icon");
                return m_google_translate_icon;
            }
        }

        #endregion Textures

        public static GUISkin InfinityEditorSkin
        {
            get
            {
                if (m_infinityEditorSkin == null)
                    m_infinityEditorSkin = Resources.Load<GUISkin>("Skins/infinity-editor-skin");
                return m_infinityEditorSkin;
            }
        }

        public static GUIStyle AccordionHeader
        {
            get
            {
                return InfinityEditorSkin.GetStyle("AccordionHeader");
            }
        }

        public static GUIStyle FontAwesome
        {
            get
            {
                return InfinityEditorSkin.GetStyle("FontAwesome");
            }
        }

        public static GUIStyle GetFontAwesomeStyle(GUIStyle copy)
        {
            var style = new GUIStyle(copy);
            style.font = FontAwesomeFont;
            return style;

        }

        public static Font FontAwesomeFont
        {
            get
            {
                if(m_fontAwesomeFont == null)
                {
                    m_fontAwesomeFont = Resources.Load<Font>("Fonts/FontAwesome");
                }
                return m_fontAwesomeFont;
            }
        }

        #endregion Properties

    }
}