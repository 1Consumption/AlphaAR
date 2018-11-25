using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Swap : MonoBehaviour {
    GameObject swap;
    // Use this for initialization
    void Start () {
        swap = GameObject.Find("Swap");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setModel(GameObject obj)
    {
        if (swap != null && obj != null)
        {
            for (int i = 0; i < swap.transform.childCount; i++)
            {
                swap.transform.GetChild(i).gameObject.SetActive(false);
            }

            obj.transform.parent = swap.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.SetActive(true);
        }
    }
}
