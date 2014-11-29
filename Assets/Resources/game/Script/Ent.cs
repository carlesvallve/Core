using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Ent : MonoBehaviour {

	private Grid grid;
	

	public void init (Grid grid, Vector3 pos) {
		this.grid = grid;

		transform.parent = grid.transform;
		transform.localPosition = pos;

		grid.cam.target = transform;

		//moveTo(new Vector3(1,0,1), 0.5f);
	}


	public void moveInDirection(Vector2 delta) {
		// get direction
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			delta.x = delta.x > 0 ? 1 : -1;
			delta.y = 0;
		} else {
			delta.y = delta.y > 0 ? 1 : -1;
			delta.x = 0;
		}

		//print (delta.x + " " + delta.y);

		// get new position
		Vector3 pos = new Vector3(
			Mathf.Round(transform.position.x) + delta.x, 
			0, 
			Mathf.Round(transform.position.z) + delta.y
		);

		// move
		moveTo(pos, 0.2f);
	}


	private void moveTo(Vector3 pos, float duration) {
		transform.DOMove(pos, duration)
			.SetEase(Ease.InOutSine)
			.SetLoops(1)
			.OnComplete(endMove);
	}


	private void endMove () {
		//print ("end move");
	}
}
