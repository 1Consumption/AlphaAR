using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quizResult : MonoBehaviour
{
    GameObject pos;

    // Use this for initialization
    void Start()
    {
        pos = GameObject.Find("Sphere");
        HideResult();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowRight()
    {
        for (int i = 0; i < pos.transform.childCount; i++)
        {
            Transform child = pos.transform.GetChild(i);
            if (child.tag == "RIGHT")
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    public void ShowWrong()
    {
        for (int i = 0; i < pos.transform.childCount; i++)
        {
            Transform child = pos.transform.GetChild(i);
            if (child.tag == "WRONG")
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    public void HideResult()
    {
        for (int i = 0; i < pos.transform.childCount; i++)
        {
            Transform child = pos.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }
}