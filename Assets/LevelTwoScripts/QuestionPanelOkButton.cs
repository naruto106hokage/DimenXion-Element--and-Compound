using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuestionPanelOkButton : MonoBehaviour
{


	public void QuestionOneOkButton ()
	{
		GameManagerTwo.instance.tableScreen.transform.DOLocalMoveZ (.58f, 2f);
		GameManagerTwo.instance.anim.Play ("zoomClip_d 1");
		AllCouroutineL2.instance.StartCoroutine ("AfterMetalPlacedOnTableOne");
	}

	public void QuestionTwoOkButton ()
	{
		GameManagerTwo.instance.details [3].SetActive (true);
		GameManagerTwo.instance.instructions [3].SetActive (true);
	}

	public void ScreenComeFront ()
	{
		GameManagerTwo.instance.details [6].SetActive (true);
		GameManagerTwo.instance.tableScreen.transform.DOLocalMoveX (0.959f, 2f);
	}

	public void ScreenGoBack ()
	{
		SoundManager.instance.PlayAudioClip (SfxPlayVoice.spv.audioclip [17]);
		GameManagerTwo.instance.tableScreen.transform.DOLocalMoveX (0.48f, 2f).OnComplete (ShowActivityCompletePanel);
	}

	void ShowActivityCompletePanel ()
	{
		GameManagerTwo.instance.instructions [8].SetActive (true);
	}
}
