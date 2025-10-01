using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(LocaliseTextAndVoiceOver))]
public class RandomExclamation : MonoBehaviour
{
    public int MaxAttemptCount = 3;
    static int AttemptCount;

    public string Tkey1{ get { return "C_TryAgain1"; } }

    public string Tkey2{ get { return "C_TryAgain2"; } }

    public string Tkey3{ get { return "C_TryAgain3"; } }

//    public string Wkey1{ get { return "C_WellDone1"; } }

    public string Wkey2{ get { return "C_WellDone2"; } }

    public string Wkey3{ get { return "C_WellDone3"; } }

    [HideInInspector]
    public bool _WellDone = true;
    Image _img;

    LocaliseTextAndVoiceOver _ltvo;

    void Awake()
    {
        _ltvo = GetComponent<LocaliseTextAndVoiceOver>();
        _img = GetComponent<Image>();
        AttemptCount = 0;
    }

    void OnEnable()
    {
        _ltvo.enabled = true;
        AttemptCount++;

        if (_WellDone)
        {
            AttemptCount = 0;
            switch (Random.Range(1, 3))
            {
//                case 0:
//                    _ltvo.gameObject.name = Wkey1;
//                    break;
                case 1:
                    _ltvo.gameObject.name = Wkey2;
                    break;
                case 2:
                    _ltvo.gameObject.name = Wkey3;
                    break;
            }
        }
        else
        {
            if (AttemptCount < MaxAttemptCount)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        _ltvo.gameObject.name = Tkey1;
                        break;
                    case 1:
                        _ltvo.gameObject.name = Tkey2;
                        break;
                    case 2:
                        _ltvo.gameObject.name = Tkey3;
                        break;
                }
            }
            else
            {
                AttemptCount = 0;
                switch (Random.Range(0, 2))
                {
                    case 0:
                        _ltvo.gameObject.name = Tkey2;
                        break;
                    case 1:
                        _ltvo.gameObject.name = Tkey3;
                        break;
                }
            }
        }

        _ltvo.OnLanguageChangeListener();
        _ltvo.PlayVoiceOver();

        if (_img != null)
        {
            if (_img.enabled)
                _img.rectTransform.sizeDelta = new Vector2(GetComponentInChildren<Text>().preferredWidth + 60, _img.rectTransform.sizeDelta.y);
        }
    }

    void OnDisable()
    {
        _ltvo.enabled = false;
    }
}
