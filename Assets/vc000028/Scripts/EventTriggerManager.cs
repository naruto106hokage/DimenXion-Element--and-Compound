using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventTriggerManager: MonoBehaviour
{
	public static EventTriggerManager instance;

	void Awake ()
	{
		instance = this;
//		EventTrigger et = GetComponent<EventTrigger> ();
//		SetEventTriggerState (et, EventTriggerType.PointerUp, "Up", UnityEventCallState.EditorAndRuntime);
//		SetEventTriggerState (et, EventTriggerType.PointerClick, "Click", UnityEventCallState.RuntimeOnly);
	}

	public void SetEventTriggerState (EventTrigger ET, EventTriggerType ETType, string MethodName, UnityEventCallState NewState)
	{
		for (int i = 0; i < ET.triggers.Count; i++) {
			EventTrigger.Entry Trigger = ET.triggers [i];

			EventTrigger.TriggerEvent CB = Trigger.callback;

			for (int j = 0; j < CB.GetPersistentEventCount (); j++) {
				
				if (CB.GetPersistentMethodName (j) == MethodName && Trigger.eventID == ETType) {
					
					CB.SetPersistentListenerState (j, NewState);
				}
			}
		}
	}



}