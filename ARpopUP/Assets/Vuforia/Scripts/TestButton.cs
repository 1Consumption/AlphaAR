using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    public GUISkin skin;
    public static int mode;
    // Use this for initialization
    void Start()
    {
        mode = 0;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        Application.Quit();
    //    }

    //}

    uint exitCountValue = 0;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            exitCountValue++;
            if (!IsInvoking("disable_DoubleClick"))
                Invoke("disable_DoubleClick", 0.3f);
        }
        if (exitCountValue == 2)
        {
            CancelInvoke("disable_DoubleClick");
            Application.Quit();
        }
    }

    void disable_DoubleClick()
    {
        exitCountValue = 0;
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        int height = Screen.height;
        int width = Screen.width;
        int xValue = width / 4;
        int yValue = height / 9;
        string learn = "<size=50>Learning Mode</size>";
        string quiz = "<size=50>Quiz Mode</size>";
        string quit = "<size=50>Quit</size>";
        if (GUI.Button(new Rect(width / 2 - xValue / 2, height / 2 - yValue / 2, xValue, yValue), learn))
        {
            mode = 1;
            SceneManager.LoadScene("theme");
        }

        if (GUI.Button(new Rect(width / 2 - xValue / 2, height / 2 + yValue, xValue, yValue), quiz))
        {
            mode = 2;
            SceneManager.LoadScene("theme");
        }

        if (GUI.Button(new Rect(width / 2 - xValue / 2, height / 2 + (int)(yValue*2.5), xValue, yValue), quit))
        {
            Application.Quit();
        }
    }

}
