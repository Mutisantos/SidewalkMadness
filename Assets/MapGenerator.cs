using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Area{
	public int height;
	public int width;

	public Area(int x, int y){
		height = x;
		width = y;
	}

}

public class MapGenerator : MonoBehaviour {

	public Building[] buildings;
	public Building[] roads;
	public int height;
	public int width;
	public int mainStreetDepth;
	
	
	public bool finishedLoading;
	private int direction; //Par:Horizontal; Impar:Vertical


	// Use this for initialization
	void Start () {
		direction = 0;
		finishedLoading = false;
		this.generateMap();
	}
	
	private void generateMap(){
		Queue<Area> regiones = new Queue<Area>();
		Area regTotal = new Area(height,width);
		regiones.Enqueue(regTotal);
		for(int i =1;i<=mainStreetDepth;i++){
			for(int j = 0; j < Mathf.Pow(2,i);j++){
				Area regPartir = regiones.Dequeue();
				

			}
		}


	}

	private Area calculateRegion(int x, int y, int x1, int y1){
		Area area = new Area();
		area.height = Mathf.Abs(x - x1);
		area.width = Mathf.Abs(y - y1);
		return area;
	}



}
