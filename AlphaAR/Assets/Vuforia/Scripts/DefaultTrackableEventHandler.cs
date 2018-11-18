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
    GameObject childObject = null;
    GameObject animalObject = null;
    GameObject target;
    GameObject sound;
    GameObject[] animals;
    SortedList mapped = new SortedList();

    String curMsg = "";

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
        String swapStr = "<size=22>SWAP</size>";
        String soundStr = "<size=22>SOUND</size>";
        //GUI버튼 생성 클릭 하면 참
        if (GUI.Button(new Rect(10, 10, 300, 80), swapStr))
        {
            bool state = false;

            for (int i = 0; i < target.transform.childCount; i++)
            {
                childObject = getChildObject(target, i);
                if (childObject.GetComponent<ImageFlag>().getFlag() == true)
                {
                    mapped.Add(childObject.GetComponent<ImageFlag>().getX(), childObject.name.Substring(11));
                }
            }

            curMsg = getMsg(mapped);

            for (int i = 0; i < animals.Length; i++)
            {
                if (curMsg.Equals(animals[i].name))
                {
                    state = true;
                    animalObject = animals[i];
                    break;
                }
            }

            if (state)
            {
                String middleChar = "";
                for (int i = 0; i < curMsg.Length; i++)
                {
                    if (i==curMsg.Length/2){
                        middleChar = curMsg.Substring(i, i);
                    }
                }

                for (int i = 0; i < target.transform.childCount;i++){
                    childObject = getChildObject(target, i);
                    childObject.GetComponent<ImageFlag>().setChildFalse();
                }

                for (int i = 0; i < target.transform.childCount;i++){
                    childObject = getChildObject(target, i);
                    if(childObject.name.Substring(11).Equals(middleChar)){
                        animalObject.transform.parent = childObject.transform;
                        animalObject.transform.localPosition = new Vector3(0, 0, 0);
                        animalObject.transform.localRotation = Quaternion.identity;
                        animalObject.transform.localScale = new Vector3(1, 1, 1);
                        animalObject.SetActive(true);
                        break;
                    }
                    
                }
                mapped.Clear();
            }
            else
            {
                Debug.Log("invalid!");
            }

        }

        if (GUI.Button(new Rect(10, 110, 300, 80), soundStr))
        {
            if(animalIdx==animals.Length){
                animalIdx = 0;
            }
            for (int i = 0; i < sound.transform.childCount;i++){
                childObject = getChildObject(sound, i);
                if(childObject.name.Equals(animals[animalIdx].name)){
                    childObject.GetComponent<AudioSource>().Play();
                    break;
                }
            }

            animalIdx++;
        }
    }

    public String getMsg(SortedList list){
        String temp = "";
        for (int i = 0; i < list.Count;i++){
            temp += list.GetByIndex(i);
        }

        Debug.Log("getMsg : " + temp);
        return temp;
    }

    public String setMsgEmpty(){
        return "";
    }

    public GameObject getChildObject(GameObject obj,int idx){
        return obj.transform.GetChild(idx).gameObject;
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

            for (int i = 0; i < target.transform.childCount;i++){
                childObject = getChildObject(target, i);
                childObject.GetComponent<ImageFlag>().setChildTrue();
            }

            for (int i = 0; i < animals.Length;i++){
                animals[i].SetActive(false);
            }
            curMsg = setMsgEmpty();
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
