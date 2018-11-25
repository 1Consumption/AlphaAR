/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System;
using System.Collections.Generic;

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

    public static SortedList<float, string> trackableItems = new SortedList<float, string>();//to save the card name & position x
    public static string trackableMsg; //current tracked card string
    public static bool sloss = false;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
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

    private void Update()
    {

    }

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>

    public static void updateMsg()
    {
        trackableMsg = "";
        foreach (KeyValuePair<float, string> kv in trackableItems)
        {
            trackableMsg = trackableMsg + kv.Value[0];
        }
        ChkMsg();
    }
    public static string ChkMsg()
    {
        Debug.Log("-------MSG:" + trackableMsg);
        return trackableMsg;
    }
    public static void EditMsg(bool i, string str, float x) //determine currently trackable msg
    {
        if (i) //add found trackable target
        {
            trackableItems.Add(x, str);
        }
        else  //remove lost trackable target
        {
            int idx = 0;
            foreach (KeyValuePair<float, string> kv in trackableItems)
            {
                if (kv.Value.Equals(str))
                {
                    trackableItems.RemoveAt(idx);
                    break;
                }
                else
                {
                    idx++;
                }
            }
        }
        updateMsg();
    }

    public static bool getStateLoss()
    {
        return sloss;
    }
    public static void setStateLoss(bool state)
    {
        sloss = state;
    }
    public static string getTargetinMiddleString()
    {
        return "ImageTarget" + trackableItems.Values[trackableItems.Count / 2];
    }

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
            EditMsg(true, mTrackableBehaviour.TrackableName, mTrackableBehaviour.transform.position.x);

            mTrackableBehaviour.GetComponent<ImageFlag>().setFlag(true);
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            EditMsg(false, mTrackableBehaviour.TrackableName, mTrackableBehaviour.transform.position.x);

            setStateLoss(true);
            mTrackableBehaviour.GetComponent<ImageFlag>().setFlag(false);

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