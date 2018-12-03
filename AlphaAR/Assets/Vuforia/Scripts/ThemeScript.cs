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
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("Intro");
            }
        }
    }

    private void OnGUI()
    {
        int height = Screen.height;
        int width = Screen.width;
        int xValue = width / 7;
        int yValue = height / 21;
        for (int i = 0; i < 10;i++){
            if (i % 2 == 0)
            {
                if (GUI.Button(new Rect(xValue, yValue * (2*i+2), xValue*2, yValue*2), "<size=50>"+themeList[i]+"</size>"))
                {
                    theme = themeList[i];
                    themeIdx = i;
                    SceneManager.LoadScene("testMode");
                }
            }else{
                if (GUI.Button(new Rect(xValue * 4, yValue * (2*(i-1)+2), xValue*2, yValue*2), "<size=50>" + themeList[i] + "</size>"))
                {
                    themeIdx = i;
                    theme = themeList[i];
                    SceneManager.LoadScene("testMode");
                }
            }
        }
    }
}
