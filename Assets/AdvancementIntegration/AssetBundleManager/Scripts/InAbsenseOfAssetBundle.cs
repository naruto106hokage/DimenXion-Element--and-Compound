using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Linq;


public class InAbsenseOfAssetBundle : MonoBehaviour 
{
	#region Creating Instance
    private static InAbsenseOfAssetBundle instance = null;
    public static InAbsenseOfAssetBundle Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InAbsenseOfAssetBundle>();
            return instance;
        }
    }
	#endregion

	#region Variables
	[SerializeField]private GameObject warningPanel;
    public Text warnigMsg;
	private GameObject canvas_Main;
    [Header("Downloader Canvas")]
	[SerializeField] private GameObject canvas_Downloader;
    [Header("AskForDownload Canvas")]
	[SerializeField] private GameObject canvas_AskForDownload;
	#region warning Text
	internal readonly string settingDefaultLang = "Setting Default Language";
	internal readonly string assetBundleNotInServer = "Asset Bundle is not in server";
	internal readonly string  internetIssue = "Internet is not available";
	internal readonly string  someThingWentWrong = "Something went wrong";
	#endregion
	#endregion
	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

    GameObject FindMainCanvas()
    {
        canvas_Main = FindObjectOfType<LanguageSelectionManager>().gameObject.transform.GetChild(0).gameObject;
        return canvas_Main;
    }

    void OnSceneLoaded(Scene scene , LoadSceneMode mode)
    {
		//Assigning Main Canvas
		if ( (canvas_Main == null) && (SceneManager.GetActiveScene().name == "MainMenu") )
            canvas_Main = FindMainCanvas();

		if ( (PlayerPrefs.GetString("currentLanguage") != LanguageHandler.defaultLanguage) && (!AssetBundleManager.Instance.IsFileExist()) )
			ChooseDefaultLang();
    }
   
    #region Ask For Download  
    public void EnableAskForDownload()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // print("EnableAskForDownload");
            transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, 12f);
            FindMainCanvas();
            canvas_Main.SetActive(false);
        }
        else
        {
            transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, 1f);
        }
        //print("canvas_AskForDownload");
        canvas_AskForDownload.SetActive(true);
    }
	// Click on Skip button
	public void SkipToDownloadBundle()
	{
		canvas_AskForDownload.SetActive(false);
		warnigMsg.text = settingDefaultLang;
		ChooseDefaultLang();
	}
    // Click on download button
    public void StartDownloadingBundle()
    {
		//Debug.Log ("DownloadClicked");
        canvas_AskForDownload.SetActive(false);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			// Show check your internet connect msg
			//Debug.LogError("NoInterNetConnection");
			warnigMsg.text = ""+internetIssue+"\n"+settingDefaultLang;
			ChooseDefaultLang();
		}
		else
		{

			if(canvas_Main != null)
				canvas_Main.SetActive(false);
			canvas_Downloader.SetActive(true);        
			UrlApi.instance.HitApiToStartDownload(); 
		}
    }
   
    #endregion

    #region Downloading&AfterThat
	//cancel clicked
    public void CancelDownloadBundle()
    {
        canvas_Downloader.SetActive(false);
        try
        {
            if (AssetBundleManager.Instance.IsFileExist())
            {
                File.Delete(AssetBundleManager.Instance.GetTheDestinationPath());

                if (AssetBundleManager.Instance.IsFileExist())
                {
                    // Debug.LogError("TestingUnableToDelete");
                    File.Delete(AssetBundleManager.Instance.GetTheDestinationPath());

                }
                else
                {
                   // Debug.Log("TestingFileIsDeletedSuccessFully");
                }
            }

            else
            {
               // Debug.Log("TestingFileIsNotExits");
            }
        }

        catch
        {
           // Debug.Log("TestingWeAreInCatchBecozIssueInDelete");
        }
        MyUiCltr.instance.UICancelDownload("ForUpadte");
		warnigMsg.text = settingDefaultLang;
		ChooseDefaultLang();
    }
    // After Completion 
    public void AfterDownloadIsComplete()
	{
        try
        {
            if (!AssetBundleManager.Instance.IsFileCompletelyDownloaded())
            {
				Debug.LogError ("FileIsNotFullyDownloaded");
                CancelDownloadBundle();
                return;
            }
        }

        catch
        {
            Debug.LogError("NameOfAssetBundleIsNotCorrect");
            CancelDownloadBundle();
        }
		
		//Debug.Log ("TestingAfterFileCompletlyDownloaded");
        canvas_Downloader.SetActive(false);
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
			//Debug.Log ("TestingMainMenuGettingTrue");
            canvas_Main.SetActive(true);
            LanguageSelectionManager.Instance.StartCoroutine("DelayChangeButtonFunctions");
            dpn.DpnCameraRig._instance._center_eye.clearFlags = CameraClearFlags.Skybox;
        }
            
        else
        {
            LanguageHandler.instance.setCurrentLanguage();
            LanguageHandler.instance.changeLanguageUpdate();
            if (LanguageHandler.instance.loadingScene != null)
            {
                LanguageHandler.instance.loadingScene.gameObject.SetActive(true);
            }
        }
       
		MyUiCltr.instance.Reset ();
    }
    public void ChooseDefaultLang()
    {
        dpn.DpnCameraRig._instance._center_eye.clearFlags = CameraClearFlags.SolidColor;
        dpn.DpnCameraRig._instance._center_eye.backgroundColor = Color.black;

        GlobalAudioSrc.Instance.audioSrc.clip = null;
        Debug.Log(this.gameObject.activeSelf);
        this.gameObject.SetActive(true);
        StartCoroutine("ShowWarningMsg");
        MyUiCltr.instance.UICancelDownload("ToCleanUp");

    }

    IEnumerator ShowWarningMsg()
    {
        canvas_Downloader.SetActive(false);
        canvas_AskForDownload.SetActive(false);
        if(canvas_Main != null)
            canvas_Main.SetActive(false);
        warningPanel.SetActive(true);
		warningPanel.transform.SetParent (dpn.DpnCameraRig._instance._center_eye.transform);
		warningPanel.transform.localEulerAngles = Vector3.zero;
		warningPanel.transform.localPosition = new Vector3 (0f, 0f, 0.65f);
        yield return new WaitForSeconds(5f);
        warningPanel.SetActive(false);
		warningPanel.transform.SetParent (this.transform.parent.gameObject.transform);
        PlayerPrefs.SetString("currentLanguage", LanguageHandler.defaultLanguage);
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            LanguageSelectionManager.Instance.SetDefaultSettings();
            dpn.DpnCameraRig._instance._center_eye.clearFlags = CameraClearFlags.Skybox;
        }

        else
        {
            LanguageHandler.instance.setCurrentLanguage();
            LanguageHandler.instance.changeLanguageUpdate();
            if (LanguageHandler.instance.loadingScene != null)
            {
                LanguageHandler.instance.loadingScene.gameObject.SetActive(true);
            }
        }
        if(canvas_Main != null)
            canvas_Main.SetActive(true);
       
		MyUiCltr.instance.Reset ();
    }
    #endregion
    public void OnApplicationPause(bool isPause)  
    {
        //isPause gets true when the application get paused and Become false when the application get resumed
        //if ( !isPause && (! File.Exists(Application.persistentDataPath +"ar")  ) && ( PlayerPrefs.GetString("currentLanguage") == "ar" )  )
        if ( (!isPause) && (PlayerPrefs.GetString("currentLanguage") != LanguageHandler.defaultLanguage) )
        {
			if ((canvas_AskForDownload.activeSelf) || (canvas_Downloader.activeSelf) || (SceneManager.GetActiveScene ().name == "LoadingScene"))
				return;
			if (!AssetBundleManager.Instance.IsFileExist ()) 
			{
				//Debug.Log ("TestingBundleIsNotInThePath");
				ChooseDefaultLang();
			}
				
        }
    }
}
