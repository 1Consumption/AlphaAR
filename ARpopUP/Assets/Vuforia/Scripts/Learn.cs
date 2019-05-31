using UnityEngine;
using System.Collections;
using Vuforia;
using System;
using UnityEngine.SceneManagement;

public class Learn : MonoBehaviour
{
    public static bool rotateFlag = false;
    GameObject childObject = null;
    GameObject curObject = null;
    GameObject target;
    GameObject sound;
    GameObject swapObject = null;
    GameObject[] objects;
    SortedList mapped = new SortedList();
    String[,] objectList = { { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "GOAT", "CHICKEN", "BEAR", "COW", "FROG", "SHARK", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" } };

    String curMsg = "";
    String objectStr = "";
    String middleTargetName = "";
    int objectIdx = 0;

    bool startFlag = false;
    bool lossFlag = false;
    bool flag = false;
    bool startObjFlag = false;
    bool test = false;
    // Use this for initialization

    public GUISkin swapSkin;
    public GUISkin nextSkin;
    public GUISkin soundSkin;
    public GUISkin textSkin;
    public GUISkin labelSkin;

    public bool plz = false;
    public bool noLoss = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Alphabet");
        objects = GameObject.FindGameObjectsWithTag(ThemeScript.theme);
        sound = GameObject.FindGameObjectWithTag(ThemeScript.theme + "Sound");
        if (ThemeScript.theme.Equals("Animal"))
        {
            Debug.Log("asdasdasd");
            swapObject = GameObject.FindGameObjectWithTag("SwapAnim");
        }
        else
        {
            swapObject = GameObject.FindGameObjectWithTag("Swap");
        }
        objectStr = objectList[ThemeScript.themeIdx, 0];

    }

    void OnGUI()
    {
        int height = Screen.height / 10;
        int width = Screen.width / 20;

        if (objectIdx == objects.Length)
        {
            objectIdx = 0;
        }

        if (plz == false)
        {
            for (int i = 0; i < objects.Length; i++) {
                objectIdx++;
                if (objectIdx == objects.Length)
                {
                    objectIdx = 0;
                }
                Debug.Log("idx : " + objectIdx);
                objectStr = objectList[ThemeScript.themeIdx, objectIdx];
                for (int k = 0; k < objects.Length; k++)
                {
                    if (objectStr.Equals(objects[k].name))
                    {
                        DefaultTrackableEventHandler.setStateLoss(false);
                        swapObject.GetComponent<Swap>().setModel(objects[k]);
                        break;
                    }
                }
                Debug.Log("cur num : " + objectStr);
                flag = true;
                test = false;
            }
            rotateFlag = true;
            plz = true;
        }
        GUI.skin = swapSkin;

        //GUI버튼 생성 클릭 하면 참
        if (startFlag == true)
        {
            //for(int i=0;i<)
            if (GUI.Button(new Rect(width * 18, (int)(height * 8.5), (int)(width * 1.5), height), ""))
            {
                if (DefaultTrackableEventHandler.trackableItems.Count != 0)
                {
                    curMsg = DefaultTrackableEventHandler.ChkMsg();
                    middleTargetName = DefaultTrackableEventHandler.getTargetinMiddleString();
                    Debug.Log("cur msg : " + curMsg);
                    Debug.Log("가운데 이름 : " + DefaultTrackableEventHandler.getTargetinMiddleString());

                    if (curMsg.Equals(objectStr))
                    {
                        for (int i = 0; i < target.transform.childCount; i++)
                        {
                            childObject = getChildObject(target, i);
                            childObject.GetComponent<ImageFlag>().setChildFalse();
                        }

                        for (int i = 0; i < objects.Length; i++)
                        {
                            if (objects[i].name.Equals(curMsg))
                            {
                                curObject = objects[i];
                                break;
                            }
                        }
                        Debug.Log("middle char is " + middleTargetName);
                        for (int i = 0; i < target.transform.childCount; i++)
                        {
                            childObject = getChildObject(target, i);
                            if (childObject.name.Substring(11, 1).Equals(middleTargetName.Substring(11, 1)))
                            {
                                DefaultTrackableEventHandler.setStateLoss(false);
                                Debug.Log(curObject.name + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                                Debug.Log(childObject.transform + "paaaaaaaaaaaaaaaareeeeeeeeeeeeeen");
                                Animator anim = curObject.GetComponent<Animator>();
                                curObject.transform.parent = childObject.transform;
                                if (ThemeScript.theme.Equals("Animal"))
                                {
                                    Debug.Log("animallllll");
                                    curObject.transform.localPosition = new Vector3(0, 0, 0);
                                    curObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
                                    if (curObject.name.Equals("CHICKEN"))
                                    {
                                        curObject.transform.localScale = new Vector3(2, 2, 2);
                                    }
                                    else if (curObject.name.Equals("FROG"))
                                    {
                                        curObject.transform.localScale = new Vector3(4, 4, 4);
                                    }
                                    else
                                    {
                                        curObject.transform.localScale = new Vector3(1, 1, 1);
                                    }
                                }
                                else
                                {
                                    curObject.transform.localPosition = new Vector3(0, 1, 0);
                                    curObject.transform.Rotate(new Vector3(45, 90, 0) * Time.deltaTime);
                                    //curObject.transform.localRotation = Quaternion.identity;
                                    curObject.transform.localScale = new Vector3(1, 1, 1);
                                }
                                curObject.SetActive(true);
                                noLoss = true;
                                anim.SetInteger("action", 1);
                                break;
                            }

                        }
                    }
                    else
                    {
                        Debug.Log("invalid!");
                    }
                }
            }

            GUI.skin = soundSkin;

            if (GUI.Button(new Rect(width * 18, (int)(height * 7), (int)(width * 1.5), height), ""))
            {
                for (int i = 0; i < sound.transform.childCount; i++)
                {
                    childObject = getChildObject(sound, i);
                    if (childObject.name.Equals(objectStr))
                    {
                        childObject.GetComponent<AudioSource>().Play();
                        break;
                    }
                }

            }

            GUI.skin = nextSkin;

            if (GUI.Button(new Rect(width * 18, (int)(height * 0.1), (int)(width * 1.5), height), ""))
            {
            for (int i = 0; i < target.transform.childCount; i++)
                    {
                        Debug.Log("innnnnnnnnnnnnnnnnnnnnnnn");
                        childObject = getChildObject(target, i);
                        childObject.GetComponent<ImageFlag>().setChildTrue();
                        childObject.GetComponent<ImageFlag>().setNumberChildFalse();
                        if (test == false)
                        {
                            swapObject.GetComponent<Swap>().setModel(GameObject.Find(objectList[ThemeScript.themeIdx, objectIdx]));
                           
                        }
                    }


                noLoss = false;
                objectIdx++;
                if (objectIdx == objects.Length)
                {
                    objectIdx = 0;
                }
                Debug.Log("idx : " + objectIdx);
                objectStr = objectList[ThemeScript.themeIdx, objectIdx];
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objectStr.Equals(objects[i].name))
                    {
                        DefaultTrackableEventHandler.setStateLoss(false);
                        swapObject.GetComponent<Swap>().setModel(objects[i]);
                        break;
                    }
                }
                Debug.Log("cur num : " + objectStr);
                flag = true;
                test = false;

            }

            if (flag)
            {
                GUI.skin = labelSkin;
                GUI.Label(new Rect(width * 7, (int)(height * 0.3), (int)(width * 6.5), (int)(height * 1.3)), "<size=150>" + objectStr.ToUpper() + "</size>");
                if (swapObject != null && startObjFlag == false)
                {
                    swapObject.GetComponent<Swap>().setModel(GameObject.Find(objectList[ThemeScript.themeIdx, objectIdx]));
                    startObjFlag = true;
                }
            }
        }
        else
        {
            GUI.skin = textSkin;
            string start = "<size=200>Start</size>";
            if (GUI.Button(new Rect(width * 7, (int)(height * 4), width * 6, (int)(height * 2)), start))
            {
                flag = true;
                startFlag = true;
                GameObject.Find("Canvas").SetActive(false);
            }
        }
    }

    public GameObject getChildObject(GameObject obj, int idx)
    {
        return obj.transform.GetChild(idx).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (noLoss == false)
        {
            lossFlag = DefaultTrackableEventHandler.getStateLoss();
            if (lossFlag == true)
            {
                for (int i = 0; i < target.transform.childCount; i++)
                {
                    Debug.Log("innnnnnnnnnnnnnnnnnnnnnnn");
                    childObject = getChildObject(target, i);
                    childObject.GetComponent<ImageFlag>().setChildTrue();
                    childObject.GetComponent<ImageFlag>().setNumberChildFalse();
                    if (test == false)
                    {
                        swapObject.GetComponent<Swap>().setModel(GameObject.Find(objectList[ThemeScript.themeIdx, objectIdx]));
                        test = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i].name.Equals(objectStr))
                    {
                        objects[i].SetActive(true);
                        break;
                    }
                }
            }
        }
        if (Application.platform == RuntimePlatform.Android)
        {

            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("theme");
            }
        }
    }
}