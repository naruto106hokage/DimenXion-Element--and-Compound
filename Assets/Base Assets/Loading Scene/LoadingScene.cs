using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    static int loadingSceneIndex;
	static bool FisrtTime;
    public static int LoadingSceneIndex
    {
        set
        {
            loadingSceneIndex = value;
			Debug.Log (loadingSceneIndex);
			SceneManager.LoadScene("LoadingScene");
        }
    }

    public GameObject LoadingFX;

    AsyncOperation _AO;



    void Start()
    {
        LoadingFX.SetActive(false);
        if (!FisrtTime)
		{
			if (!PlayerPrefs.HasKey ("PlayLevel"))
				PlayerPrefs.SetString ("PlayLevel", "MainMenu");
		
			#region ToGetVersion	
			if (PlayerPrefs.GetString ("PlayLevel").Contains ("_")) 
			{
				string [] version;
				version = PlayerPrefs.GetString ("PlayLevel").Split ('_');
				try 
				{
					PlayerPrefs.SetString ("PlayLevel", version [0]);
					PlayerPrefs.SetString ("PlayVersion", version[1]);	
				}
				catch
				{
					PlayerPrefs.SetString ("PlayLevel", version [0]);
					PlayerPrefs.SetString ("PlayVersion", "0");	
				}
			}
			#endregion
			_AO = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("PlayLevel"));

			if(_AO == null)
				_AO = SceneManager.LoadSceneAsync("MainMenu");

			FisrtTime = true;
		}
		else
		{
			_AO = SceneManager.LoadSceneAsync (loadingSceneIndex);
		}

        _AO.allowSceneActivation = false;
        StartCoroutine(StartLoadingBar());
    }

    IEnumerator StartLoadingBar()
    {
		
        //LoadingFX.SetActive(true);
        while (_AO.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
        }

 		yield return new WaitForSeconds(2f);
        _AO.allowSceneActivation = true;
    }

}
