using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private Grid grid;


	public void init (Grid grid, Vector3 pos) {
		this.grid = grid;

		transform.parent = grid.transform;
		transform.localPosition = pos;
	}
}
