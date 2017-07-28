using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainInput{
	public class MainInputManager : MonoBehaviour {


		public float horizontal;
		public float vertical;
		public bool leftDown;
		public bool rightDown;
		public bool upDown;
		public bool downDown;



		public bool startBttnDown;
		public bool selectBttnDown;
		public bool button_ADown;
		public bool button_BDown;


		/*Detección de presionado*/
		public bool leftPressed;
		public bool rightPressed;
		public bool startBttnPressed;
		public bool selectBttnPressed;
		public bool button_APressed;
		public bool button_BPressed;

		public bool leftUp;
		public bool rightUp;
		public bool startBttnUp;
		public bool selectBttnUp;
		public bool button_AUp;
		public bool button_BUp;

		public bool playable;
		public bool pc; // Plataforma PC


		void Update () {
			if (playable && pc) {//Inputs de teclado

				horizontal = Input.GetAxisRaw ("Horizontal");
				vertical = Input.GetAxisRaw ("Vertical");

				startBttnDown = Input.GetButtonDown ("Start");
				selectBttnDown = Input.GetButtonDown ("Select");
				button_ADown = Input.GetButtonDown ("A_Btn");
				button_BDown = Input.GetButtonDown ("B_Btn");
				leftDown = Input.GetButtonDown ("Left_btn");
				rightDown = Input.GetButtonDown ("Right_btn");

				startBttnUp = Input.GetButtonUp ("Start");
				selectBttnUp = Input.GetButtonUp ("Select");
				button_AUp = Input.GetButtonUp ("A_Btn");
				button_BUp = Input.GetButtonUp ("B_Btn");
				leftUp = Input.GetButtonUp ("Left_btn");
				rightUp = Input.GetButtonUp ("Right_btn");

				startBttnPressed = Input.GetButton ("Start");
				selectBttnPressed = Input.GetButton ("Select");
				button_APressed = Input.GetButton ("A_Btn");
				button_BPressed = Input.GetButton ("B_Btn");
				leftPressed = Input.GetButton ("Left_btn");
				rightPressed = Input.GetButton ("Right_btn");


			}
			if (playable && !pc) {//Inputs de botones de pantalla
				if (rightDown) {
					horizontal = 1;
				}
				if (leftDown) {
					horizontal = -1;
				}

				if (!rightDown && !leftDown) {
					horizontal = 0;
				}


				if (upDown) {
					vertical = 1;
				}
				if (downDown) {
					vertical = -1;
				}

				if (!upDown && !downDown) {
					vertical = 0;
				}
				
			}
		}

		public void pressRight(bool pressed){
			rightDown = pressed;
		}

		public void pressLeft(bool pressed){
			leftDown = pressed;
		}

		public void pressDown(bool pressed){
			downDown = pressed;
		}

		public void pressUp(bool pressed){
			upDown = pressed;
		}



	}
}
