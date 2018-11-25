using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,100,100),"learn")){
            Application.LoadLevel("SampleScene");
        }

        if (GUI.Button(new Rect(10, 110, 100, 210), "quiz"))
        {
            Application.LoadLevel("quizMode");
        }

    }
}
