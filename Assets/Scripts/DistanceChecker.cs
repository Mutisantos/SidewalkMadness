using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceChecker : MonoBehaviour {

	public Transform player;
	public Transform goal;
	public Transform begin;
	private float distance;
	private float maxVal;
	public Slider barraDis;
	public Image fill;

	// Use this for initialization
	void Start () {
		barraDis.maxValue = Vector2.Distance (begin.position, goal.position);	
		maxVal = barraDis.maxValue;
	}
	
	// Update is called once per frame
	void Update () {
		checkDistance ();
	}


	void checkDistance(){
		distance = Vector2.Distance (player.position, goal.position);
		float gap = -(distance - maxVal);
		barraDis.value = gap;

		float percentDis = (distance *100) / maxVal;

		if (percentDis < 20) {
			fill.color = Color.green;
		} else if (percentDis > 20 && percentDis < 40) {
			fill.color = Color.yellow;
		} else if (percentDis > 40 && percentDis < 60) {
			fill.color = new Color32 (240, 130, 40, 255);
		} else if (percentDis > 60 && percentDis < 80) {
			fill.color = new Color32 (229,78,51,255);
		} else if (percentDis > 80) {
			fill.color = Color.red;
		}


	}
}
