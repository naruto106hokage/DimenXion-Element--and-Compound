using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum PanelType
{
NormalPanel,
Panel_with_OK,
    QuestionPanel
}
;

[ExecuteInEditMode]
public class AutomaticPanelResizer : MonoBehaviour
{
    public static AutomaticPanelResizer instance;
    public PanelType panelType;

    [Header("Margins")]
    public float _marginLeft;
    public float _marginTop;
    public float _marginRight;
    public float _marginBottom;

    [Header("Limit Size")]
    public float _minWidth;
    public float _maxWidth;
    public float _minHeight;
    public float _maxHeight;

    [Header("Elements")]
    public float _space = 10;

    List<Text> txt;
    List<RectTransform> textRect;
    RectTransform selfRect;

    float width, height;

    void Awake()
    {
        selfRect = GetComponent<RectTransform>();
        txt = new List<Text>();
        textRect = new List<RectTransform>();

        if (_maxWidth < _minWidth && _maxWidth != 0)
            _maxWidth = _minWidth;

        if (_maxHeight < _minHeight && _maxHeight != 0)
            _maxHeight = _minHeight;
        		
        for (int i = 0; i < transform.childCount; i++)
        {
            Text t = transform.GetChild(i).GetComponent<Text>();
            if (t != null)
            {
                txt.Add(t);
                textRect.Add(t.GetComponent<RectTransform>());

                textRect[i].anchorMin = new Vector2(0.5f, 1f);
                textRect[i].anchorMax = new Vector2(0.5f, 1f);
                textRect[i].pivot = new Vector2(0.5f, 1f);
            }
        }
    }

    void OnEnable()
    {
        LanguageHandler.LanguageChangeEventFire += ResizeBg;
    }

    void OnDisable()
    {
        LanguageHandler.LanguageChangeEventFire -= ResizeBg;
    }

    void Update()
    {
        #if UNITY_EDITOR
        Awake();
        ResizeBg();
        #endif
    }

    void ResizeBg()
    {
        List<float> _height = new List<float>();
        List<float> _width = new List<float>();
        float p1 = 0;
        float p2 = 0;
        width = 0;
        height = 0;

        for (int i = 0; i < txt.Count; i++)
        {
            float w = 0;
            if (_maxWidth == 0)
            {
                if (txt[i].preferredWidth < _minWidth - _marginLeft - _marginRight)
                    w = _minWidth - _marginLeft - _marginRight;
                else
                    w = txt[i].preferredWidth;
            }
            else
            {
                if (txt[i].preferredWidth > _maxWidth - _marginLeft - _marginRight)
                    w = _maxWidth - _marginLeft - _marginRight;
                else
                {
                    if (txt[i].preferredWidth < _minWidth - _marginLeft - _marginRight)
                        w = _minWidth - _marginLeft - _marginRight;
                    else
                        w = txt[i].preferredWidth;
                }
            }

            textRect[i].sizeDelta = new Vector2(w, textRect[i].sizeDelta.y);
            textRect[i].sizeDelta = new Vector2(w, txt[i].preferredHeight);

            _width.Add(textRect[i].sizeDelta.x);
            _height.Add(textRect[i].sizeDelta.y);

            width = width < _width[i] ? _width[i] : width;
            height = height + _height[i];
        }

        if (panelType == PanelType.QuestionPanel)
        {
            for (int i = 0; i < txt.Count; i++)
            {
                textRect[i].sizeDelta = new Vector2(width, textRect[i].sizeDelta.y);
                textRect[i].sizeDelta = new Vector2(width, txt[i].preferredHeight);
            }
        }

        if (panelType == PanelType.Panel_with_OK || panelType == PanelType.QuestionPanel)
        {
            RectTransform BtnRect = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
            Text BtnTxt = BtnRect.GetComponentInChildren<Text>();
            RectTransform BtnTxtRect = BtnTxt.GetComponent<RectTransform>();

            BtnRect.anchorMin = new Vector2(0.5f, 0f);
            BtnRect.anchorMax = new Vector2(0.5f, 0f);
            BtnRect.pivot = new Vector2(0.5f, 0f);

            BtnTxtRect.anchorMin = new Vector2(0.5f, 0.5f);
            BtnTxtRect.anchorMax = new Vector2(0.5f, 0.5f);
            BtnTxtRect.pivot = new Vector2(0.5f, 0.5f);
            BtnTxtRect.anchoredPosition3D = new Vector3(0, 0, 0);

            BtnTxtRect.sizeDelta = new Vector2(BtnTxt.preferredWidth, BtnTxtRect.sizeDelta.y);
            BtnTxtRect.sizeDelta = new Vector2(BtnTxtRect.sizeDelta.x, BtnTxt.preferredHeight);

            BtnRect.sizeDelta = new Vector2(BtnTxtRect.sizeDelta.x+60, BtnTxtRect.sizeDelta.y+20);

            height = height + BtnRect.sizeDelta.y+(_space*1.5f);
        }

        float h = height + _marginTop + _marginBottom + _space * (txt.Count - 1);
        if (_maxHeight != 0)
        {
            h = Mathf.Clamp(h, _minHeight, _maxHeight);
        }
        else
        {
            if (h < _minHeight)
                h = _minHeight;
        }
        selfRect.sizeDelta = new Vector2(width + _marginLeft + _marginRight, h);

        for (int i = 0; i < txt.Count; i++)
        { 
            if (i == 0)
            {
                p2 = p2 + _marginTop;
            }
            else
            {
                p2 = p2 + _height[i - 1] + _space;
            }

            p1 = (_marginRight - _marginLeft) / 2;
            textRect[i].anchoredPosition3D = new Vector3(-p1, -p2, 0);
        }

        if (panelType == PanelType.Panel_with_OK || panelType == PanelType.QuestionPanel)
        {
            RectTransform BtnRect = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
            BtnRect.anchoredPosition3D = new Vector3(0, _marginBottom, 0);
        }
    }
}