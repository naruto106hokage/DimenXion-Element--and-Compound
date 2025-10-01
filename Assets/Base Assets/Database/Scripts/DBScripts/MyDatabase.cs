using System.Collections.Generic;

// For JSON purpose
using UnityEngine;


[ System.Serializable]
public class MyDatabase
{

	public List < MyDataEntry > myDataEntries;

}

[ System.Serializable]
public class MyDataEntry
{

	[HideInInspector]
	public int ActivityID;
	[HideInInspector]
	public int UserID;
	[HideInInspector]
	public string UserName;
	[HideInInspector]
	public string PackageID;
	[HideInInspector]
	public string PackageName;
	public string LevelID;
	public string QuestionID;
	public string Correctness;
	public string hintUsed;
	public int UserAttempts;
	public int UserScore;
	[HideInInspector]
	public string Language;
	public string AttemptedOn;
	[HideInInspector]
	public int mode;
    [HideInInspector]
    public string timeStamp;
    [HideInInspector]
    public string skipped;


	// For JSON purpose
	public MyDataEntry (int ActivityID, int UserID, string UserName, string PackageID, string PackageName, string LevelID, string QuestionID,
		string Correctness, string hintUsed , int UserAttempts, int UserScore, string Language, string AttemptedOn , int mode)
	{

		this.ActivityID = ActivityID;
		this.UserID = UserID;
		this.UserName = UserName;
		this.PackageID = PackageID;
		this.PackageName = PackageName;
		this.LevelID = LevelID;
		this.QuestionID = QuestionID;
		this.Correctness = Correctness;
		this.hintUsed = hintUsed;
		this.UserAttempts = UserAttempts;
		this.UserScore = UserScore;
		this.Language = Language;
		this.AttemptedOn = AttemptedOn;
		this.mode = mode;

	}

//	// For Question Data Entry purpose
//	public MyDataEntry (string QuestionID, string QuestionName, string Option_1, string Option_2, string Option_3, string Option_4, string UserResponse, string CorrectResponse, int UserAttempts, int UserScore, bool Correctness, bool hintUsed )
//	{
//
//		this.QuestionID = QuestionID;
//		this.QuestionName = QuestionName;
//		this.Option_1 = Option_1;
//		this.Option_2 = Option_2;
//		this.Option_3 = Option_3;
//		this.Option_4 = Option_4;
//		this.UserResponse = UserResponse;
//		this.CorrectResponse = CorrectResponse;
//		this.UserAttempts = UserAttempts;
//		this.UserScore = UserScore;
//		this.AttemptedOn = UmetyDataBase.DatabaseManager.dbm.CurrentDateTime ( );
//		this.Correctness = Correctness.ToString ( );
//		this.hintUsed = hintUsed.ToString ( );
//
//	}

	// For Question Data Entry purpose
    public MyDataEntry ( string LevelID, string QuestionID, int UserAttempts, int UserScore, bool Correctness, bool hintUsed, string timeStamp, bool skipped)
	{
		this.LevelID = LevelID;
		this.QuestionID = QuestionID;
		this.UserAttempts = UserAttempts;
		this.UserScore = UserScore;
		this.AttemptedOn = UmetyDatabase.DatabaseManager.dbm.CurrentDateTime ( );
		this.Correctness = Correctness.ToString ( );
		this.hintUsed = hintUsed.ToString ( );
        this.timeStamp = timeStamp;
        this.skipped = skipped.ToString();
	}

}