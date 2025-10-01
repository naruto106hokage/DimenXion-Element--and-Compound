using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UmetyDatabase
{
	
	[ System.Serializable ]
	public class  DatabaseQuestionsData
	{
		
		public string QuestionID;
		public string QuestionName;
		public List < string > QuestionOptions;
		public int CorrectOptionIndex;

	}

	public class DBQuestionHandler : MonoBehaviour
	{
		public static DBQuestionHandler Instance;
		public List < DatabaseQuestionsData > DBQuestionData;

		void Awake ( )
		{
			
			Instance = this;

		}

		public void OnCorrectAnswer ( string LevelID , string QuestionID , int UserAttempts , int UserScore , bool Correctness ) {

//			int currentQuestionIndex = 0;

//			for ( int i = 0; i < DBQuestionData.Count; i++ ) {
//				
//				if ( DBQuestionData [ i ].QuestionID == QuestionID ) {
//					
//					currentQuestionIndex = i;
//
//				} else {
//					
//					Debug.Log ( "There is no such question with the question id" + QuestionID );
//					return;
//
//				}
//
//			}

			DatabaseManager.dbm.AddEntry ( LevelID , QuestionID , UserAttempts , UserScore , true , false );

		}

	}

}