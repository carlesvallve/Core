using UnityEngine;
using System.Collections;


public class Enemy : Ent {

	public override void init (Grid grid, Transform parent, Vector3 pos) {
		// initialize ent
		base.init(grid, parent, pos);

		name = "Enemy";
		body.transform.eulerAngles = new Vector3(0, 180, 0);
		grid.cam.target = transform;
	}
}
