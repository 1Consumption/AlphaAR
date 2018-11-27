using UnityEngine;
using System.Collections;
using Vuforia;

public class ImageFlag : MonoBehaviour
{
    public TrackableBehaviour theTrackable;
    bool flag = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setFlag(bool rcv)
    {
        this.flag = rcv;
    }

    public bool getFlag()
    {
        return this.flag;
    }

    public void setChildFalse()
    {
        for (int i = 0; i < theTrackable.transform.childCount; i++)
        {
            Transform child = theTrackable.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }

    public void setChildTrue()
    {
        for (int i = 0; i < theTrackable.transform.childCount; i++)
        {
            Transform child = theTrackable.transform.GetChild(i);
            child.gameObject.SetActive(true);
        }
    }

    public void setAlphabetChildFalse()
    {
        for (int i = 0; i < theTrackable.transform.childCount; i++)
        {
            Transform child = theTrackable.transform.GetChild(i);
            if (child.tag == "alphabet3D")
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void setAlphabetChildTrue()
    {
        for (int i = 0; i < theTrackable.transform.childCount; i++)
        {
            Transform child = theTrackable.transform.GetChild(i);
            if (child.tag == "alphabet3D")
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void setNumberChildTrue(){
        for (int i = 0; i < theTrackable.transform.childCount; i++)
        {
            Transform child = theTrackable.transform.GetChild(i);
            if (child.tag == "Number")
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void setNumberChildFalse()
    {
        for (int i = 0; i < theTrackable.transform.childCount; i++)
        {
            Transform child = theTrackable.transform.GetChild(i);
            if (child.tag == "Number")
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}