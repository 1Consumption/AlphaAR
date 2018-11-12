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

    public void setFlag(bool rcv){
        this.flag = rcv;
    }

    public bool getFlag(){
        return this.flag;
    }

    public float getYPosition(){
        return this.transform.position.y;
    }

    public void setChildFalse(){
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
}
