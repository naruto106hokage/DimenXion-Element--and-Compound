using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//using System.Data;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using Beebyte.Obfuscator;

namespace UmetyDatabase
{

    public class DatabaseManager : MonoBehaviour
    {



        private AndroidJavaObject util = null;
        private AndroidJavaObject activitiyContext = null;
        int lastId;

        // Instance for accessing variables of this class
        public static DatabaseManager dbm;


        bool masterAppDatabaseNotAcceptingData;

        // Enter your topic ID here very carefully as only according to this your data will be saved and displayed when required
        // ( DO CROSS VERIFY AT LEAST TWO TIMES )
        public string topicID;

        public int totalNumberOfQuestions;

        // These are made public and hide in inspector so as to check these entries at any point of your game by removing HideInInspector
        // and also for making their access possible outside script through instance
        //		[HideInInspector]
        public int userID;
        //		[HideInInspector]
        public string userName;
        //		[HideInInspector]
        public string language;
        //		[HideInInspector]
        public int mode;
        public string deviceName;

        [HideInInspector]
        public string packageName;


        public string logo;

        public List<LevelData> levelsData;

        //		[HideInInspector]
        public List<MyDataEntry> dataEntries;

        // This is for the reading of the database when required internally
        //		private IDataReader reader;

        // These variables are for saving firstDataEntry's ActivityId and lastDataEntry's ActivityId to delete those entries when success is received from server
        private int activityId_FirstDataEntry;
        private int activityId_LastDataEntry;

        void Awake()
        {
            lastId = -1;
            masterAppDatabaseNotAcceptingData = false;

            if (dbm == null)
            {
                dbm = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

			packageName = Application.identifier;

			topicID = packageName.Substring (packageName.LastIndexOfAny (".".ToCharArray ()) + 1,
				(packageName.Length - packageName.LastIndexOfAny (".".ToCharArray ()) - 1)).ToUpper ();

			packageName += "_V" + Application.version;

#if UNITY_EDITOR
            PlayerPrefs.SetInt("userId", 1367);
            PlayerPrefs.SetString("userName", "John Doe");
#endif
            userID = PlayerPrefs.GetInt("userId");
            userName = PlayerPrefs.GetString("userName");
            language = PlayerPrefs.GetString("currentLanguage");
            mode = PlayerPrefs.GetInt("mode");
            deviceName = PlayerPrefs.GetString("platform");
            logo =  PlayerPrefs.GetString("LOGO");
            Debug.Log("Platformmmm"+ deviceName);
            Debug.Log("Imageeee" + logo);
        }

        void Start()
        {
#if !UNITY_EDITOR
            if (!Debug.isDebugBuild)
            {
                if (userID == 0 || userName == null || userName == "")
                {
                    PlayPauseSimulation.instance.ShowStartErrorMessage(":(\n\nContact Support");
                }
            PlayerPrefs.DeleteKey("userId");
            }
#endif
        }

        public void InsertFinalDataToDatabase()
        {
            if (AllQuestionsAttempted())
            {
                Debug.Log("Sending...");
                Invoke("SendDataToMasterApp", .5f);
            }
        }

        String UUID;

        [Skip]
        void SendDataToMasterApp()
        {
            UUID = Guid.NewGuid().ToString();
            //#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activitiyContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }
            print(dataEntries.Count + " : dataEntries.Count");
            for (int i = 0; i < dataEntries.Count; i++)
            {



                //To transfer data to dummy master app
                if (!masterAppDatabaseNotAcceptingData)
                {
                    Debug.Log("DataEntries Question ID " + dataEntries[i].QuestionID);

                    var otherUtil = new AndroidJavaClass("contentproviderplugin.umety.com.contentprovider.Util");

                    int j = otherUtil.CallStatic<int>("SampleFunction");
                    Debug.Log("Calling Function" + j);
                    string dataInsertionResult = otherUtil.CallStatic<string>("SendDataToMasterApp",
                        activitiyContext,
                        dataEntries[i].AttemptedOn,
                        dataEntries[i].Correctness,
                        dataEntries[i].hintUsed,
                        language, dataEntries[i].LevelID,
                        dataEntries[i].UserScore,
                        userID,
                        dataEntries[i].QuestionID,
                        userName,
                        topicID,
                        packageName,
                        dataEntries[i].UserAttempts,
                        mode,
                        UUID,
                        dataEntries[i].timeStamp,
                        dataEntries[i].skipped);
                    Debug.Log("SendingDB" + dataInsertionResult);

                    int resultID = int.Parse(dataInsertionResult);

                    if (lastId == -1 && resultID != -1)
                    {
                        lastId = resultID;
                    }
                    else if (lastId == -1 && resultID == -1)
                    {
                        masterAppDatabaseNotAcceptingData = true;
                    }
                    else if (resultID == -1)
                    {
                        masterAppDatabaseNotAcceptingData = true;
                        otherUtil.CallStatic<int>("RemoveDataFromMasterApp", activitiyContext, lastId - 1);
                    }
                }

            }
            //#endif
            dataEntries.Clear();
        }






        public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
        {

            return table.Cast<DictionaryEntry>().ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value);

        }





        public string CurrentDateTime()
        {

            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:") + DateTime.UtcNow.Millisecond.ToString();

        }

        public void ClearDataEntries()
        {

            Debug.Log("Cleared the data entries");
            dataEntries = new List<MyDataEntry>();

        }

        public List<MyDataEntry> LoadData(string levelSceneName)
        {

            if (File.Exists(Application.persistentDataPath + "/" + levelSceneName + ".dat"))
            {

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + levelSceneName + ".dat", FileMode.Open);

                LevelDataEntries currentLevelDataEntries = (LevelDataEntries)bf.Deserialize(file);
                file.Close();

                return currentLevelDataEntries.dataEntries;

            }
            else
            {

                return null;

            }

        }

        public void SaveData(string levelSceneName, List<MyDataEntry> dataEntries)
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + levelSceneName + ".dat");

            LevelDataEntries currentLevelDataEntries = new LevelDataEntries();
            currentLevelDataEntries.dataEntries = dataEntries;

            bf.Serialize(file, currentLevelDataEntries);
            file.Close();

        }

        public bool CheckIfAllLevelsDataExist()
        {

            for (int i = 0; i < levelsData.Count; i++)
            {

                if (!File.Exists(Application.persistentDataPath + "/" + levelsData[i].levelSceneName + ".dat"))
                    return false;

            }

            return true;

        }

        public void CollectDataFromFilesToDataEntries()
        {

            for (int i = 0; i < levelsData.Count; i++)
            {

                List<MyDataEntry> levelDataEntries = LoadData(levelsData[i].levelSceneName);

                for (int j = 0; j < levelDataEntries.Count; j++)
                    dataEntries.Add(levelDataEntries[j]);

            }

        }

        public void DeleteDataFiles()
        {

            for (int i = 0; i < levelsData.Count; i++)
            {

                if (File.Exists(Application.persistentDataPath + "/" + levelsData[i].levelSceneName + ".dat"))
                    File.Delete(Application.persistentDataPath + "/" + levelsData[i].levelSceneName + ".dat");

            }

        }

        public int CheckForRedundancy(string questionId, string levelId)
        {

            for (int i = 0; i < dataEntries.Count; i++)
            {

                if (dataEntries[i].QuestionID == questionId && dataEntries[i].LevelID == levelId)
                {

                    int dataEntryIndex = i;
                    return dataEntryIndex;

                }

            }

            return -1;

        }



        public void AddEntry(string LevelID, string QuestionID, int UserAttempts, int UserScore, bool Correctness, bool hintUsed, bool skipped = false)
        {
            print("Add Entry " + " " + LevelID + " " + QuestionID + " " + UserAttempts + " " + UserScore + " " + Correctness + " " + hintUsed + " " + " " + " " + skipped);
            string timeStamp = CurrentDateTime();
            QuestionID = topicID + LevelID + QuestionID;

            if (LevelID == null || LevelID == "")
            {

                Debug.LogError("You have not entered the Level Id in the data entry");
                return;

            }

            if (QuestionID == null || QuestionID == "")
            {

                Debug.LogError("You have not entered the Question Id in the data entry");
                return;

            }

            if (UserAttempts == 0)
            {

                Debug.LogError("You have not entered the User Attempts in the data entry");
                return;

            }

            if (CheckForRedundancy(QuestionID, LevelID) == -1)
            {
                dataEntries.Add(new MyDataEntry(LevelID, QuestionID, UserAttempts, UserScore, Correctness, hintUsed, timeStamp, skipped));

            }
            else
            {

                ReplaceDataEntry(LevelID, QuestionID, UserAttempts, UserScore, Correctness, hintUsed, timeStamp, skipped);

            }

            if (AllQuestionsAttempted())
            {
                Debug.Log("ALL Question DOne");
                InsertFinalDataToDatabase();
            }

        }



        public void ReplaceDataEntry(string LevelID, string QuestionID, int UserAttempts, int UserScore, bool Correctness, bool hintUsed, string timeStamp, bool skipped)
        {
            int dataEntryIndex = CheckForRedundancy(QuestionID, LevelID);
            dataEntries[dataEntryIndex].LevelID = LevelID;
            dataEntries[dataEntryIndex].QuestionID = QuestionID;
            dataEntries[dataEntryIndex].UserAttempts = UserAttempts;
            dataEntries[dataEntryIndex].UserScore = UserScore;
            dataEntries[dataEntryIndex].AttemptedOn = CurrentDateTime();
            dataEntries[dataEntryIndex].Correctness = Correctness.ToString();
            dataEntries[dataEntryIndex].hintUsed = hintUsed.ToString();
            dataEntries[dataEntryIndex].timeStamp = timeStamp;
            dataEntries[dataEntryIndex].skipped = skipped.ToString();
        }


        bool AllQuestionsAttempted()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            foreach (var item in levelsData)
            {
                if (item.levelSceneName.Equals(currentScene))
                {
                    if (dataEntries.Count == item.noOfQuestions)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }




    }

    [System.Serializable]
    public class LevelData
    {

        public string levelSceneName;
        public int noOfQuestions;

    }

    [System.Serializable]
    public class LevelDataEntries
    {

        public List<MyDataEntry> dataEntries;

    }

}