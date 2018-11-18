/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System;
using System.Collections;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;
    GameObject target;
    GameObject sound;
    GameObject[] animals;
    GameObject animalObject = null;
    GameObject childObj = null;
    String curMsg = null;
    SortedList mapped = new SortedList();

    int animalIdx = 0;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Alphabet");
        animals = GameObject.FindGameObjectsWithTag("Animal");
        sound = GameObject.FindGameObjectWithTag("Sound");

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    void OnGUI()
    {
        //GUI버튼 생성 클릭 하면 참
        String swapStr = "<size=22>SWAP</size>";
        String soundStr = "<size=22>SOUND</size>";
        if (GUI.Button(new Rect(10, 10, 300, 80), swapStr))
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                childObj = target.transform.GetChild(i).gameObject;
                if (childObj.GetComponent<ImageFlag>().getFlag() == true)
                {
                    mapped.Add(childObj.transform.position.x, childObj.name);
                }
            }
            curMsg = getMsg(mapped);
            bool state = false;

            for (int i = 0; i < animals.Length; i++)
            {
                if (curMsg.Equals(animals[i].name))
                {
                    animalObject = animals[i];
                    state = true;
                    break;
                }
            }

            if (state)
            {
                String middleChar = "";
                for (int i = 0; i < curMsg.Length; i++)
                {
                    if (i == curMsg.Length / 2)
                    {
                        middleChar = curMsg.Substring(i, i);
                        break;
                    }
                }
                Debug.Log("middleChar is " + middleChar);

                for (int i = 0; i < target.transform.childCount; i++)
                {
                    childObj = target.transform.GetChild(i).gameObject;
                    if (childObj.GetComponent<ImageFlag>().getFlag() == true)
                    {
                        childObj.GetComponent<ImageFlag>().setChildFalse();
                    }
                }

                for (int i = 0; i < target.transform.childCount; i++)
                {
                    childObj = target.transform.GetChild(i).gameObject;
                    if (childObj.name.Substring(11).Equals(middleChar))
                    {
                        Debug.Log("childobj name is " + childObj.name);
                        animalObject.transform.parent = childObj.transform;
                        animalObject.transform.localPosition = new Vector3(0, 0, 0);
                        animalObject.transform.localRotation = Quaternion.identity;
                        animalObject.SetActive(true);
                        break;
                    }
                }

                mapped.Clear();
            }
            else
            {
                Debug.Log("Invalid!");
            }

        }

        if (GUI.Button(new Rect(10, 100, 300, 80), soundStr))
        {
            if(animalIdx==animals.Length){
                animalIdx = 0;
            }


            for (int i = 0; i < sound.transform.childCount;i++){
                childObj = sound.transform.GetChild(i).gameObject;
                if(childObj.name.Equals(animals[animalIdx].name)){
                    childObj.GetComponent<AudioSource>().Play();
                    break;
                }
            }

            animalIdx++;

        }

    }


    public String getMsg(SortedList list)
    {
        String temp = "";
        for (int i = 0; i < list.Count;i++){
            temp += list.GetByIndex(i).ToString().Substring(11);
        }
        Debug.Log("getMsg() : " + temp);
        return temp;
    }

    public String setMsgEmpty(){
        Debug.Log("message will be empty!");
        return "";
    }

    //private void Update()
    //{

    //}

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

            mTrackableBehaviour.GetComponent<ImageFlag>().setFlag(true);
            Debug.Log(mTrackableBehaviour.TrackableName + " is " + GameObject.Find("ImageTarget" + mTrackableBehaviour.TrackableName).GetComponent<ImageFlag>().getFlag());
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            mTrackableBehaviour.GetComponent<ImageFlag>().setFlag(false);
            Debug.Log(mTrackableBehaviour.TrackableName+" is "+ GameObject.Find("ImageTarget" + mTrackableBehaviour.TrackableName).GetComponent<ImageFlag>().getFlag());

            curMsg = setMsgEmpty();

            for (int i = 0; i < target.transform.childCount; i++)
            {
                childObj = target.transform.GetChild(i).gameObject;
                childObj.GetComponent<ImageFlag>().setChildTrue();
            }

            for (int i = 0; i < animals.Length;i++){
                animals[i].SetActive(false);
            }

            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PROTECTED_METHODS


}


