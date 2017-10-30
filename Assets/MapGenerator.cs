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
	public Building vroad;
	public Building hroad;
	
	public int height;
	public int width;
	public int mainStreetDepth;
	public int randomRange;	
	public bool finishedLoading;

	public int mapRate;
	private int direction; //Par:Horizontal; Impar:Vertical



	void Start () {
		direction = 0;
		finishedLoading = false;
		this.generateMap();
	}
	
	private void generateMap(){
		Queue<Area> regiones = new Queue<Area>();
		Queue<Area> calles = new Queue<Area>();
		Area regTotal = new Area(0,0,height,width);
		regiones.Enqueue(regTotal);

		//Generacion de regiones principales
		for(int i =0;i<mainStreetDepth;i++){
			for(int j = 0; j < Mathf.Pow(2,i);j++){
				Area regPartir = regiones.Dequeue();
				int partitionCoord = 0;
				if(direction%2==0){//Vertical
					int lowrange = 	regPartir.width/2 - randomRange > 0 ? regPartir.width/2 - randomRange : 0;
					partitionCoord = Random.Range(lowrange, regPartir.width/2 + randomRange);
					Area izq = new Area(regPartir.x, regPartir.y, regPartir.height, partitionCoord);
					Area der = new Area(partitionCoord+1, regPartir.y, regPartir.height, (regPartir.width-partitionCoord-1));
					Area calle = new Area(partitionCoord, regPartir.y, regPartir.height, 1);
					regiones.Enqueue(izq);
					regiones.Enqueue(der);
					calles.Enqueue(calle);
				}
				else{//Horizontal
					int lowrange = 	regPartir.height/2 - randomRange > 0 ? regPartir.height/2 - randomRange : 0;
					partitionCoord = Random.Range(lowrange, regPartir.height/2 + randomRange);
					Area abj = new Area(regPartir.x, regPartir.y, partitionCoord, regPartir.width);
					Area arr = new Area(regPartir.x, partitionCoord+1, (regPartir.height-partitionCoord-1), regPartir.width);
					Area calle = new Area(regPartir.x, partitionCoord, 1, regPartir.width);
					regiones.Enqueue(arr);
					regiones.Enqueue(abj);
					calles.Enqueue(calle);
				}
			}
			direction++;
		}
		/**Pintado de calles */
		while(calles.Count > 0){
			Area road = calles.Dequeue();

			print(road.x+","+road.y+"/"+road.height+","+road.width);
			if(road.height > road.width){//Vertical
	
				for(int i = road.y; i < (road.y+road.height); i++){
					Instantiate(vroad,new Vector3(road.x * mapRate,i * mapRate,0),Quaternion.identity);
				}
			}
			else{//Horizontal
				for(int i = road.x; i < (road.x+road.width); i++){
					Instantiate(hroad,new Vector3(i * mapRate,road.y * mapRate,0),Quaternion.identity);
				}
			}
		}
		/****/
		while(regiones.Count > 0){
			Area area = regiones.Dequeue();
			Debug.Log(area.x+","+area.y+"--"+area.width+","+area.height);
			//Instantiate(buildings[0],new Vector3(area.x * mapRate,area.y * mapRate,0),Quaternion.identity);
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
