using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ThemeScript : MonoBehaviour
{
    public static string theme = "";
    public static string[] themeList = { "Number","Color","Animal","Vehicle","Food","School","Person","Sport","Job","Etc" };
    public static int themeIdx = 0;
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
        string returnMenu = "<size=30>RETURN TO MENU</size>";
        if (GUI.Button(new Rect(10, 10, 300, 80), returnMenu))
        {
            Application.LoadLevel("Intro");
        }

        int height = Screen.height;
        int width = Screen.width;
        int xValue = width / 7;
        int yValue = height / 20;
        for (int i = 0; i < 10;i++){
            if (i % 2 == 0)
            {
                if (GUI.Button(new Rect(xValue * 2, yValue * (i + 3), xValue, yValue*2), "<size=50>"+themeList[i]+"</size>"))
                {
                    theme = themeList[i];
                    themeIdx = i;
                    SceneManager.LoadScene("testMode");
                }
            }else{
                if (GUI.Button(new Rect(xValue * 4, yValue * (i+2), xValue, yValue*2), "<size=50>" + themeList[i] + "</size>"))
                {
                    themeIdx = i;
                    theme = themeList[i];
                    SceneManager.LoadScene("testMode");
                }
            }
        }
    }
}
