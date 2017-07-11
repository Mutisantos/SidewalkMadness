using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	
	[SerializeField]
	private Transform player;
	private Vector3 objetivo;
	[SerializeField]
	private float speed;
	[SerializeField]
	private Vector3 Offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		objetivo = new Vector3 (player.position.x + Offset.x, player.position.y + Offset.y, player.position.z + Offset.z);
		Vector3 movement = Vector3.Lerp (transform.position, objetivo, speed * Time.deltaTime);
		transform.position = movement;
	}
}
