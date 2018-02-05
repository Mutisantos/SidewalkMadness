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
	public int depth;
	public orientation dir;
	public buildingTypeEnum areaType;

	public Area(int x, int y, int h, int w, orientation or,int depth){
		this.height = h;
		this.width = w;
		this.x = x;
		this.y = y;
		this.dir = or;
		this.areaType = buildingTypeEnum.BUILDING;
		this.depth = depth;
	}

	public Area(int x, int y, int h, int w, orientation or, buildingTypeEnum areaType){
		this.height = h;
		this.width = w;
		this.x = x;
		this.y = y;
		this.dir = or;
		this.areaType = areaType;
		this.depth = 1;
	}

	public string toString(){
		return ("("+x+","+y+")"+"--"+"("+height+","+width+")");
	}
	
	public int getArea(){
		return height * width;
	}

}

/**Script for generating the game map using tilemaps */
public class MapGenerator : MonoBehaviour {

	public Tilemap roadMaps;
	public Tilemap buildingMap;
	public Tilemap propsMap;
	public TileBase roadTile;
	public BuildingTile[] groundTiles;
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
	private Dictionary<buildingTypeEnum,List<BuildingTile>> contentTiles;
	//Los circuitos se guardan como areas para añadir vehiculos/peatones que se muevan por las calles
	//Los circuitos se componen de 4 puntos
	private Queue<Area> roads;

	
	void Start () {
		direction = 0;
		finishedLoading = false;
		contentTiles = new Dictionary<buildingTypeEnum,List<BuildingTile>>();
		roads = new Queue<Area>();
		groupBuildingsByType();
		generateMap();
	}
	
	private void generateMap(){
		roadMaps.ClearAllTiles();
		buildingMap.ClearAllTiles();
		propsMap.ClearAllTiles();

		Queue<Area> regiones = new Queue<Area>();		
		///-- Split the whole roadMaps in regions - generate roads
		Area regTotal = new Area(0,0,height,width,orientation.HORIZONTAL,1);
		PaintStreets(new Area(0,0,height,width,orientation.HORIZONTAL,1));
		if(!initHorizontal){
			regTotal.dir = orientation.VERTICAL;
			direction = 1;
		}
		regiones = generateRegions(mainStreetDepth,regTotal,direction,minAreaSize);
		createRoadEnemies();
		while(regiones.Count > 0){
			Area area = regiones.Dequeue();
			int index = Random.Range(0, groundTiles.Length);
			area.areaType = (buildingTypeEnum)index;
			PaintStreets(area);
			//Validar el tamaño de la región si permite hacer edificios siquiera.
			if(validSplitRegion(area,3,3)){
				Queue<Area> buildingAreas = generateRegions(Random.Range(1, 2),area,direction,2);
			}
		}
	}
	
	private void createRoadEnemies(){
		while(roads.Count > 0){
			Area road = roads.Dequeue();
			List<Transform> waypoints = new List<Transform>();
			GameObject location = new GameObject();
			//Modos de circuito (rectangular, L, linear)
			int mode = Random.Range(0,0);
			switch(mode){
				//Rectangular
				case(0):location.transform.Translate(road.x,road.y,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x,road.y+road.height,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x+road.width,road.y+road.height,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x+road.width,road.y,0);
						waypoints.Add(location.transform);
						break;
				//L inferior
				case(1):location.transform.Translate(road.x,road.y,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x,road.y+road.height,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x,road.y,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x+road.width,road.y,0);
						waypoints.Add(location.transform);
						break;
				//L Superior
				case(2):location.transform.Translate(road.x+road.width,road.y+road.height,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x,road.y+road.height,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x+road.width,road.y+road.height,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x+road.width,road.y,0);
						waypoints.Add(location.transform);
						break;	
				//Vertical
				case(3):location.transform.Translate(road.x,road.y+road.height,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x,road.y,0);
						waypoints.Add(location.transform);
						break;	
				//Horizontal
				case(4):location.transform.Translate(road.x+road.width,road.y,0);
						waypoints.Add(location.transform);
						location = new GameObject();
						location.transform.Translate(road.x,road.y,0);
						waypoints.Add(location.transform);
						break;	
			}
			int startingPoint = Random.Range(0, waypoints.Count);
			int chosenEnemy = Random.Range(0,roadEnemyPrefabs.Length);
			GameObject goEnemy = Instantiate(roadEnemyPrefabs[chosenEnemy],waypoints[startingPoint].position,Quaternion.identity);
			goEnemy.GetComponent<EnemyPatrolMove>().setWaypoints(waypoints);

		}
	}
	private Queue<Area> generateRegions(int mainStreetDepth, Area originalArea, int direction, int minArea){
		Queue<Area> regions = new Queue<Area>();
		regions.Enqueue(originalArea);
		for(int i =0;i<mainStreetDepth;i++){
			for(int j = 0; j < Mathf.Pow(2,i);j++){
				Area regPartir = regions.Dequeue();
				roads.Enqueue(regPartir);
				int partitionCoord = 0;
				Debug.Log(regPartir.toString());
				if(regPartir.height >= minArea && regPartir.width >= minArea){
					if(direction%2==0){//Vertical
						int lowrange = 	regPartir.width/2 - randomRange >= 0 ? regPartir.width/2 - randomRange : 0;
						int hirange = 	regPartir.width/2 + randomRange < (regPartir.x + regPartir.width) ? regPartir.width/2 + randomRange : regPartir.width;
						partitionCoord = Random.Range(lowrange, hirange);					
						Area izq = new Area(regPartir.x, regPartir.y, regPartir.height, partitionCoord, orientation.VERTICAL, direction);
						Area der = new Area(partitionCoord+regPartir.x, regPartir.y, regPartir.height, regPartir.width-partitionCoord,orientation.VERTICAL,direction);
						regions.Enqueue(der);
						regions.Enqueue(izq);
					}
					else{//Horizontal
						int lowrange = 	regPartir.height/2 - randomRange > 0 ? regPartir.height/2 - randomRange : 0;
						int hirange = 	regPartir.height/2 + randomRange < height ? regPartir.height/2 + randomRange : height;
						partitionCoord = Random.Range(lowrange, hirange);
						Area abj = new Area(regPartir.x, regPartir.y, partitionCoord, regPartir.width,orientation.HORIZONTAL,direction);
						Area arr = new Area(regPartir.x, partitionCoord+regPartir.y, (regPartir.height-partitionCoord), regPartir.width,orientation.HORIZONTAL,direction);							
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

	/**Metodo que llena el diccionario de Building Types segun el tipo de Tile: BuildingType.buildingTypeEnum */
	private void groupBuildingsByType(){
		foreach (BuildingTile bt in buildingTiles){
			Debug.Log(bt.type);
			if(!contentTiles.ContainsKey(bt.type)){
				List<BuildingTile> newTiles = new List<BuildingTile>();
				newTiles.Add(bt);
				contentTiles.Add(bt.type,newTiles);
			}
			else{
				List<BuildingTile> tileList = contentTiles[bt.type];
				tileList.Add(bt);
				contentTiles.Remove(bt.type);
				contentTiles.Add(bt.type,tileList);
			}
		}
	}
	/**Metodo para validar el tamaño de las regiones para la generacion de edificios, vallas, arboles, etc. */
	/**Por lo general las regiones necesitan tamaños minimos de 2x3 (2 muros y 1 techo) */
	private bool validSplitRegion(Area region, int minLenght, int minWidth){
		bool validRegion = false;
		if(region.x > minWidth && region.y > minLenght){
			validRegion = true;
		}
		return validRegion;
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
		brushPosition.x = brushPosition.x -1;
		brushPosition.y = brushPosition.y -1;	
		roadMaps.FloodFill(brushPosition,groundTiles[(int)region.areaType].wallTile);
	}

	private void PaintBuildings (Area region){

	}




}
