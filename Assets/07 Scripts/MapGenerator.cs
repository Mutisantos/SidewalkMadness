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
	
	public int getArea(){
		return height * width;
	}

}

/**Script for generating   */
public class MapGenerator : MonoBehaviour {

	public Tilemap roadMaps;
	public Tilemap buildingMap;
	public Tilemap propsMap;
	

	public TileBase roadTile;
	public TileBase sidewalkTile;

	public BuildingTile[] buildingTiles;

	public GameObject[] sidewalkEnemyPrefabs;
	public GameObject[] roadEnemyPrefabs;
	
	public int height;
	public int width;
	public int mainStreetDepth;
	public int minAreaSize;
	public int randomRange;	
	public bool finishedLoading;

	public bool initHorizontal;
	public int mapRate;
	private int direction; //Even:Horizontal; Odd:Vertical

	void Start () {
		direction = 0;
		finishedLoading = false;
		generateMap();
	}

	private Queue<Area> generateRegions(int mainStreetDepth, Area originalArea, int direction, int minArea){
		Queue<Area> regions = new Queue<Area>();
		regions.Enqueue(originalArea);
		for(int i =0;i<mainStreetDepth;i++){
			for(int j = 0; j < Mathf.Pow(2,i);j++){
				Area regPartir = regions.Dequeue();
				int partitionCoord = 0;
				Debug.Log(regPartir.toString());
				if(regPartir.height >= minArea && regPartir.width >= minArea){
					if(direction%2==0){//Vertical
						int lowrange = 	regPartir.width/2 - randomRange >= 0 ? regPartir.width/2 - randomRange : 0;
						int hirange = 	regPartir.width/2 + randomRange < (regPartir.x + regPartir.width) ? regPartir.width/2 + randomRange : regPartir.width;
						partitionCoord = Random.Range(lowrange, hirange);					
						Area izq = new Area(regPartir.x, regPartir.y, regPartir.height, partitionCoord, orientation.VERTICAL);
						Area der = new Area(partitionCoord+regPartir.x, regPartir.y, regPartir.height, regPartir.width-partitionCoord,orientation.VERTICAL);
						regions.Enqueue(der);
						regions.Enqueue(izq);
					}
					else{//Horizontal
						int lowrange = 	regPartir.height/2 - randomRange > 0 ? regPartir.height/2 - randomRange : 0;
						int hirange = 	regPartir.height/2 + randomRange < height ? regPartir.height/2 + randomRange : height;
						partitionCoord = Random.Range(lowrange, hirange);
						Area abj = new Area(regPartir.x, regPartir.y, partitionCoord, regPartir.width,orientation.HORIZONTAL);
						Area arr = new Area(regPartir.x, partitionCoord+regPartir.y, (regPartir.height-partitionCoord), regPartir.width,orientation.HORIZONTAL);							
						regions.Enqueue(abj);
						regions.Enqueue(arr);
					}
				}
				else{
					regions.Enqueue(regPartir);
				}
			}
			direction++;
		}

		return regions;
	}
	
	private void generateMap(){
		roadMaps.ClearAllTiles();
		buildingMap.ClearAllTiles();
		propsMap.ClearAllTiles();

		Queue<Area> regiones = new Queue<Area>();
		///-- Split the whole roadMaps in regions - generate roads
		Area regTotal = new Area(0,0,height,width,orientation.HORIZONTAL);
		PaintStreets(new Area(0,0,height,width,orientation.HORIZONTAL));
		if(!initHorizontal){
			regTotal.dir = orientation.VERTICAL;
			direction = 1;
		}
		regiones = generateRegions(mainStreetDepth,regTotal,direction,minAreaSize);
		while(regiones.Count > 0){
			Area area = regiones.Dequeue();
			PaintStreets(area);
		}
	}
	
	/** */
	private void PaintStreets(Area region){
		Vector3Int brushPosition = new Vector3Int(region.x,region.y,0);
		
		for(int i = region.x * mapRate; i <= (region.x + region.width) * mapRate; i++){
			brushPosition.Set(i,region.y * mapRate,0);
			roadMaps.SetTile(brushPosition,roadTile);		
			brushPosition.Set(i,(region.y+region.height) * mapRate,0);
			roadMaps.SetTile(brushPosition,roadTile);

		}
		for(int i = region.y * mapRate; i <= (region.y + region.height) * mapRate; i++){
			brushPosition.Set(region.x * mapRate,i,0);
			roadMaps.SetTile(brushPosition,roadTile);		
			brushPosition.Set((region.x+region.width) * mapRate,i,0);
			roadMaps.SetTile(brushPosition,roadTile);

		}
		// if(region.height == 1 && region.width > 1)
		// 	brushPosition.Set(region.x + 1,region.y,0);		
		// else if(region.width == 1 && region.height > 1)
		// 	brushPosition.Set(region.x ,region.y + 1,0);
		// else
		
		brushPosition.x = brushPosition.x -1;
		brushPosition.y = brushPosition.y -1;
		
		roadMaps.FloodFill(brushPosition,sidewalkTile);
	}

	private void PaintBuildings (Area region){

	}




}
