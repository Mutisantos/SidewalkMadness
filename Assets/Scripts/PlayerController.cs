using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MainInput;

public class PlayerController : MonoBehaviour {

	public float xAxisThreshold = 0.02f;
	public float yAxisThreshold = 0.02f;
	public Vector2 speed;

	private Animator anim;
	private Rigidbody2D mybody;
	private Vector2 movement;
	public MainInputManager mainInput;
	public GameObject collisionFX;
	private bool moveSound = false;

	//Efectos de sonido para el jugador
	public AudioClip walk1;
	public AudioClip walk2;
	public AudioClip dieSound;
	public AudioClip dieExplosion;
	//Control del audio de caminada del personaje
	private Vector2 lastStepPosition;
	public float deltaStep = 1f;


	// Use this for initialization
	void Awake () {
		mybody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		movement = new Vector2(mybody.position.x, mybody.position.y);
		speed = new Vector2(0.2f,0.2f);
		lastStepPosition = mybody.position;
		collisionFX.SetActive (false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GameManager.instance.isAlive ()) {//Los no me puedo seguir moviendo si me muero
			checkStep ();
			walk ();
		}
	}


	/**Metodo para mover al personaje en un espacio XY*/
	private void walk(){


		Vector2 velocity = speed;
		float h = mainInput.horizontal;
		float l = mainInput.vertical;

		//Estado de Idle
		if (l > -yAxisThreshold && l < yAxisThreshold && h > -xAxisThreshold && h < xAxisThreshold) {
			anim.SetBool ("Idle", true);
			anim.SetFloat ("VerticalDirection", 0);
			anim.SetFloat ("HorizontalDirection", 0);
			mybody.velocity.Set(0f,0f);
			movement.x = mybody.position.x;
			movement.y = mybody.position.y;
			mybody.MovePosition (new Vector2 (movement.x, movement.y));

		}

		/*Normalizacion*/
		velocity = new Vector2(h,l);
		velocity = velocity.normalized;
		velocity.x = speed.x * velocity.x;
		velocity.y = speed.y * velocity.y;


		if (h > 0) {//derecha
			anim.SetBool ("Idle", false);
			anim.SetFloat ("HorizontalDirection", h);
			anim.SetFloat ("VerticalDirection", 0);
			movement.x = mybody.position.x + velocity.x;
		}

		else if (h < 0) {//izquierda
			anim.SetBool ("Idle", false);
			anim.SetFloat ("HorizontalDirection", h);
			anim.SetFloat ("VerticalDirection", 0);
			movement.x = mybody.position.x + velocity.x;
		}



		if (l > 0) {//arriba
			anim.SetBool ("Idle", false);
			anim.SetFloat ("VerticalDirection", l);
			anim.SetFloat ("HorizontalDirection", 0);
			movement.y = mybody.position.y + velocity.y;
		}

		else if (l < 0) {//abajo
			anim.SetBool ("Idle", false);
			anim.SetFloat ("VerticalDirection", l);
			anim.SetFloat ("HorizontalDirection", 0);
			movement.y = mybody.position.y + velocity.y;
		}


		if (l != 0 || h != 0) {
			mybody.MovePosition (new Vector2 (movement.x, movement.y));
			if (moveSound) {
				SoundManager.instance.RandomizePlayerFx (this.walk1, this.walk2);
				moveSound = false;
			}
		}
	
	}


	public void checkStep(){
		if (Vector2.Distance (mybody.position, lastStepPosition) >= deltaStep && GameManager.instance.isAlive()) { 
			moveSound = true;
			lastStepPosition = mybody.position;
		}
	}

	//Trigger que determina si el jugador gana o pierde al contacto
	private void OnTriggerEnter2D(Collider2D coll){
		
		if (coll.tag == "EnemyBody") {
			if (GameManager.instance.isSurvivalMode ()) {
				anim.SetBool ("Alive", false);
				GameManager.instance.setAlive (false);
				GameManager.instance.setEnded (true);
				StartCoroutine (dyingEffects ());
				StartCoroutine (waitForEndGame (3f));
			}
			else {
				StartCoroutine (respawnPlayer ());
			}
		}

		if (coll.tag == "Goal") {
			GameManager.instance.setAlive(true);
			GameManager.instance.setEnded(true);
			anim.SetBool ("Win", true);
			mybody.bodyType = RigidbodyType2D.Kinematic;
			StartCoroutine (waitForEndGame(3f));
		}

	
	}

	private void OnCollisionEnter2D(Collision2D coll){
		if (coll.collider.tag == "Buildings") {
			StartCoroutine (collisionEffect (true));
		}
	}

	private void OnCollisionExit2D(Collision2D coll){
		if (coll.collider.tag == "Buildings") {
			StartCoroutine (collisionEffect (false));
		}
	}

	IEnumerator waitForEndGame(float seconds){
		yield return new WaitForSeconds (seconds);
		SoundManager.instance.changeMusic (3);
		SceneManager.LoadSceneAsync("03Fin");
	}

	IEnumerator dyingEffects(){
		SoundManager.instance.PlayPlayerOnce (this.dieSound);
		yield return new WaitForSeconds (0.5f);
		SoundManager.instance.PlayOnce (this.dieExplosion);
	}

	IEnumerator collisionEffect(bool state){
		collisionFX.SetActive (state);
		yield return new WaitForSeconds (0.5f);
	}

	IEnumerator respawnPlayer(){
		
		anim.SetBool ("Alive", false);
		SoundManager.instance.PlayPlayerOnce (this.dieSound);
		yield return new WaitForSeconds (0.5f);
		SoundManager.instance.PlayOnce (this.dieExplosion);
		GameManager.instance.setAlive (false);
		GameManager.instance.setEnded (true);
		yield return new WaitForSeconds (0.5f);
		anim.SetBool ("Alive", true);
		anim.SetFloat ("Speed", 2000f);
		yield return new WaitForSeconds (0.05f);
		anim.SetFloat ("Speed", 1f);
		GameManager.instance.setAlive (true);
		GameManager.instance.setEnded (false);
		transform.position = (GameManager.instance.getRespawnPoint ());

	}
}
