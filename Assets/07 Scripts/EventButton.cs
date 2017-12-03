using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButton : MonoBehaviour {


	public GameObject[] elements;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void moveUp (){
		Debug.Log ("Me muevo arriba");
	}

	public void esconder(){
		foreach (GameObject el in elements){
			el.SetActive (false);
		}	
	}

	public void mostrar(){
		foreach (GameObject el in elements){
			el.SetActive (true);
		}	
	}


}
