using UnityEngine;
using System.Collections;

public class ThemeScript : MonoBehaviour
{
    public static string theme = "";
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        int height = Screen.height;
        int width = Screen.width;
        int xValue = width / 7;
        int yValue = height / 20;
        for (int i = 0; i < 10;i++){
            if (i % 2 == 0)
            {
                if (GUI.Button(new Rect(xValue * 2, yValue * (i + 3), xValue, yValue*2), "<size=50>test</size>"))
                {
                    Application.LoadLevel("SampleScene");
                }
            }else{
                if (GUI.Button(new Rect(xValue * 4, yValue * (i+2), xValue, yValue*2), "<size=50>test</size>"))
                {
                    Application.LoadLevel("SampleScene");
                }
            }
        }
    }
}
