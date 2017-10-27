using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Area{

	public int x;
	public int y;
	public int height;
	public int width;

	public Area(int x, int y, int h, int w){
		this.height = h;
		this.width = w;
		this.x = x;
		this.y = y;
	}

}

public class MapGenerator : MonoBehaviour {

	public Building[] buildings;
	public Building[] roads;
	public int height;
	public int width;
	public int mainStreetDepth;
	public int randomRange;	
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
		Area regTotal = new Area(0,0,height,width);
		regiones.Enqueue(regTotal);
		for(int i =1;i<=mainStreetDepth;i++){
			for(int j = 0; j < Mathf.Pow(2,i);j++){
				Area regPartir = regiones.Dequeue();
				int partitionCoord = 0;
				if(direction%2==0){
					partitionCoord = Random.Range(regPartir.width/2 - randomRange, regPartir.width/2 + randomRange);
					Area izq = calculateRegion(regPartir.x, regPartir.y, partitionCoord, regPartir.width);
					Area der = calculateRegion(partitionCoord, regPartir.y, regPartir.height, regPartir.width);
					regiones.Enqueue(izq);
					regiones.Enqueue(der);
				}
				else{
					partitionCoord = Random.Range(regPartir.height/2 - randomRange, regPartir.height/2 + randomRange);
					Area arr = calculateRegion(regPartir.x, regPartir.y, partitionCoord, regPartir.width);
					Area abj = calculateRegion(partitionCoord, regPartir.y, regPartir.height, regPartir.width);
					regiones.Enqueue(arr);
					regiones.Enqueue(abj);
				}
			}
			direction++;
		}


	}

	private Area calculateRegion(int x, int y, int x1, int y1){
		Area area = new Area();
		area.x = x;
		area.y = y;
		area.height = Mathf.Abs(x - x1);
		area.width = Mathf.Abs(y - y1);
		return area;
	}



}
