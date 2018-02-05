using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum buildingTypeEnum{BUILDING, PARK, FENCED, PARKLOT, WASTE}

//Building type: Walls, Rooftop, Items and billboards
//Park Type: Rooftop, Items
//Fenced: Rooftop, Items, Billboards
//Parking Lot: Rooftop, items
public class BuildingTile: MonoBehaviour {

	public TileBase wallTile;
	public TileBase rooftopTile;
	public TileBase itemsTile;
	public TileBase billboardTile;
	public buildingTypeEnum type;
	
}
