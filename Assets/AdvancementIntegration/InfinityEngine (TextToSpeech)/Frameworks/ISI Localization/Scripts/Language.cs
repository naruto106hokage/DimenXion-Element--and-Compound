/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/

using System;

namespace InfinityEngine.Localization
{

    /// <summary>
    ///  Languages of ISILocalization plugin
    /// </summary>
    [Serializable]
    public enum Language
    {
        English,
        French,
        Spanish,
        Italian,
        German,
        Korean,
        Chinese,
        Japanese,
        Russian,
        Portuguese,
        Polish,
        Bengali,
        Bosnian,
        Catalan,
        Czech,
        Welsh,
        Danish,
        Finnish,
        Hindi,
        Croatian,
        Hungarian,
        Indonesian,
        Dutch,
        Slovak,
        Albanian,
        Serbian,
        Swedish,
        Swahili,
        Tamil,
        Thai,
        Turkish,
        Vietnamese,
    }

    /// <summary>
    /// Extension class of <see cref="Language"/> enum.
    /// </summary>
    public static class LanguageExtensions
    {
        /// <summary>
        /// Gets the locale object corresponding to this <see cref="Language"/>
        /// </summary>
        /// <param name="language">This language</param>
        /// <returns>A Locale object</returns>
        public static Locale ToLocale(this Language language)
        {
            return Locale.WithName(language.ToString());
        }
        public static string Code(this Language language)
        {
            return language.ToLocale().LanguageCode;
        }
    }
}