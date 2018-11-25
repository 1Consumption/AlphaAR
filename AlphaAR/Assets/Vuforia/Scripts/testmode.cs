﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testmode : MonoBehaviour {

    public Button btn;
    private static bool s3DModel = false;
    private static bool sloss = false;

    private static int quizI=0 ;
    public Slider quizGage;
    public GameObject quizPosImg;
    public int[] idx;

    //public static SortedList<float, string> trackableItems = new SortedList<float, string>();//to save the card name & position x
    public static string trackableMsg; //current tracked card string
    
    public string lastShowObj = null;
    
    public static SortedList<string, GameObject> modelObj = new SortedList<string, GameObject>(); //match with the string and model
    GameObject[] models;
    GameObject[] imgTargets ;

    // Use this for initialization
    void Start () {
        btn.onClick.AddListener(onBtn1Clicked);

        models = GameObject.FindGameObjectsWithTag("Animal");
        imgTargets = GameObject.FindGameObjectsWithTag("Alphabet");

        for (int i = 0; i < models.Length; i++)
        {
            modelObj.Add(models[i].name, models[i]);
            models[i].SetActive(false);
//            Debug.Log("#set model" + models[i].name);
        }

        //int[] idx = getRandomIntArr(10, 0, modelObj.Count); //after increasing model number over 10.
        idx = getRandomIntArr(modelObj.Count, 0, modelObj.Count);

        quizGage.value = quizI + 1;
        Quiz(modelObj.Values[idx[quizI]]);
    
        /*
        foreach (KeyValuePair<string, GameObject> kv in modelObj)
        {
            kv.Value.SetActive(false);
        }*/
    }


    public void Quiz(GameObject model)
    {
        Debug.Log("*********QUIZ******");
        model.transform.parent = quizPosImg.transform;
        model.transform.localPosition = new Vector3(0, 0, 0);
        model.transform.localRotation = Quaternion.Euler(0,180,0);
        model.transform.localScale = new Vector3(2, 2, 2);
        model.SetActive(true);
    }
    public int[] getRandomIntArr(int length, int min,int max)
    {
        int[] randArr = new int[length];
        bool isDiffer;

        randArr[0] = 0; //to start with cat
       // for (int i = 0; i < length; i++)
        for(int i = 1; i < length; i++)
        {
            do
            {
                randArr[i] = Random.Range(min, max);
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
            
            obj.transform.SetParent(cardInMiddle.transform);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.SetActive(true);

            DefaultTrackableEventHandler.setStateLoss(false);
            anim.SetInteger("action", 1);
            
            s3DModel = true;
        }
        else //No matching result
        {
            Debug.Log("NO :" + trackableMsg);
            s3DModel = false;
            anim.SetInteger("action", 2);
        }
        StartCoroutine("test");
        
        return s3DModel;
    }
    IEnumerator test()
    {
        Debug.Log("3초 후에");
        yield return new WaitForSeconds(3);
        Debug.Log("3초 지남");

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
                    if (childList[i].tag == "Animal")
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
    }
    private void getSwapBack()
    {
        if (lastShowObj != null)
        {
            Debug.Log("###########"+lastShowObj+ "Lost");
            GameObject sub;
            if((sub=GameObject.Find(lastShowObj))!=null){
                Destroy(sub);
            };
            /*
            if (modelObj[lastShowObj] != null)// PROBLEM HERE
            {
                modelObj[lastShowObj].SetActive(false);
            }*/
            s3DModel = false;

            //GameObject[] imgTargets = GameObject.FindGameObjectsWithTag("Alphabet");
            for(int i = 0; i < imgTargets.Length; i++)
            {
                imgTargets[i].GetComponent<ImageFlag>().setAlphabetChildTrue();
            }
        }
    }
    void onBtn1Clicked()
    {
        Debug.Log("pressed btn");

        if (quizI + 1 <= modelObj.Count)
        {

            Transform[] childList = quizPosImg.GetComponentsInChildren<Transform>(true);
            if (childList != null)
            {
                for (int i = 0; i < childList.Length; i++)
                {
                    if (childList[i].tag == "Animal")
                    {
                        modelSwap(childList[i].gameObject);
                    }
                }
            }
        }
        else {
            Debug.Log("END QUIZ");
            //    finish the quiz + result report 
        }
    }
	
	// Update is called once per frame
	void Update () {
        sloss = DefaultTrackableEventHandler.getStateLoss();
        if (sloss && s3DModel)
        {
            getSwapBack();
            quizGage.value = quizI + 1;
        }
    }
}
