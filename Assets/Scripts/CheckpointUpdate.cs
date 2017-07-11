﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointUpdate : MonoBehaviour {


	private Animator anim;

	void Start(){
		if (GameManager.instance.isSurvivalMode ()) {
			this.gameObject.SetActive (false);
		} else {
			anim = GetComponent<Animator> ();
		}
	}


	//El checkpoint cambia el respawning point del jugador 
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.tag == "Player" && !GameManager.instance.isEnded () && !GameManager.instance.isSurvivalMode()) {
			GameManager.instance.setRespawnPoint (transform.position);
			GameManager.instance.setlastCheckpoint (this);
			anim.SetBool ("checked",true);
		}
	}


	public void setActiveCheckpoint(bool active){
		if (!GameManager.instance.isSurvivalMode ())
			anim.SetBool ("checked",active);
	}

}
