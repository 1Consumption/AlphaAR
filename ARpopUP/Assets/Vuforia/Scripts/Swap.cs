using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Swap : MonoBehaviour {
    GameObject swap;
    // Use this for initialization
    void Start()
    {
        if (ThemeScript.theme.Equals("Animal"))
        {
            swap = GameObject.Find("SwapAnim");
        }
        else
        {
            swap = GameObject.Find("Swap");
        }
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
            //obj.transform.localPosition = new Vector3(0, 0, 0);
            //obj.transform.localRotation = Quaternion.identity;


            obj.transform.localPosition = new Vector3(0, 0, 0);

            obj.transform.localRotation = Quaternion.Euler(0, 180, 0);

            if (ThemeScript.theme.Equals("Animal"))
            {
                if (obj.name.Equals("CHICKEN"))
                {
                    obj.transform.localScale = new Vector3(2, 2, 2);
                }
                else if (obj.name.Equals("FROG"))
                {
                    obj.transform.localScale = new Vector3(4, 4, 4);
                }
                else if (obj.name.Equals("SHARK"))
                {
                    obj.transform.localPosition = new Vector3(1, 0, 0);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    obj.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                obj.transform.localScale = new Vector3(1, 1, 1);
            }
            obj.SetActive(true);
        }
    }
}
