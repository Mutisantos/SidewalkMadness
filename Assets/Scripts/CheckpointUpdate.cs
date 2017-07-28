using System.Collections;
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
			GameManager.instance.setOnCheckpoint (true);
			anim.SetBool ("checked",true);
		}
	}


	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.tag == "Player" && !GameManager.instance.isEnded () && !GameManager.instance.isSurvivalMode()) {
			GameManager.instance.setOnCheckpoint (false);
			anim.SetBool ("checked",false);
		}
	}


	public void setActiveCheckpoint(bool active){
		if (!GameManager.instance.isSurvivalMode ())
			anim.SetBool ("checked",active);
	}

}
