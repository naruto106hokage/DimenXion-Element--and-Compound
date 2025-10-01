using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Parameter
{
	public string parameterName;
	public string parameterValue;
}

public class VoiceAssistanceManager : MonoBehaviour
{


	public string currentResponseText;

	public TypeOfVoiceActions currentVoiceAction;

	public List<Parameter> currentActionParams = new List<Parameter> ();


		
}
