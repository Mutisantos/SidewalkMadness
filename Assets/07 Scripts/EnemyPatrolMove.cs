﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolMove : MonoBehaviour
{
	public GameObject pointsContainer;
	//Puntos de recorrido de patrulla
	[SerializeField]
	private List<Transform> waypoints;
	//Velocidad de movimiento del enemigo
	public float step = 0.01f;
	//Multiplicador de velocidad cuando inicia la persecusión
	public float pursuitMultiplier = 1.5f;
	//Si el enemigo tiene que rotar su propio colisionador fisico (enemigos rectangulares) 
	public bool rotateCollider = false;

	//Componente de animacion
	private Animator anim;
	//Velocidad original de animacion
	private float animSpeed;
	//Colision con el jugador
	private Collider2D enemyCollider;
	//Colision fisica 
	private Collider2D enemyBody;
	//Vision del enemigo
	private Collider2D enemyRange;
	//Collider
	private Rigidbody2D myBody;

	//Indice del waypoint - configurable para editar los puntos objetivos
	public int targetPoint = 0;
	//Posicion Actual
	private Vector2 actualPosition;
	//Posicion del waypoint siguiente
	private Vector2 nextPosition;
	//Velocidad de movimiento base
	private float speed;
	//Si el enemigo se encuentra en estado de persecusión
	private bool inPursuit;

	public AudioClip[] detectionFX;

	private float maxDistance = 0.2f;

	private bool counted = false;

	private int collided = 0;



	// Use this for initialization
	void Start ()
	{
		
		enemyRange = GetComponentsInChildren<Collider2D> () [1];
		if (rotateCollider) {//Solo necesario para enemigos no simetricos
			enemyBody = GetComponentsInChildren<Collider2D> () [0];
			enemyCollider = GetComponentsInChildren<Collider2D> () [2];
		}
		myBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		actualPosition = new Vector2 (0f, 0f);
		nextPosition = new Vector2 (0f, 0f);
		speed = step;
		inPursuit = false;
		animSpeed = anim.GetFloat("Speed");
		counted = false;
		if (pointsContainer != null) {
			Transform[] points = pointsContainer.GetComponentsInChildren<Transform> ();
			waypoints.Clear ();
			for (int i = 1; i < points.Length; i++) {
				waypoints.Add(points[i]);
			}
		}

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!GameManager.instance.isEnded ()) {//Los enemigos no pueden seguir persiguiendo si el juego se termina
			movePosition ();
		}
	}

	void Update (){
		if (!GameManager.instance.isEnded ()) {//Los enemigos no pueden seguir persiguiendo si el juego se termina
			changeDirection ();
		}
	}


	/**Metodo que mueve el enemigo hacia una posicion determinada por sus waypoints o el enemigo*/

	public void movePosition ()
	{
		actualPosition = myBody.position;
		if (!inPursuit) {
			nextPosition = waypoints [targetPoint].position;
		}
		if (Vector2.Distance (this.actualPosition, this.nextPosition) > this.maxDistance) {
			myBody.MovePosition (Vector2.MoveTowards (this.actualPosition, this.nextPosition, step));
		} else {
			if (targetPoint >= waypoints.Count - 1) {
				targetPoint = 0;
			} else {
				targetPoint++;
			}
		}	
	}




	/**Metodo que cambia la direccion a la que mira el enemigo segun el punto que tenga de objetivo para moverse*/
	public void changeDirection(){
		/*Establecer la dirección de vision y del sprite*/
		Vector3 temp = enemyRange.transform.rotation.eulerAngles;
		float deltaX = actualPosition.x - nextPosition.x;
		float deltaY = actualPosition.y - nextPosition.y;
		int direction = 0;
		if (Mathf.Abs (deltaX) > Mathf.Abs (deltaY)) {//Darle mas prelación al X que al Y
			if (actualPosition.x > nextPosition.x) {//Apuntar a la derecha
				temp.z = -90f;
				direction = 0;
			}
			if (actualPosition.x < nextPosition.x) {//Apuntar a la izquierda
				temp.z = 90f;
				direction = 2;
			}

			enemyRange.transform.rotation = Quaternion.Euler (temp);
			if (rotateCollider) {
				enemyBody.transform.rotation = Quaternion.Euler (temp);
				enemyCollider.transform.rotation = Quaternion.Euler (temp);
			}

		}
		else {
			if (actualPosition.y < nextPosition.y) {//Apuntar a arriba
				temp.z = 180f;
				direction = 3;
			}
			if (actualPosition.y > nextPosition.y) {//Apuntar a abajo
				temp.z = 0f;
				direction = 1;
			}

			enemyRange.transform.rotation = Quaternion.Euler (temp);
			if (rotateCollider) {
				enemyBody.transform.rotation = Quaternion.Euler (temp);
				enemyCollider.transform.rotation = Quaternion.Euler (temp);
			}

		}

		if(direction != anim.GetInteger("Direction")){
			anim.SetBool ("Idle", false);
			anim.SetInteger ("Direction", direction);
			anim.SetFloat ("Speed", animSpeed);
		}
			
	
	
	}


	//Solo alertar al jugador recien el enemigo puede verme
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.tag == "Player" && !GameManager.instance.isEnded () && !GameManager.instance.isOnCheckpoint()) {
			SoundManager.instance.RandomizeFx (detectionFX);
			inPursuit = true;
			step = pursuitMultiplier * speed;
			Rigidbody2D player = coll.attachedRigidbody;
			nextPosition = player.position;
		}
		else if (coll.tag == "VehicleEnemy"){
			StartCoroutine (mutualEnemyDetection (Random.Range(0,1)));		
		}
	}

	//Solo alertar al jugador recien el enemigo puede verme
	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.tag == "Player" && !GameManager.instance.isEnded () && !GameManager.instance.isOnCheckpoint()) {
			inPursuit = true;
			step = pursuitMultiplier * speed;
			Rigidbody2D player = coll.attachedRigidbody;
			nextPosition = player.position;
		}

		else if (coll.tag == "Player" && !GameManager.instance.isEnded () && GameManager.instance.isOnCheckpoint()) {
			inPursuit = false;
			step = speed;
			nextPosition = waypoints [targetPoint].position;//Vuelvo a mi rutina
			if (!counted) {
				GameManager.instance.addAvoided (1);
				counted = true;
			}
		}
	}


	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.tag == "Player") {
			inPursuit = false;
			step = speed;
			nextPosition = waypoints [targetPoint].position;//Vuelvo a mi rutina
			if (!counted) {
				GameManager.instance.addAvoided (1);
				counted = true;
			}
		}
		if(collided==0){
			step = speed;
		}
	}

	
	void OnCollisionEnter2D(Collision2D other)	{
		Debug.Log(other.otherCollider.tag);
		if(other.otherCollider.tag == "VehicleEnemy"){
			StartCoroutine (mutualEnemyDetection (2));	
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if(other.otherCollider.tag == "VehicleEnemy"){
			this.step = speed;
		}
	}
	public void setWaypoints (List<Transform> waypoints){
		this.waypoints = waypoints;
	}

	public void setTargetPoint(int index){
		this.targetPoint = index;
	}


	//Metodo que hace que el enemigo invierta el orden en el que sigue su recorrido
	void invertWaypointOrder(){
		int newTarget = waypoints.Count - 1 - this.targetPoint;
		if(newTarget < 0)
			newTarget = 0;
		else if (newTarget > waypoints.Count -1)
			newTarget = waypoints.Count -1;
		this.targetPoint = newTarget;
		this.waypoints.Reverse();
	}

	void returnToPreviousWaypoint(){
		if(0 == this.targetPoint){
			this.targetPoint = waypoints.Count - 1;
		}
		else{
			this.targetPoint -= 1;
		}
	}
	
	//Corrutina para que el enemigo decida que hacer si se encuentra con otro 
	IEnumerator mutualEnemyDetection(int index){
		//Acelerar o desacelerar reduce la probabilidad que tengan colisiones "perfectas".
		//Invertir el orden de la rutina funciona como ultimo recurso para colisiones.		
		switch(index){
			//Acelerar
			case(0):this.step = step*1.1f;
					yield return new WaitForSeconds (Random.Range(1,3));
					//returnToPreviousWaypoint();
					break;
			//Desacelerar
			case(1):this.step = step*0.9f;
					yield return new WaitForSeconds (Random.value);
					break;
			//Invertir
			case(2):invertWaypointOrder();
					yield return new WaitForSeconds (Random.value);
					break;
		}
		yield return new WaitForSeconds (0);
	}
}
