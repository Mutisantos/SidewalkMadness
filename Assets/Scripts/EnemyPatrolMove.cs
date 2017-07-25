using System.Collections;
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
		anim.SetFloat ("Speed", animSpeed);
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
			anim.SetFloat ("Speed", animSpeed * 20);
		}
			
	
	
	}



	//Solo alertar al jugador recien el enemigo puede verme
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.tag == "Player" && !GameManager.instance.isEnded ()) {
			SoundManager.instance.RandomizeFx (detectionFX);
		}
	}


	/**Metodo para usar el Trigger de vision del personaje para perseguirlo mientras pueda verlo*/
	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.tag == "Player") {
			inPursuit = true;
			step = pursuitMultiplier * speed;
			Rigidbody2D player = coll.attachedRigidbody;
			nextPosition = player.position;
		}
	}

	/**Metodo para usar el Trigger cuando el personaje salga del rango de vision*/
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
	}






}
