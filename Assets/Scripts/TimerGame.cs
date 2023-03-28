using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerGame : MonoBehaviour {

    public float startTime;
    public float elapsedTime;
    public Text timeResultText;
    public TableManager tableManager;
	
    // Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (tableManager.strikesCount < 3)
        {
            elapsedTime = Time.time - startTime;           
        }

        if (tableManager.strikesCount == 3)
        {
            timeResultText.text = "" + Mathf.Round(elapsedTime).ToString() + " segs";
            return;
        }
	}
}
