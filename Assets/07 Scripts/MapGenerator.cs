using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum orientation{HORIZONTAL, VERTICAL}
public struct Area{

	public int x;
	public int y;
	public int height;
	public int width;

	public orientation dir;

	public Area(int x, int y, int h, int w, orientation or){
		this.height = h;
		this.width = w;
		this.x = x;
		this.y = y;
		this.dir = or;
	}

	public string toString(){
		return ("("+x+","+y+")"+"--"+"("+height+","+width+")");
	}

}

public class MapGenerator : MonoBehaviour {

	public Building[] buildings;

	public Building initialBuilding;
	public Building targetBuilding;
	public Building vroad;
	public Building hroad;
	
	public int height;
	public int width;
	public int mainStreetDepth;
	public int randomRange;	
	public bool finishedLoading;

	public bool initHorizontal;
	public int mapRate;
	private int direction; //Par:Horizontal; Impar:Vertical
	//Building Side Size -> Building 
	private Dictionary<int, ArrayList> buildMap = new Dictionary<int, ArrayList>();

	void Start () {
		direction = 0;
		finishedLoading = false;
		this.generateMap();
	}
	
	private void orderBuildings(){
		foreach (Building b in buildings){
			
		}
	}

	private void generateMap(){
		Queue<Area> regiones = new Queue<Area>();
		Queue<Area> calles = new Queue<Area>();
		Area regTotal = new Area(0,0,height,width,orientation.HORIZONTAL);
		if(!initHorizontal){
			regTotal.dir = orientation.VERTICAL;
			direction = 1;
		}
		regiones.Enqueue(regTotal);
		//Generacion de regiones principales
		for(int i =0;i<mainStreetDepth;i++){
			for(int j = 0; j < Mathf.Pow(2,i);j++){
				Area regPartir = regiones.Dequeue();
				int partitionCoord = 0;
				if(direction%2==0){//Vertical
					int lowrange = 	regPartir.width/2 - randomRange >= 0 ? regPartir.width/2 - randomRange : 0;
					int hirange = 	regPartir.width/2 + randomRange < (regPartir.x + regPartir.width) ? regPartir.width/2 + randomRange : regPartir.width;
					partitionCoord = Random.Range(lowrange, hirange);					
					Area izq = new Area(regPartir.x, regPartir.y, regPartir.height, partitionCoord, orientation.VERTICAL);
					Area der = new Area(partitionCoord+regPartir.x+1, regPartir.y, regPartir.height, regPartir.width-partitionCoord-1,orientation.VERTICAL);
					Area calle = new Area(partitionCoord+regPartir.x, regPartir.y, regPartir.height, 1,orientation.VERTICAL);
					regiones.Enqueue(der);
					regiones.Enqueue(izq);
					calles.Enqueue(calle);
				}
				else{//Horizontal
					Debug.Log("Padre Horizontal:"+regPartir.toString());
					int lowrange = 	regPartir.height/2 - randomRange > 0 ? regPartir.height/2 - randomRange : 0;
					int hirange = 	regPartir.height/2 + randomRange < height ? regPartir.height/2 + randomRange : height;
										
					partitionCoord = Random.Range(lowrange, hirange);
					Debug.Log(lowrange+"-"+hirange+":"+partitionCoord);
					
					Area abj = new Area(regPartir.x, regPartir.y, partitionCoord, regPartir.width,orientation.HORIZONTAL);
					Area arr = new Area(regPartir.x, partitionCoord+regPartir.y+1, (regPartir.height-partitionCoord-1), regPartir.width,orientation.HORIZONTAL);
					Area calle = new Area(regPartir.x, partitionCoord+regPartir.y, 1, regPartir.width,orientation.HORIZONTAL);
					Debug.Log("Hijo abajo:"+arr.toString());
					Debug.Log("Hijo arriba:"+abj.toString());
					
					
					regiones.Enqueue(abj);
					regiones.Enqueue(arr);
					calles.Enqueue(calle);
				}
			}
			direction++;
		}
		/**Pintado de calles */
		while(calles.Count > 0){
			Area road = calles.Dequeue();			
			if(road.dir == orientation.VERTICAL){//Vertical
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
			Debug.Log(area.toString());
		}

		
	}
	
	private Area instantiateBuildings(Area original){
		Area restante = new Area(0,0,0,0,orientation.HORIZONTAL);

		return restante;
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
