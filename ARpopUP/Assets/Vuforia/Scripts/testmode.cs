using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class testmode : MonoBehaviour
{
    public Button btn;
    private static bool s3DModel = false;
    private static bool sloss = false;

    public string SelectedTheme;
    private static int quizI = 0;
    public Slider quizGage;
    public GameObject quizPosImg;
    public GameObject quizResPos;
    public int[] idx;
    GameObject sound;
    GameObject childObject = null;

    public bool isDone = false;
    public GameObject quizEndScreen;
    public Text scoreText;
    int score = 0;

    public static string trackableMsg; //current tracked card string
    public string lastShowObj = null;

    public static SortedList<string, GameObject> modelObj = new SortedList<string, GameObject>(); //match with the string and model
    GameObject[] models;
    GameObject[] imgTargets;

    GameObject canvas;
    bool startFlag = false;
    public GUISkin textSkin;

    // Use this for initialization
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        quizEndScreen.SetActive(false);
        btn.onClick.AddListener(onBtn1Clicked);
        SelectedTheme = ThemeScript.theme;
        sound = GameObject.FindGameObjectWithTag(ThemeScript.theme + "Sound");

        models = GameObject.FindGameObjectsWithTag(SelectedTheme);
        imgTargets = GameObject.FindGameObjectsWithTag("Alphabet");

        for (int i = 0; i < models.Length; i++)
        {
            modelObj.Add(models[i].name, models[i]);
            models[i].SetActive(false);
        }

        idx = getRandomIntArr(modelObj.Count, 0, modelObj.Count);
        isDone = false;

        //quizGage.value = quizI + 1;
        //Quiz(modelObj.Values[idx[quizI]]);
    }


    public void Quiz(GameObject model)
    {
        Debug.Log("*********QUIZ: "+model.name+"******");
        
        for (int i = 0; i < sound.transform.childCount; i++)
        {
            childObject = sound.transform.GetChild(i).gameObject;
            if (childObject.name.Equals(model.name))
            {
                childObject.GetComponent<AudioSource>().Play();
                break;
            }
        }

        model.transform.parent = quizPosImg.transform;
        model.transform.localPosition = new Vector3(0, 0, 0);
        model.transform.localRotation = Quaternion.Euler(0, 180, 0);
        if (SelectedTheme == "Food")
        {
            if (model.name == "PIZZA")
            {
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            model.transform.localScale = new Vector3(6, 6, 6);
        }
        else if (model.name == "CHICKEN")
        {
            model.transform.localScale = new Vector3(4, 4, 4);
        }
        else if (model.name == "FROG")
        {
            model.transform.localScale = new Vector3(8, 8, 8);
        }
        else
        {
            model.transform.localScale = new Vector3(2, 2, 2);
        }
        model.SetActive(true);
    }
    public int[] getRandomIntArr(int length, int min, int max)
    {
        int[] randArr = new int[length];
        bool isDiffer;

        // randArr[0] = 0; //to start with the first obj
        for (int i = 0; i < length; i++)
        //for (int i = 1; i < length; i++)
        {
            do
            {
                randArr[i] = UnityEngine.Random.Range(min, max);
                isDiffer = true;
                for (int j = 0; j < i; j++)
                {
                    if (randArr[j] == randArr[i])
                    {
                        isDiffer = false;
                        break;
                    }
                }
            } while (!isDiffer);
        }
        return randArr;
    }
    bool modelSwap(GameObject obj)
    {
        Animator anim = obj.GetComponent<Animator>();

        trackableMsg = DefaultTrackableEventHandler.ChkMsg();
        if (trackableMsg != null && modelObj.Values[idx[quizI]].name.Equals(trackableMsg))
        {
            Debug.Log("YES :" + trackableMsg);
            lastShowObj = trackableMsg;
            sloss = false;

            for (int i = 0; i < imgTargets.Length; i++)
            {
                imgTargets[i].GetComponent<ImageFlag>().setAlphabetChildFalse();
            }
            GameObject cardInMiddle = GameObject.Find(DefaultTrackableEventHandler.getTargetinMiddleString());

            obj.transform.parent = cardInMiddle.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (obj.name == "chicken")
            {
                obj.transform.localScale = new Vector3(2, 2, 2);
            }
            else if (obj.name == "frog")
            {
                obj.transform.localScale = new Vector3(4, 4, 4);
            }
            else
            {
                obj.transform.localScale = new Vector3(1, 1, 1);
            }
            obj.SetActive(true);

            DefaultTrackableEventHandler.setStateLoss(false);
            anim.SetInteger("action", 1);
            quizResPos.GetComponent<quizResult>().ShowRight();
            GameObject.Find("sndRight").GetComponent<AudioSource>().Play();
            score += 10;
            s3DModel = true;
        }
        else //No matching result
        {
            Debug.Log("NO :" + trackableMsg);
            s3DModel = false;
            anim.SetInteger("action", 2);
            quizResPos.GetComponent<quizResult>().ShowWrong();
            GameObject.Find("sndWrong").GetComponent<AudioSource>().Play();
        }
        StartCoroutine("test");

        return s3DModel;
    }
    IEnumerator test()
    {
        Debug.Log("3초 후에");
        yield return new WaitForSeconds(3);
        Debug.Log("3초 지남");

        quizResPos.GetComponent<quizResult>().HideResult();
        if (s3DModel)
        {
            getSwapBack();
        }
        else
        {
            Transform[] childList = quizPosImg.GetComponentsInChildren<Transform>(true);
            if (childList != null)
            {
                for (int i = 0; i < childList.Length; i++)
                {
                    if (childList[i].tag == SelectedTheme)
                    {
                        Destroy(childList[i].gameObject);
                    }
                }
            }
        }

        quizI++;
        if (quizI < modelObj.Count)
        {
            quizGage.value = quizI + 1;
            Quiz(modelObj.Values[idx[quizI]]);
        }
        else
        {
            isDone = true;
        }
    }
    private void getSwapBack()
    {
        if (lastShowObj != null)
        {
            Debug.Log("###########" + lastShowObj + "Lost");
            GameObject sub;
            if ((sub = GameObject.Find(lastShowObj)) != null)
            {
                Destroy(sub);
            };

            s3DModel = false;
            for (int i = 0; i < imgTargets.Length; i++)
            {
                imgTargets[i].GetComponent<ImageFlag>().setAlphabetChildTrue();
            }
        }
    }
    void onBtn1Clicked()
    {
        Debug.Log("pressed btn");

        if (quizI <= modelObj.Count)
        {

            Transform[] childList = quizPosImg.GetComponentsInChildren<Transform>(true);
            if (childList != null)
            {
                for (int i = 0; i < childList.Length; i++)
                {
                    if (childList[i].tag == SelectedTheme)
                    {
                        modelSwap(childList[i].gameObject);
                    }
                }
            }
        }
        else
        {
            Debug.Log("END QUIZ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        sloss = DefaultTrackableEventHandler.getStateLoss();
        if (sloss && s3DModel)
        {
            getSwapBack();
            quizGage.value = quizI + 1;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("Intro");
            }
        }
    }

    void OnGUI()
    {
        int height = Screen.height / 10;
        int width = Screen.width / 20;

        if (startFlag == false)
        {
            GUI.skin = textSkin;
            string start = "<size=200>Start</size>";
            if (GUI.Button(new Rect(width * 7, (int)(height * 4), width * 6, (int)(height * 2)), start))
            {
                startFlag = true;
                GameObject.Find("CanvasStart").SetActive(false);
                canvas.SetActive(true);
                
                quizGage.value = quizI + 1;
                Quiz(modelObj.Values[idx[quizI]]);
            }
        }
        if (!isDone)
        {
            quizEndScreen.SetActive(false);
        }
        else
        {
            quizEndScreen.SetActive(true);
            scoreText.text = "" + score;
        }
    }
}