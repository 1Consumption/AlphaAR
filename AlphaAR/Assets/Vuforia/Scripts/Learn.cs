using UnityEngine;
using System.Collections;
using Vuforia;
using System;

public class Learn : MonoBehaviour
{
    GameObject childObject = null;
    GameObject numberObject = null;
    GameObject target;
    GameObject sound;
    GameObject swapObject = null;
    GameObject[] numbers;
    SortedList mapped = new SortedList();
    String[] numberList = { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" };

    String curMsg = "";
    String numberStr = "";
    String middleTargetName= "";
    int numberIdx = 0;

    bool lossFlag = false;
    bool flag = false;
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Alphabet");
        numbers = GameObject.FindGameObjectsWithTag("Number");
        sound = GameObject.FindGameObjectWithTag("Sound");
        swapObject = GameObject.FindGameObjectWithTag("Swap");
        if (swapObject != null)
        {
            swapObject.GetComponent<Swap>().setModel(GameObject.Find(numberList[numberIdx]));
        }
        numberStr = numberList[0];
    }

    void OnGUI()
    {
        int height = Screen.height;
        int width = Screen.width;
        String swapStr = "<size=30>SWAP</size>";
        String soundStr = "<size=30>SOUND</size>";
        String nextStr = "<size=30>NEXT</size>";
        String returnMenu = "<size=30>RETURN TO MENU</size>";

        if (numberIdx == numbers.Length)
        {
            numberIdx = 0;
        }

        //GUI버튼 생성 클릭 하면 참
        if (GUI.Button(new Rect(10, 10, 300, 80), swapStr))
        {
            curMsg = DefaultTrackableEventHandler.ChkMsg();
            middleTargetName = DefaultTrackableEventHandler.getTargetinMiddleString();
            Debug.Log("cur msg : " + curMsg);
            Debug.Log("가운데 이름 : " + DefaultTrackableEventHandler.getTargetinMiddleString());

            if (curMsg.Equals(numberStr))
            {
                for (int i = 0; i < target.transform.childCount; i++)
                {
                    childObject = getChildObject(target, i);
                    childObject.GetComponent<ImageFlag>().setChildFalse();
                }

                for (int i = 0; i < numbers.Length;i++){
                    if(numbers[i].name.Equals(curMsg)){
                        numberObject = numbers[i];
                        break;
                    }
                }
                Debug.Log("middle char is " + middleTargetName);
                for (int i = 0; i < target.transform.childCount; i++)
                {
                    childObject = getChildObject(target, i);
                    if (childObject.name.Substring(11, 1).Equals(middleTargetName.Substring(11,1)))
                    {
                        DefaultTrackableEventHandler.setStateLoss(false);
                        Debug.Log(numberObject.name + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                        Debug.Log(childObject.transform + "paaaaaaaaaaaaaaaareeeeeeeeeeeeeen");
                        numberObject.transform.parent = childObject.transform;
                        numberObject.transform.localPosition = new Vector3(0, 1, 0);
                        numberObject.transform.localRotation = Quaternion.identity;
                        numberObject.transform.localScale = new Vector3(1, 1, 1);
                        numberObject.SetActive(true);
                        break;
                    }

                }

            }
            else
            {
                Debug.Log("invalid!");
            }
        }

        if (GUI.Button(new Rect(10, 110, 300, 80), soundStr))
        {
            for (int i = 0; i < sound.transform.childCount; i++)
            {
                childObject = getChildObject(sound, i);
                if (childObject.name.Equals(numberStr))
                {
                    childObject.GetComponent<AudioSource>().Play();
                    break;
                }
            }

        }

        if (GUI.Button(new Rect(10, 210, 300, 80), nextStr))
        {
            numberIdx++;
            if (numberIdx == numbers.Length)
            {
                numberIdx = 0;
            }
            numberStr = numberList[numberIdx];
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numberStr.Equals(numbers[i].name))
                {
                    DefaultTrackableEventHandler.setStateLoss(false);
                    swapObject.GetComponent<Swap>().setModel(numbers[i]);
                    break;
                }
            }
            Debug.Log("cur num : " + numberStr);
            flag = true;
        }

        if (GUI.Button(new Rect(10, 310, 300, 80), returnMenu))
        {
            Application.LoadLevel("button");
        }

        if (flag)
            GUI.Label(new Rect(510, 10, 700, 210), "<size=150>" + numberStr + "</size>");
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
                swapObject.GetComponent<Swap>().setModel(GameObject.Find(numberList[numberIdx]));
            }
        }else{
            for (int i = 0; i < numbers.Length; i++){
                if(numbers[i].name.Equals(numberStr)){
                    numbers[i].SetActive(true);
                    break;
                }
            }
        }
    }
}