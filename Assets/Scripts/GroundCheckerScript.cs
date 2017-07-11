using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GroundChecker{//Encapsulamiento para quienes hagan using.GroundChecker
	public class GroundCheckerScript : MonoBehaviour {
		[SerializeField]
		private bool grounded;
		void OnTriggerStay2D(Collider2D coll){
			if (coll.tag == "Ground") {
				grounded = (coll.tag == "Ground");
			}
		}

		void OnTriggerExit2D(Collider2D coll){
			grounded = false;	
		}

		public bool isGrounded(){
			return grounded;
		}
	}
}