using UnityEngine;
//*Clase nativa para crear edificios o bloques de calles en el mapa */
public class Building : MonoBehaviour{
	//X y Y iniciales
	public int x;
	public int y;
	//Dimensiones del edificio
	public int height;
	public int width;

	public Building(int x,int y,int h,int w){
		this.x = x;
		this.y = y;
		this.height = h;
		this.width = w;
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


}
