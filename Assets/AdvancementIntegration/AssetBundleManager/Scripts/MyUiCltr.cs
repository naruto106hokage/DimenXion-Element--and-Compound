using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyUiCltr : MonoBehaviour {

	public static MyUiCltr instance;
	#region Variables
	[SerializeField]private EasyBgDownloaderCtl ebdCtl;
	[HideInInspector]
	public string  currentUrl;
	private float progress;
	[SerializeField]private GameObject btn_Cancel;
	[Header("LoadingBar")]
	[SerializeField]private Image loadingBar;
	[SerializeField]private Text percentageAmmount;
	[Header("FileSizeInTheServer&DownloadingSize")]
	public Text fileSize;
	[SerializeField]private Text downloadingFileSize;
    // We holding ChangeProgress  to 0,  When we try to download any lang
    //for second time, When the first time we hit the cancel
    //button while downloading any lang.Below variable is 
    private static float lastValueOfProgress = 0f;
    private static int downloadClickCount = 0;
    private bool runUpdateCondition = true;
    #endregion

    void Awake()
	{
		instance = this;
       // Debug.LogError("I am created");
	}

	void Start () 
	{
		ebdCtl.OnComplete += OnCompleteDownload;
		ebdCtl.OnError += OnErrorDownload;
	}

	void Update () 
	{
		if ( ( !string.IsNullOrEmpty(currentUrl) ) && ( ebdCtl.IsRunning (currentUrl) )) 
		{
            if ((downloadClickCount > 1) && ((ebdCtl.GetProgress(currentUrl) == 1) ||
                (lastValueOfProgress == ebdCtl.GetProgress(currentUrl))) && (runUpdateCondition))
            {
              //  Debug.Log("TestingBingo");
                ChangeProgress(0.0f);
                return;
            }
            runUpdateCondition = false;
            ChangeProgress ( ebdCtl.GetProgress( currentUrl) );
		}
	}

	public void UIDownloading (string url)
	{
        downloadClickCount++;
        if (downloadClickCount > 1)
            runUpdateCondition = true;
        //Debug.Log("TestingDownloadClickCount" + downloadClickCount);
        currentUrl = null;
		ChangeProgress(0.0f);
		currentUrl = url;
        ebdCtl.StartDL(currentUrl);
	}

	public void UICancelDownload (string reason) 
	{
       // Debug.Log("TestingStopDownloadForMyUICLt" + currentUrl);
        if (reason == "ForUpadte")
        {
            lastValueOfProgress = ebdCtl.GetProgress(currentUrl);
        }
        UrlApi.instance.StopAllCoroutines();
        ebdCtl.StopAllCoroutines();
        ebdCtl.StopDL(currentUrl);
        currentUrl = null;
		ChangeProgress(0.0f);
	}
	public void Reset ()
	{
		//Debug.Log ("TestingResetValues");
		fileSize.text = "0MB";
		downloadingFileSize.text = "0MB";
		ChangeProgress(0.0f);
		loadingBar.fillAmount = 0f;
	}

	private void ChangeProgress(float currentProgess)
	{
		progress = currentProgess;
		percentageAmmount.text = ""+ (int)( progress * 100) + " %";
		float downloadingAmount = ( UrlApi.instance.fileSizeInMb / 100 ) * (int)(progress * 100) ;
		downloadingAmount = Mathf.Round (downloadingAmount*100)/100;
		downloadingFileSize.text = "" + downloadingAmount+"MB";
		loadingBar.fillAmount = progress;
	}

	public void OnCompleteDownload(string requestURL, string destPath)
	{
			//Debug.Log ("TestingOnCompleteDownload");
			currentUrl = null;
			ChangeProgress(1f);
			fileSize.text = "0MB";
			InAbsenseOfAssetBundle.Instance.AfterDownloadIsComplete();
	}

	public void OnErrorDownload(string requestURL, EasyBgDownloaderCtl.DOWNLOAD_ERROR ErrorCode ,  string ErrorMsg)
	{
			currentUrl = null;
			ChangeProgress(0f);
			fileSize.text = "0MB";
			InAbsenseOfAssetBundle.Instance.warnigMsg.text = InAbsenseOfAssetBundle.Instance.someThingWentWrong+"\n" +
                                                             InAbsenseOfAssetBundle.Instance.settingDefaultLang;
			InAbsenseOfAssetBundle.Instance.ChooseDefaultLang();
	}

}
