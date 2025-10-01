
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustMenu : MonoBehaviour
{
    [Range(1,6)]
    public int NumberOfChild = 1;
    public bool isVT;
    public bool isWithoutAS;
    private GameObject defaultRow;
    private GameObject defaultChild;

    private GameObject duplicateLevel;
    private GameObject duplicateRow;

    private GameObject LO;
    private GameObject AS;

    private HorizontalLayoutGroup horizontalLayoutGroup;


    public void SetLayOut()
    {
        ReSetLayOut();
    }

    private void ReSetLayOut()
    {
        LO = transform.parent.GetChild(transform.GetSiblingIndex() - 1).gameObject;
        AS = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;
        LO.SetActive(true);
        AS.SetActive(true);
        defaultRow = transform.GetChild(0).gameObject;
        defaultChild = transform.GetChild(0).GetChild(0).gameObject;

        for (int i = transform.GetChild(0).childCount - 1; i > 0; i--)
        {
            if (transform.GetChild(0).childCount > 1)
            {
                DestroyImmediate(transform.GetChild(0).GetChild(i).gameObject);
            }
        }

        for (int i = transform.childCount - 1; i > 0; i--)
        {
            if (transform.childCount > 1)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }


        transform.GetChild(0).transform.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
        transform.GetChild(0).transform.GetComponent<HorizontalLayoutGroup>().childControlHeight = false;
        transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 160f);
        GenerateLayout();

    }

    private void GenerateLayout()
    {
        if (NumberOfChild == 2 || NumberOfChild == 3)
        {
            for (int i = 0; i < (NumberOfChild - 1); i++)
            {
                duplicateLevel = Instantiate(defaultChild);
                duplicateLevel.gameObject.name = "Level" + (i + 2);
                duplicateLevel.transform.SetParent(defaultRow.transform);
                duplicateLevel.transform.localScale = Vector3.one;
                duplicateLevel.transform.localPosition = new Vector3(duplicateLevel.transform.localPosition.x, duplicateLevel.transform.localPosition.y, 0);
            }
            defaultRow.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
        }
        else if (NumberOfChild > 3)
        {
            for (int i = 0; i < (NumberOfChild / 3); i++)
            {
                duplicateLevel = Instantiate(defaultChild);
                duplicateLevel.gameObject.name = "Level2";
                duplicateLevel.transform.SetParent(defaultRow.transform);
                duplicateLevel.transform.localScale = Vector3.one;
                duplicateLevel.transform.localPosition = new Vector3(duplicateLevel.transform.localPosition.x, duplicateLevel.transform.localPosition.y, 0);

            }

            defaultRow.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
            defaultRow.GetComponent<HorizontalLayoutGroup>().childControlHeight = true;

            duplicateRow = Instantiate(defaultRow);
            duplicateRow.gameObject.name = "Row2";
            duplicateRow.transform.SetParent(this.gameObject.transform);
            duplicateRow.transform.localScale = Vector3.one;
            duplicateRow.transform.localPosition = new Vector3(duplicateRow.transform.localPosition.x, duplicateRow.transform.localPosition.y, 0);

            if (NumberOfChild == 5)
            {
                duplicateLevel = Instantiate(defaultChild);
                duplicateLevel.gameObject.name = "Level3";
                duplicateLevel.transform.SetParent(defaultRow.transform);
                duplicateLevel.transform.localScale = Vector3.one;
                duplicateLevel.transform.localPosition = new Vector3(duplicateLevel.transform.localPosition.x, duplicateLevel.transform.localPosition.y, 0);
            }


        }
        if (isVT)
        {
            LO.SetActive(false);
            AS.SetActive(false);
        }
        if (isWithoutAS)
        {
            AS.SetActive(false);
        }
    }


}
