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

		moveTo(new Vector3(1,0,1), 0.5f);
	}


	private void moveTo(Vector3 pos, float duration) {
		transform.DOMove(pos, duration)
			.SetEase(Ease.InOutQuint)
			.SetLoops(1)
			.OnComplete(endMove);
	}


	private void endMove () {
		print ("end move");
	}
}
