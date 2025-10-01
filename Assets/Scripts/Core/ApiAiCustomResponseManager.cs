using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using LitJson;
using ApiAiSDK.Model;
using System.Linq;

public enum ApiAiCustomResponseManagerState
{
    DEFAULT,
    PROCESSING,
    SUCCESS,
    FAILURE
}

public class APIAICustomFullfillmentData
{
    public string LevelID;
    //    public Dictionary<string, string>[] Modules;
    public string ActionIncomplete;
}

public class APIAICustomFullfillment : Fulfillment
{
    public APIAICustomFullfillmentData Data;

    public APIAICustomFullfillment()
        : base()
    {
        Data = new APIAICustomFullfillmentData();
    }
}

[System.Serializable]
public class APIAIResponseCustomResult : Result
{
    public string ActionIncomplete;
    public new APIAICustomFullfillment Fulfillment;

    public APIAIResponseCustomResult()
    {
        Fulfillment = new APIAICustomFullfillment();
    }
}

[System.Serializable]
public class APIAICustomResponse : AIResponse
{
    public new APIAIResponseCustomResult Result;

    public APIAICustomResponse()
    {
        Result = new APIAIResponseCustomResult();
    }
}


public class ApiAiCustomResponseManager : MonoBehaviour
{
    //	public static ApiAiCustomResponseManager instance;

    public ApiAiCustomResponseManagerState currentState;

    internal APIAICustomResponse currentResponse;
   

    SpeechRecognizationManager speechRecognizationManager;

    string client_Id;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("MarketType"))
        {
            client_Id = "38ab6da965ed4f0dbdab9edcb98e11ba";
        }
        if (PlayerPrefs.GetString("MarketType") == "B2B")
        {
            client_Id = "38ab6da965ed4f0dbdab9edcb98e11ba";
        }
        else if (PlayerPrefs.GetString("MarketType") == "B2C")
        {
            client_Id = "e3ff10b48a6c466b8bfda4074dca4b8e";
        }
    }

    void Start()
    {
        speechRecognizationManager = GetComponent<SpeechRecognizationManager>();
    }

    public void TextRequest(string text)
    {
        currentState = ApiAiCustomResponseManagerState.DEFAULT;

        Dictionary<string,string> mainHeader = new Dictionary<string, string>();
        WWWForm form = new WWWForm();
        mainHeader = form.headers;

        mainHeader["Authorization"] = "Bearer " + client_Id;

        if (mainHeader.ContainsKey("Content-Type"))
        {

            mainHeader["Content-Type"] = "application/json";

        }
        else
        {

            mainHeader.Add("Content-Type", "application/json");
        }

        ApiData temp = new ApiData();

        temp.query = text;
        temp.lang = "en";
        temp.sessionId = "10";

//		temp.contexts = new List<Contexts> (1);
//		temp.contexts.Add (new Contexts ());

        temp.originalRequest = new OriginalRequest();
        string[] TopicID = Application.identifier.Split('.');
        if (TopicID.Length > 3)
        {
            temp.originalRequest.data.moduleId = TopicID[3];
            print(temp.originalRequest.data.moduleId);
        }
        else
        {
            Debug.LogError("Topic ID is not correct in Package Name");
        }


        //	temp.contexts [0].name = "daydream";
        //	temp.contexts [0].lifespan = 1;

        //		string jsonData = JsonUtility.ToJson (new ApiData { 
        //
        //			query = text,
        //			lang = "en",
        //			sessionId="1",
        //			v="20150910"
        //		});


        string jsonData = JsonUtility.ToJson(temp);
        Debug.Log(jsonData);

        byte[ ] postData = System.Text.Encoding.ASCII.GetBytes(jsonData);
	

        text = text.Replace(" ", "%20");
	

//		WWW www = new WWW ("https://api.dialogflow.com/v1/query?query=" + text + "&lang=en&sessionId=1&v=20150910", null, mainHeader);
        WWW www = new WWW("https://api.dialogflow.com/v1/query?v=20150910", postData, mainHeader);
	
        StartCoroutine(Request(www));
    }

    IEnumerator Request(WWW www)
    {
        currentState = ApiAiCustomResponseManagerState.PROCESSING;

        yield return www;

        if (www.error == null)
        {
			
            currentState = ApiAiCustomResponseManagerState.SUCCESS;

            currentResponse = new APIAICustomResponse();

            Debug.Log(www.text);
            JsonData itemData = JsonMapper.ToObject(www.text);

            currentResponse.Result.Action = itemData["result"]["action"].ToString();
            currentResponse.Result.ActionIncomplete = itemData["result"]["actionIncomplete"].ToString();

            currentResponse.Result.ResolvedQuery = itemData["result"]["resolvedQuery"].ToString();

            string temp = itemData["result"]["parameters"].ToJson();
            currentResponse.Result.Parameters = JsonMapper.ToObject<Dictionary<string, object>>(temp);

            currentResponse.Result.Fulfillment.Speech = itemData["result"]["fulfillment"]["speech"].ToString();
            if ((itemData["result"]["fulfillment"]as IDictionary).Contains("data"))
            {
                try
                {
                    if ((itemData["result"]["fulfillment"]["data"]as IDictionary).Contains("levelId"))
                    {
                        currentResponse.Result.Fulfillment.Data.LevelID = itemData["result"]["fulfillment"]["data"]["levelId"].ToString();
                        Debug.Log("apicount " + currentResponse.Result.Fulfillment.Data.LevelID);
                    }
                }
                catch
                {
                    Debug.Log("catch_1");
                }

//                if ((itemData["result"]["fulfillment"]["data"]as IDictionary).Contains("modules"))
//                {
//
//                    temp = itemData["result"]["fulfillment"]["data"]["modules"].ToJson();
//                    currentResponse.Result.Fulfillment.Data.Modules = JsonMapper.ToObject<Dictionary<string, string>[]>(temp);
//                }
//
//                if ((itemData["result"]["fulfillment"]["data"]as IDictionary).Contains("actionIncomplete"))
//                {
//                    currentResponse.Result.Fulfillment.Data.ActionIncomplete = itemData["result"]["fulfillment"]["data"]["actionIncomplete"].ToString();
//                }
            }
            else
            {
                currentResponse.Result.Fulfillment.Data = null;
            }
            temp = itemData["result"]["contexts"].ToJson();
            currentResponse.Result.Contexts = JsonMapper.ToObject<AIOutputContext[]>(temp);

            for (int i = 0; i < currentResponse.Result.Contexts.Length; i++)
            {
                currentResponse.Result.Contexts[i].Name = itemData["result"]["contexts"][i]["name"].ToString();
                currentResponse.Result.Contexts[i].Lifespan = int.Parse(itemData["result"]["contexts"][i]["lifespan"].ToString());

                temp = itemData["result"]["contexts"][i]["parameters"].ToJson();
                currentResponse.Result.Contexts[i].Parameters = JsonMapper.ToObject< Dictionary<string, object>>(temp);
            }
        }
        else
        {
            currentState = ApiAiCustomResponseManagerState.FAILURE;

            Debug.Log("WWW Error : " + www.error);

            speechRecognizationManager.OnErrorFromDialogFlow();
        }
    }
}

[ System.Serializable]	
public class ApiData
{
    public string query;
    public string lang;
    public string sessionId;
    //	public List<Contexts> contexts;
    public OriginalRequest originalRequest;
}

[ System.Serializable]
public class Contexts
{

    public string name;
    public int lifespan;

}

[ System.Serializable]
public class OriginalRequest
{
    public Data data = new Data();
}

[ System.Serializable]
public class Data
{
    public string moduleId;
}