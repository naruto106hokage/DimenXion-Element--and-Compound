using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
using LitJson;

public class UrlApi : MonoBehaviour
{
	public static UrlApi instance;
	public Dictionary<string, string> mainHeader;
	internal float sizeOfFile;
	internal float fileSizeInMb;

	

	string BaseJsonPath = "https://api.umety.com/v1/getlanguageasset?t=";
	string BaseLanguagePath = "&lc=";
	string BasePlatformPath = "&p=vr";

	byte[] rawData;
    private void Awake()
    {
        instance = this;
		Init ();
	}

	void Init() {
		WWWForm form = new WWWForm();
		mainHeader = form.headers;
		rawData = form.data;
		mainHeader["Authorization"] = "Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("unity:@piun!ty"));
	}

    public void HitApiToStartDownload(){
		string Path = GetAPIURl ();

		WWW www = new WWW(Path, null, mainHeader);
		StartCoroutine(GetUrlAndStartDownload(www));
    }

     IEnumerator GetUrlAndStartDownload(WWW www)
    {
        yield return www;
        if (www.error == null)
		{
          //  Debug.Log("TestingApiHitWithNoError");
            JsonData itemData = JsonMapper.ToObject(www.text);
            JsonKeysValue jsonKeys = new JsonKeysValue();
            jsonKeys.type = itemData["type"].ToString();
			if (jsonKeys.type.Equals("Success"))
			{
				jsonKeys.status = (int)itemData ["status"];
				if (jsonKeys.status == 200) 
				{
                    jsonKeys.ASSET_FILE = itemData ["detail"] ["data"] [0] ["ASSET_FILE"].ToString ();
					jsonKeys.ASSET_FILE_SIZE = itemData ["detail"] ["data"] [0] ["ASSET_FILE_SIZE"].ToString ();
					sizeOfFile = int.Parse (jsonKeys.ASSET_FILE_SIZE);
					fileSizeInMb = sizeOfFile / (1024 * 1024); 
					fileSizeInMb = Mathf.Round (fileSizeInMb * 100) / 100;
					MyUiCltr.instance.fileSize.text = "" + fileSizeInMb + "MB";
					MyUiCltr.instance.UIDownloading (jsonKeys.ASSET_FILE);
				}
				else if (jsonKeys.status == 203) 
				{
					//Debug.Log (itemData["detail"].ToString());
					InAbsenseOfAssetBundle.Instance.warnigMsg.text = InAbsenseOfAssetBundle.Instance.assetBundleNotInServer+"\n" 
                                                                     +InAbsenseOfAssetBundle.Instance.settingDefaultLang;
					InAbsenseOfAssetBundle.Instance.ChooseDefaultLang();	
				}
                else
                {
                    InAbsenseOfAssetBundle.Instance.warnigMsg.text = InAbsenseOfAssetBundle.Instance.someThingWentWrong + "\n"
                                                                     + InAbsenseOfAssetBundle.Instance.settingDefaultLang;
                    InAbsenseOfAssetBundle.Instance.ChooseDefaultLang();
                }
            }
			else
			{
				InAbsenseOfAssetBundle.Instance.warnigMsg.text = InAbsenseOfAssetBundle.Instance.someThingWentWrong+"\n" 
                                                                 +InAbsenseOfAssetBundle.Instance.settingDefaultLang;
				InAbsenseOfAssetBundle.Instance.ChooseDefaultLang();	
			}	
		}
		else if (www.error != null)
		{
			//Debug.LogError (www.error);
			InAbsenseOfAssetBundle.Instance.warnigMsg.text =  InAbsenseOfAssetBundle.Instance.someThingWentWrong+"\n" 
                                                              +InAbsenseOfAssetBundle.Instance.settingDefaultLang;
			InAbsenseOfAssetBundle.Instance.ChooseDefaultLang();
		}
    }

	internal string GetAPIURl()
    {
		return ( BaseJsonPath + AssetBundleManager.Instance.GetTopicID() + BaseLanguagePath +  PlayerPrefs.GetString("currentLanguage")+ BasePlatformPath);
	}
}

public class JsonKeysValue
{
    public  string type;
    public int status;
	public string ASSET_FILE;
	public string ASSET_FILE_SIZE;
}
