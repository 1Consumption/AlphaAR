/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System;

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
    GameObject cat;
    GameObject[] target;
    GameObject[] animals;
    bool flag = false;
    int index = 0;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Alphabet");
        //Debug.Log(Math.Ceiling((float)target.Length / (float)2));
        cat = GameObject.Find("CAT");
        animals = GameObject.FindGameObjectsWithTag("Animal");

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
        if (GUI.Button(new Rect(10, 10, 300, 80), "swap"))
        {
            bool state = true;
            for (int i = 0; i < target.Length;i++){
                if(target[i].GetComponent<ImageFlag>().getFlag()==false){
                    state = false;
                }
            }

            if (state)
            {
                //for (int i = 0; i < target.Length-1; i++)
                //{
                //    if (target[i].transform.position.y>target[i+1].transform.position.y)
                //    {
                //        state = false;
                //    }
                //}
                if (state)
                {
                    for (int i = 0; i < target.Length;i++){

                        target[i].GetComponent<ImageFlag>().setChildFalse();
                    }

                    if (flag == false)
                    {
                        cat.transform.parent = target[target.Length/2].transform;
                        // Adjust the position and scale
                        // so that it fits nicely on the target
                        cat.transform.localPosition = new Vector3(0, 0.2f, 0);
                        cat.transform.localRotation = Quaternion.identity;
                        cat.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        flag = true;
                    }
                    cat.SetActive(true);

                } else{
                    Debug.Log("invalid!");
                }
            }
        }

        if (GUI.Button(new Rect(10, 100, 300, 80), "recovery"))
        {
            for (int i = 0; i < target.Length;i++){
                target[i].GetComponent<ImageFlag>().setChildTrue();
            }
            cat.SetActive(false);
        }
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

            for (int i = 0; i < target.Length;i++){
                target[i].GetComponent<ImageFlag>().setChildTrue();
            }
            cat.SetActive(false);
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
