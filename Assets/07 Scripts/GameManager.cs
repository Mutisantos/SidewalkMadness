using UnityEngine;
using System.Collections;


//Singleton para GameManager
public class GameManager : MonoBehaviour {

	public static GameManager instance;

	private float time = 0f;
	private int enemiesAvoided = 0;
	private bool ended = false;
	private bool alive = false;
	//Modo de juego de contrarreloj, el checkpoint solo aplica cuando survivalMode es falso
	public bool survivalMode = true;
	//Dejar al jugador inmune en los checkpoints
	[SerializeField]
	private bool onCheckpoint = false;
	[SerializeField]
	private Vector2 respawnPoint;
	[SerializeField]
	public CheckpointUpdate lastCheckpoint;

	void Awake() {
		MakeSingleton ();
	}

	private void MakeSingleton() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}


	public float getTime(){
		return this.time;
	}

	public void setTime(float time){
		this.time = time;
	}

	public int getAvoided(){
		return this.enemiesAvoided;
	}

	public void setAvoided(int avoided){
		this.enemiesAvoided = avoided;
	}

	public void addAvoided(int avoided){
		this.enemiesAvoided += avoided;
	}



	public bool isEnded(){
		return this.ended;
	}

	public void setEnded(bool ended){
		this.ended = ended;
	}

	public bool isAlive(){
		return this.alive;
	}

	public void setAlive(bool alive){
		this.alive = alive;
	}

	public void resetValues(){
		this.ended = false; 
		this.time = 0f; 
		this.enemiesAvoided = 0; 
		this.alive = true;
	}

	public bool isSurvivalMode(){
		return this.survivalMode;
	}

	public void setSurvivalMode(bool survivalMode){
		this.survivalMode = survivalMode;
	}

	public bool isOnCheckpoint(){
		if (survivalMode) {
			return false;
		}
		return this.onCheckpoint;
	}

	public void setOnCheckpoint(bool onCheckpoint){
		this.onCheckpoint = onCheckpoint;
	}

	public Vector2 getRespawnPoint(){
		return this.respawnPoint;
	}

	public void setRespawnPoint(Vector2 respawnPoint){
		this.respawnPoint = respawnPoint;
	}


	public void setlastCheckpoint(CheckpointUpdate lastCheckpoint){
		if(this.lastCheckpoint != null)
			this.lastCheckpoint.setActiveCheckpoint (false);//El anterior checkpoint queda inactivo ...
		this.lastCheckpoint = lastCheckpoint;// ... y se reemplaza por el nuevo
	}


} 
