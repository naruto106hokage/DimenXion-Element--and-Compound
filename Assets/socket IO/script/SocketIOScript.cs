using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using socket.io;
using System.Linq;
using UmetyDatabase;
using LitJson;
using System;


public class ChatData
{
	public string teacherId;
	public string classId;
	public string contId;
	public string action;
	public string androidPkg;
	public List<UserIdDeviceIdModule> userIds = new List<UserIdDeviceIdModule> ();
};

[Serializable]
public class TeacherIdClassId
{
	public string teacherId;
	public string classId;
}



	public class UserIdDeviceIdModule
{
	public string UserId;
	public string DeviceId;
};


public class UserData
{
	public string userId;
	public string message;
};


public class SocketIOScript : MonoBehaviour
{
	public static SocketIOScript instance;

	[SerializeField]
	private bool readyToSendImage = false;

	public string serverURL = "";
	public string serverURLScreenshot = "";
	public string deviceId = "";
	public Socket socket = null;
	protected Socket socketScreenshot = null;
	Camera[] CaptureCameras;
	public RenderTexture screenCaptureTex;
	byte[] imageBytes;
	Texture2D tex;
	String imgToSendInBase64;
	Rect texRect;
	float frameTime;
	UserData userData;
	bool closeImageCapture;
	GameObject captureCam;
	private bool readyForReconnectSocket = false;
	private bool readyForReconnectSocketScreenshot = false;

	public string lastImageString = "";

	void Awake ()
	{
		#if UNITY_EDITOR
		PlayerPrefs.SetString ("teacherId", "1095");
		PlayerPrefs.SetString ("socketURL", "https://socket.umety.com:3000");
		PlayerPrefs.SetString ("socketURLScreenshot", "https://monitor.umety.com:3000");

		#endif
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject); 
		} else if (instance != this) {
			Destroy (this.gameObject);
			return;
		}

		pauseImage = "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";
	}

	void OnEnable ()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}


	void Start ()
	{
		userData = new UserData ();
		tex = new Texture2D (295, 166, TextureFormat.RGB24, false);
		texRect = new Rect (0, 0, 295, 166);

		//Debug.Log ("Url " + PlayerPrefs.GetString ("socketURLScreenshot"));
		//Debug.Log ("Device id " + PlayerPrefs.GetString ("deviceId"));

		serverURL = PlayerPrefs.GetString ("socketURL");
		serverURLScreenshot = PlayerPrefs.GetString ("socketURLScreenshot");
		deviceId = PlayerPrefs.GetString ("deviceId");

		DoOpen ();
		DoOpenForScreenshot ();

		StartCoroutine (SendImageRoutine ());

		//Debug.Log("Teacher Id " + PlayerPrefs.GetString("teacherId"));


	}

	public  int i = 0;
	public int j = 0;

	void Update ()
	{
		

		if (++i == 60) {

			i = 0;

			try {
				Debug.Log ("DC100350 socket.IsConnected : " + socket.IsConnected + "    " + socketScreenshot.IsConnected);	
			} catch (Exception e) {
				Debug.Log ("DC100350 Erroor in check socket.IsConnected : " + e.Message);	
				RecreateSocket.INSTANCE.DistroyAndCreateSocket ();
			}
				

			CheckAndTryReconnectSocket ();
			CheckAndTryReconnectSocketScreenshot ();



			if (socket.IsConnected || socketScreenshot.IsConnected) {
				j = 0;
			} else {
			
				if (++j == 20) {
					CheckBothSocketAndRecreate ();
					j = 0;
				}
			}		

		}
			
	}



	void OnSceneLoaded (Scene scene, LoadSceneMode mode)
	{
		Invoke ("CreateCaptureCameras", 0.1f);
	}


	public void CreateCaptureCameras ()
	{
		CaptureCameras = FindObjectsOfType<Camera> ();
		for (int i = 0; i < CaptureCameras.Length; i++) {
			captureCam = new GameObject ("Capture Camera");

			captureCam.transform.SetParent (CaptureCameras [i].gameObject.transform);
			captureCam.transform.localPosition = Vector3.zero;
			captureCam.transform.localRotation = Quaternion.identity;

			captureCam.AddComponent<Camera> ();
			captureCam.GetComponent<Camera> ().targetTexture = screenCaptureTex;

			captureCam.GetComponent<Camera> ().clearFlags = CaptureCameras [i].clearFlags;
			if (captureCam.GetComponent<Camera> ().clearFlags == CameraClearFlags.Color) {
				captureCam.GetComponent<Camera> ().backgroundColor = Color.black;
			}
			captureCam.GetComponent<Camera> ().cullingMask = CaptureCameras [i].cullingMask;
			captureCam.GetComponent<Camera> ().depth = CaptureCameras [i].depth;
			captureCam.GetComponent<Camera> ().nearClipPlane = CaptureCameras [i].nearClipPlane;
			captureCam.GetComponent<Camera> ().stereoTargetEye = StereoTargetEyeMask.None;
		}
	}



	public void DoOpen ()
	{
		
		if (socket == null) {
			socket = Socket.Connect (serverURL);
			socket.On (SystemEvents.connect, () => {
				
				Debug.Log ("Socket.IO connected for PLAY PAUSE");
			});


			socket.On ("class room", (string data) => {

				Debug.Log ("class room " + data);
				ParseData (data);

			});
		}			
	}


	public void DoOpenForScreenshot ()
	{
		if (socketScreenshot == null) {
			socketScreenshot = Socket.Connect (serverURLScreenshot);
			socketScreenshot.On (SystemEvents.connect, () => {

				Debug.Log ("Socket.IO connected for Monitor");

				if (!PlayPauseSimulation.instance.isPaused)
					SetReadyToSendImage (true, 1f);
			});
				
		}
	}



	void SendChat (string str)
	{
		if (socket != null) {
			socket.Emit ("class room", str);
		}
	}


	public void ParseData (string data)
	{		
		JsonData itemData = JsonMapper.ToObject (data);
		ChatData chat = new ChatData ();
		chat.action = itemData ["action"].ToString ().ToLower();
		chat.androidPkg = itemData ["androidPkg"].ToString ();
		chat.teacherId = itemData ["teacherId"].ToString ();
		chat.classId = itemData ["classId"].ToString ();
		chat.contId = itemData ["contId"].ToString ();


		for (int i = 0; i < itemData ["userIdArr"].Count; i++) {   

			UserIdDeviceIdModule udm = new UserIdDeviceIdModule ();
			udm.UserId = itemData ["userIdArr"] [i] ["userId"].ToString ();
			udm.DeviceId = itemData ["userIdArr"] [i] ["deviceId"].ToString ();
			chat.userIds.Add (udm);
		}


		string contType = itemData ["contType"].ToString ();

		if (contType != "VR-APP")
			return;


		string userId = DatabaseManager.dbm.userID.ToString ();

		bool isCurrectDevice = chat.userIds.Count (e => e.UserId == userId && e.DeviceId == deviceId) > 0;
		if (!isCurrectDevice)
			return;


		TeacherIdClassId teacherIdClassId = LitJson.JsonMapper.ToObject<TeacherIdClassId>(PlayerPrefs.GetString("teacherId"));

		if (teacherIdClassId.classId != chat.classId)
			return;


		if (chat.teacherId == teacherIdClassId.teacherId) {
			if (chat.action == "play") {
				Time.timeScale = 1;
				SetReadyToSendImage (true, 0.5f);

				if (BackMenu.instance != null && BackMenu.instance.IsBackMenuEnabled) {
					PlayPauseSimulation.instance.OnPlayWeb (0);

				} else if (BackMenu.instance != null && !BackMenu.instance.IsBackMenuEnabled) {
					PlayPauseSimulation.instance.OnPlayWeb (1);

				} else {
					PlayPauseSimulation.instance.OnPlayWeb (1);

				}
			} else if (chat.action == "pause") {

				PlayPauseSimulation.instance.OnPauseWeb ();
				SetReadyToSendImage (false, 7f);
			} else if (chat.action == "stop") {
				
				#if!UNITY_EDITOR
				System.Diagnostics.Process.GetCurrentProcess ().Kill ();
				#endif
				Debug.Log ("stop");
				Application.Quit ();
			}
		}


	}



	void CaptureImage ()
	{
		RenderTexture.active = screenCaptureTex;
		tex.ReadPixels (texRect, 0, 0);
		tex.Apply ();
		imageBytes = tex.EncodeToJPG ();
		imgToSendInBase64 = Convert.ToBase64String (imageBytes);
		//pauseTexture.



	}

	public string pauseImage = "";

	public void SendImage ()
	{
		CaptureImage ();

		bool isDifferentImage = imgToSendInBase64 != lastImageString;

		if (!PlayPauseSimulation.instance.isPaused) {

			if (isDifferentImage) {
				userData.userId = DatabaseManager.dbm.userID.ToString ();
				//userData.userId = "42767";
				userData.message = imgToSendInBase64;
				string dataTosend = JsonUtility.ToJson (userData);
				socketScreenshot.EmitJson ("content_monitor", dataTosend);
				//Debug.Log ("Sending Image " + dataTosend);
				lastImageString = imgToSendInBase64;
				//pauseImage = "";
			}
		
		} else {

				userData.userId = DatabaseManager.dbm.userID.ToString();
				//userData.userId = "42767";
				userData.message = pauseImage;
				string dataTosend = JsonUtility.ToJson(userData);
				socketScreenshot.EmitJson("content_monitor", dataTosend);
				//Debug.Log ("Sending Image " + dataTosend);
				lastImageString = pauseImage;
			
		}

	}


	public IEnumerator SendImageRoutine ()
	{
		while (true) {

			if (readyToSendImage && socketScreenshot.IsConnected) {
				SendImage ();
			}
			yield return new WaitForSecondsRT (1f);
			//Debug.Log ("DC100350 SendImageRoutine " + readyToSendImage);	
		}
	}



	public void SetReadyToSendImage (bool value, float delayTime)
	{
		StartCoroutine (SetReadyToSendImageRoutine (value, delayTime));
	}


	public IEnumerator SetReadyToSendImageRoutine (bool value, float delayTime)
	{
		yield return new WaitForSecondsRT (delayTime);
		readyToSendImage = value;
	}


	void OnDisable ()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}


	void CheckBothSocketAndRecreate ()
	{
		if (socket == null || socketScreenshot == null)
			return;


		if (!(socket.IsConnected && socketScreenshot.IsConnected))
			RecreateSocket.INSTANCE.DistroyAndCreateSocket ();

	}


	void CheckAndTryReconnectSocket ()
	{
		if (socket == null)
			return;

		if (socket.IsConnected) {
			readyForReconnectSocket = true;
			return;
		}
		
		if (!readyForReconnectSocket)
			return;

		SocketManager.Instance.Reconnect (socket, 1);
		//RecreateSocket.INSTANCE.DistroyAndCreateSocket ();
		readyForReconnectSocket = false;
		Debug.Log ("DC100350 try to reconnect ");	
	}


	void CheckAndTryReconnectSocketScreenshot ()
	{
		if (socketScreenshot == null)
			return;

		if (socketScreenshot.IsConnected) {
			readyForReconnectSocketScreenshot = true;
			return;
		}

		if (!readyForReconnectSocketScreenshot)
			return;

		//RecreateSocket.INSTANCE.DistroyAndCreateSocket ();
		SocketManager.Instance.Reconnect (socketScreenshot, 1);
		readyForReconnectSocketScreenshot = false;
		Debug.Log ("DC100350 try to reconnect for screenshot");	
	}
		

}



public class WaitForSecondsRT : CustomYieldInstruction
{
	float m_Time;

	public override bool keepWaiting {
		get { return (m_Time -= Time.unscaledDeltaTime) > 0; }
	}

	public WaitForSecondsRT (float aWaitTime)
	{
		m_Time = aWaitTime;
	}

	public WaitForSecondsRT NewTime (float aTime)
	{
		m_Time = aTime;
		return this;
	}
}








