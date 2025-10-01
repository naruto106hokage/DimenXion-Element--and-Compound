using UnityEngine;
using System.Collections;

namespace GrowlibLauncher{

	public class UnityAPI : MonoBehaviour {

		private static UnityAPI _instance;

		// handler
		public delegate void UnityAPIDelegate(string parm);	
		public UnityAPIDelegate actionHandler;
		public delegate void UnityAPIModeDelegate(int parm);	
		public UnityAPIModeDelegate modeHandler;

		// mode
		public enum PlayControllMode
		{
			Free = 0,
			Control,
		};
		private	PlayControllMode m_mode = PlayControllMode.Free;

		public bool isFreeMode()
		{
			return m_mode == PlayControllMode.Free;
		}


		void Awake(){
			_instance = this;
		}

		public static UnityAPI Instance
		{
			get
			{	
				return _instance;
			}
		}
			
		// Use this for initialization
		void Start () {			
			DontDestroyOnLoad( gameObject );
		}

		#region called by launcher

		public void gotoChapter(string chapter)
		{
			Debug.Log("gotoChapter:"+chapter);

			if (actionHandler != null)
				actionHandler (chapter);
		}
			
		//changeMode(String mode)   // 0 Free mode     1 command mode
		public void changeMode(string mode)
		{
			Debug.Log("changeMode:"+mode);

			m_mode = PlayControllMode.Free;

			int iMode = int.Parse (mode);
			if (iMode == (int)PlayControllMode.Control)
				m_mode = PlayControllMode.Control;
				
			if (modeHandler != null)
				modeHandler ((int)m_mode);
		}


		#endregion
	}
}
