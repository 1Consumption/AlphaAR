using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Visibility : MonoBehaviour {

    GameObject Animals;
	// Use this for initialization
	void Start () {
        Animals = GameObject.Find("Animals");
        Animals.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
