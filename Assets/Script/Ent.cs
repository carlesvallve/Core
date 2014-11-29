using UnityEngine;
using System.Collections;

public class Ent : MonoBehaviour {

	private Grid grid;

	public void init (Grid grid, Vector3 pos) {
		this.grid = grid;

		transform.parent = grid.transform;
		transform.localPosition = pos;

		grid.cam.target = transform;
	}
}
