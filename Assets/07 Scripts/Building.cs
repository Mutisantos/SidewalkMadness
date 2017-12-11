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

	public int enemyCounter;

	private List<Transform> waypoints;

	public Building(int x,int y,int height,int width){
		enemyCounter = 0;
		this.x = x;
		this.y = y;
		this.height = height;
		this.width = width;
		waypoints = new List<Transform>();
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

	public void AddEnemies(){
		
	}


}
