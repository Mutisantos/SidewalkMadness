using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainInput;
using GroundChecker;


public class NinjaController : MonoBehaviour {

	private Rigidbody2D rb;
	private Animator anim;
	private float vSpeed;
	private MainInputManager minpute;
	private bool faceRight;
	private SpriteRenderer sprit;

	private GroundCheckerScript gch;
	[SerializeField]
	private float jumpForce;
	private float speed;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		minpute = GetComponent<MainInputManager> ();
		gch = GetComponentInChildren<GroundCheckerScript> ();
		sprit = GetComponent<SpriteRenderer> ();
		speed = 10;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		vSpeed = rb.velocity.y;
		anim.SetFloat ("verticalSpeed", vSpeed);
		if(minpute.button_ADown){
			rb.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
		}
		anim.SetBool  ("Grounded", gch.isGrounded());

		Vector2 movementX = new Vector2 (minpute.horizontal * speed, rb.velocity.y);
		rb.velocity = movementX;
		if (faceRight && minpute.horizontal < 0)
			FliptheSprite ();
		if (!faceRight && minpute.horizontal > 0)
			FliptheSprite ();
		anim.SetBool ("Walking", minpute.horizontal != 0);


	}

	void FliptheSprite(){
		sprit.flipX = faceRight;
		faceRight = !faceRight;
	
		
	}


}
