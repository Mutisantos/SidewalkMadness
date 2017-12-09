using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

	public Tilemap map;

	public TileBase roadTile;
	public TileBase sidewalkTile;

	public BuildingTile[] buildingTiles;
	public int height;
	public int width;
	public int mainStreetDepth;

	public int minAreaSize;
	public int randomRange;	
	public bool finishedLoading;

	public bool initHorizontal;
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
					Area der = new Area(partitionCoord+regPartir.x, regPartir.y, regPartir.height, regPartir.width-partitionCoord,orientation.VERTICAL);
					regiones.Enqueue(der);
					regiones.Enqueue(izq);
				}
				else{//Horizontal
					Debug.Log("Padre Horizontal:"+regPartir.toString());
					int lowrange = 	regPartir.height/2 - randomRange > 0 ? regPartir.height/2 - randomRange : 0;
					int hirange = 	regPartir.height/2 + randomRange < height ? regPartir.height/2 + randomRange : height;
					partitionCoord = Random.Range(lowrange, hirange);
					Area abj = new Area(regPartir.x, regPartir.y, partitionCoord, regPartir.width,orientation.HORIZONTAL);
					Area arr = new Area(regPartir.x, partitionCoord+regPartir.y, (regPartir.height-partitionCoord), regPartir.width,orientation.HORIZONTAL);							
					regiones.Enqueue(abj);
					regiones.Enqueue(arr);
				}
			}
			direction++;
		}
		/****/
		while(regiones.Count > 0){
			Area area = regiones.Dequeue();
			PaintRegion(area);
			Debug.Log(area.toString());
		}
	}
	
	private void PaintRegion(Area region){
		Vector3Int brushPosition = new Vector3Int(region.x,region.y,0);
		for(int i = region.x * mapRate; i <= (region.x + region.width) * mapRate; i++){
			map.SetTile(new Vector3Int(i,region.y * mapRate,0),roadTile);			
			map.SetTile(new Vector3Int(i,(region.y+region.height) * mapRate,0),roadTile);
			// if(region.height > minAreaSize){
			// 	map.SetTile(new Vector3Int(i,(region.y) * mapRate - 1,0),roadTile);			
			// 	map.SetTile(new Vector3Int(i,(region.y+region.height) * mapRate - 1,0),roadTile);
			// }
		}
		for(int i = region.y * mapRate; i <= (region.y + region.height) * mapRate; i++){
			map.SetTile(new Vector3Int(region.x * mapRate,i,0),roadTile);
			map.SetTile(new Vector3Int((region.x+region.width) * mapRate,i,0),roadTile);
			// if(region.width > minAreaSize){
			// 	map.SetTile(new Vector3Int(region.x * mapRate - 1,i,0),roadTile);
			// 	map.SetTile(new Vector3Int((region.x+region.width) * mapRate -1,i,0),roadTile);
			// }
		}
	}

	private void PaintBuildings (Area region){

	}




}
