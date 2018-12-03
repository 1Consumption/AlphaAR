using UnityEngine;
using System.Collections;
using Vuforia;
using System;
using UnityEngine.SceneManagement;

public class Learn : MonoBehaviour
{
    GameObject childObject = null;
    GameObject curObject = null;
    GameObject target;
    GameObject sound;
    GameObject swapObject = null;
    GameObject[] objects;
    SortedList mapped = new SortedList();
    String[,] objectList = { { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" } };

    String curMsg = "";
    String objectStr = "";
    String middleTargetName= "";
    int objectIdx = 0;

    bool lossFlag = false;
    bool flag = false;

    public GUISkin swapSkin;
    public GUISkin soundSkin;
    public GUISkin nextSkin;

    // Use this for initialization

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Alphabet");
        objects = GameObject.FindGameObjectsWithTag(ThemeScript.theme);
        sound = GameObject.FindGameObjectWithTag(ThemeScript.theme + "Sound");
        swapObject = GameObject.FindGameObjectWithTag("Swap");
        //if (swapObject != null)
        //{
        //    swapObject.GetComponent<Swap>().setModel(GameObject.Find(objectList[ThemeScript.themeIdx,objectIdx]));
        //}
        //objectStr = objectList[ThemeScript.themeIdx, 0];
    }

    void OnGUI()
    {
        int height = Screen.height/9;
        int width = Screen.width/18;

        if (objectIdx == objects.Length)
        {
            objectIdx = 0;
        }


        //GUI버튼 생성 클릭 하면 참
        GUI.skin = swapSkin;
        if (GUI.Button(new Rect((int)(width*16.8), 10, width, height),""))
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
                            curObject.transform.parent = childObject.transform;
                            curObject.transform.localPosition = new Vector3(0, 1, 0);
                            curObject.transform.Rotate(new Vector3(45, 90, 0) * Time.deltaTime);
                            //curObject.transform.localRotation = Quaternion.identity;
                            curObject.transform.localScale = new Vector3(1, 1, 1);
                            curObject.SetActive(true);
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

        GUI.skin = nextSkin;

        if (GUI.Button(new Rect((int)(width * 16.8), height * 8, width, height), ""))
        {
            objectIdx++;
            if (objectIdx == objects.Length)
            {
                objectIdx = 0;
            }
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

        }

        GUI.skin = soundSkin;

        if (GUI.Button(new Rect((int)(width * 15.5), height*8, width, height), ""))
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

        if(GUI.Button(new Rect(10,10,100,100),"asdasd")){
            flag = true;
            if (swapObject != null)
            {
                swapObject.GetComponent<Swap>().setModel(GameObject.Find(objectList[ThemeScript.themeIdx, objectIdx]));
            }
            objectStr = objectList[ThemeScript.themeIdx, 0];
        }



        if (flag)
            GUI.Label(new Rect(510, 10, 700, 210), "<size=150>" + objectStr + "</size>");
    }

    public GameObject getChildObject(GameObject obj, int idx)
    {
        return obj.transform.GetChild(idx).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        lossFlag = DefaultTrackableEventHandler.getStateLoss();
        if(lossFlag==true){
            for (int i = 0; i < target.transform.childCount;i++){
                Debug.Log("innnnnnnnnnnnnnnnnnnnnnnn");
                childObject = getChildObject(target, i);
                childObject.GetComponent<ImageFlag>().setChildTrue();
                childObject.GetComponent<ImageFlag>().setNumberChildFalse();
                swapObject.GetComponent<Swap>().setModel(GameObject.Find(objectList[ThemeScript.themeIdx, objectIdx]));
            }
        }else{
            for (int i = 0; i < objects.Length; i++){
                if(objects[i].name.Equals(objectStr)){
                    objects[i].SetActive(true);
                    break;
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