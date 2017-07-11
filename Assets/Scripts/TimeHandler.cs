using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour {


	private Text timeText;
	private float timeAlive;
	//private Text lifeText;



	void Start(){
		timeText = GameObject.Find ("TimeText").GetComponent<Text> ();
		timeAlive = Time.timeSinceLevelLoad;
		/**inicializar el juego reiniciando las variables del Game Manager */
		GameManager.instance.resetValues ();

	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.isEnded()) {
			timeAlive = Time.timeSinceLevelLoad;
			GameManager.instance.setTime(timeAlive);
			System.TimeSpan t = System.TimeSpan.FromSeconds (timeAlive);
			timeText.text = string.Format ("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, (timeAlive * 1000) % 100);
		}
	}
}
