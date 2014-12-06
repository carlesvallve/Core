using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private Grid grid;
	private bool walkable = true;


	public void init (Grid grid, Transform parent, Vector3 pos) {
		this.grid = grid;

		name = "Tile_" + pos.x + "_" + pos.z;
		transform.parent = parent;
		transform.localPosition = pos;
	}


	public void setWalkable (bool walkable) {
		this.walkable = walkable;
	}


	public bool getWalkable () {
		return walkable;
	}
}
