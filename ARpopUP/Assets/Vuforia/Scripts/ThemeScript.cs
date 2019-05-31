using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ThemeScript : MonoBehaviour
{
    public GUISkin skin;
    public GUISkin labelSkin;
    public GUISkin numberSkin;
    public GUISkin animalSkin;
    public GUISkin unavailableSkin;
    public static string theme = "";
    public static string[] themeList = { "Number","Color","Animal","Vehicle","Food","School","Person","Sport","Job","Etc" };
    public static string[] contentsList = { "Number contents", "Color contents", "Animal contents", "Vehicle contents", "Food contents", "School contents", "Person contents", "Sport contents", "Job contents", "Etc contents" };
    public static int themeIdx = 0;
    bool flag = false;
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
        GUI.skin = skin;
        int height = Screen.height;
        int width = Screen.width;
        int xValue = width / 12;
        int yValue = height / 21;
        string msg = "<color=#F2A63A><size=110>Select the Theme</size></color>";
        GUI.Label(new Rect((int)(xValue*0.7),yValue*2,xValue*6,(int)(yValue*3)), msg);

        for (int i = 0; i < 10;i++){
            if (i % 2 == 0)
            {
                if (GUI.Button(new Rect((int)(xValue*6.5), yValue * (2*i+2), (int)(xValue*4), yValue*2), "<size=50>"+themeList[i]+"</size>"))
                {
                    theme = themeList[i];
                    themeIdx = i;
                    flag = true;
                }
            }

            //else{
            //    if (GUI.Button(new Rect((int)(xValue * 9), yValue * (2*(i-1)+2), (int)(xValue*2), yValue*2), "<size=50>" + themeList[i] + "</size>"))
            //    {
            //        themeIdx = i;
            //        theme = themeList[i];
            //        flag = true;
            //    }
            //}
        }

        GUI.skin = labelSkin;

        if(flag){
            GUI.Label(new Rect((int)(xValue * 0.5), yValue * 6, xValue*5,yValue*13),"");
            if (themeIdx == 0)
            {
                GUI.skin = numberSkin;
                GUI.Label(new Rect((int)(xValue * 0.5), yValue * 6, xValue * 5, yValue * 13), "");
            } else if(themeIdx==2){
                GUI.skin = animalSkin;
                GUI.Label(new Rect((int)(xValue * 1), (int)(yValue * 7), (int)(xValue * 3.7), yValue * 8), "");
            }
            GUI.skin = skin;
            if(GUI.Button(new Rect((int)(xValue*4),(int)(yValue*17),xValue,yValue),"<size=30>Enter</size>")){
                if (TestButton.mode == 1)
                {
                    SceneManager.LoadScene("testMode");
                }
                else
                {
                    SceneManager.LoadScene("quizMode");
                }
            }
        }
    }
}
