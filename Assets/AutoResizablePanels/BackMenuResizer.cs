using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BackMenuResizer : MonoBehaviour
{
    Transform _Menu, _QuitMenu;
    float w,h;

    void Awake()
    {
        _Menu = transform.Find("Menu");
        _QuitMenu = transform.Find("QuitMenu");
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
        ResizeBg();
        #endif
    }

    void ResizeBg()
    {
        w = 0;
        h = 0;
        RectTransform rt;
        for (int i = 0; i < _Menu.childCount; i++)
        {
            float temp = _Menu.GetChild(i).GetChild(0).GetComponent<Text>().preferredWidth;
            w = (w > temp) ? w : temp;
        }

        for (int j = 0; j < _Menu.childCount; j++)
        {
            rt = _Menu.GetChild(j).GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(w+60, rt.sizeDelta.y);
        }

        rt = _Menu.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(w+620,rt.sizeDelta.y);

        // QuitMenu Start

        float x1 = 0;
        x1 = _QuitMenu.GetChild(0).GetChild(0).GetComponent<Text>().preferredWidth;

        _QuitMenu.GetChild(1).GetComponent<RectTransform>().pivot = new Vector2(1,0.5f);
        _QuitMenu.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-25,_QuitMenu.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y);
        _QuitMenu.GetChild(2).GetComponent<RectTransform>().pivot = new Vector2(0,0.5f);
        _QuitMenu.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(25,_QuitMenu.GetChild(2).GetComponent<RectTransform>().anchoredPosition.y);

        float x2 = 0;
        for (int i = 1; i < _QuitMenu.childCount; i++)
        {
            float temp = _QuitMenu.GetChild(i).GetChild(0).GetComponent<Text>().preferredWidth;
            x2 = (x2 > temp) ? x2 : temp;
        }

        for (int j = 1; j < _QuitMenu.childCount; j++)
        {
            rt = _QuitMenu.GetChild(j).GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(x2+175, rt.sizeDelta.y);
        }

        float x3 = (x1 > (x2+175 + x2+175 + 50)) ? x1 : (x2+175 + x2+175 + 50);
        rt = _QuitMenu.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(x3+300,rt.sizeDelta.y);
    }
}
