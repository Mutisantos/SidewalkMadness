using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResultScreen : MonoBehaviour {

	public Text timeText;
	public Text enemiesText;
	public Text titleText;
	public string winMessage;
	public string loseMessage;


	// Use this for initialization
	void Start () {
		stateChecker ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void stateChecker(){

		System.TimeSpan t = System.TimeSpan.FromSeconds (GameManager.instance.getTime());
		enemiesText.text = "ENEMIGOS EVADIDOS:" + GameManager.instance.getAvoided();
		if (GameManager.instance.isAlive()) {
			titleText.text = winMessage;
			timeText.text = "TIEMPO TOTAL:" + string.Format ("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds);
		} else {
			titleText.text = loseMessage;
			timeText.gameObject.SetActive (false);
		}

	}

}
