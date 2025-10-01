using UnityEngine;
using InfinityEngine.Localization;

namespace InfinityEngine.Localization {
	///<summary>
	///	This class is generated automaticaly by InfinityEngine, it contains constants used by many scripts.  DO NOT EDIT IT !
	///</summary>
	public static class R3 {

	public static class strings {

		///<summary>
		///All strings keys separated by ','. You can created an array by using string.split() method
		///</summary>

		public const string Names = "HelloWorld";
		///<summary>
		///Value of the string with the key HelloWorld
		///</summary>

		#if UNITY_ANDROID && !UNITY_EDITOR
		public static ISIString HelloWorld = new ISIString("HelloWorld");
		#endif

	 }
	public static class audios {

		///<summary>
		///All audios keys separated by ','. You can created an array by using string.split() method
		///</summary>

		public const string Names = "Hello";
		///<summary>
		///get property of the AudioClip with the key Hello
		///</summary>

		public static AudioClip Hello { get { return ISILocalization.GetAudio("Hello");} }

	 }
	public static class sprites {

		///<summary>
		///All sprites keys separated by ','. You can created an array by using string.split() method
		///</summary>

		public const string Names = "LocalizedSprite";
		///<summary>
		///get property of the Sprite with the key LocalizedSprite
		///</summary>

		public static Sprite LocalizedSprite { get { return ISILocalization.GetSprite("LocalizedSprite");} }

	 }
	}
}
