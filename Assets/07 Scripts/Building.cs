using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//*Clase nativa para crear edificios o bloques de calles en el mapa */
public class Building : MonoBehaviour{
	//X y Y iniciales
	public int x;
	public int y;
	//Dimensiones del edificio
	public int height;
	public int width;

	private List<Transform> waypoints = new List<Transform>();

	void Start(){
	//	setBuildingPosition();
	}

	public void setBuildingPosition(){
		transform.position = new Vector3(x,y,0);
	}

	public int getX(){
		return x;
	}
	public int getY(){
		return y;
	}
	public int getHeight(){
		return height;
	}
	public int getWidth(){
		return width;
	}

	public bool isVertical(){
		if(height > width)
			return true;
		else
			return false;
	}


}
