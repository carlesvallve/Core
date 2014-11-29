using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Ent : MonoBehaviour {

	private Grid grid;
	public GameObject box;
	

	public void init (Grid grid, Vector3 pos) {
		this.grid = grid;
		this.box = transform.Find("Box").gameObject;

		transform.parent = grid.transform;
		transform.localPosition = pos;

		grid.cam.target = transform;
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
		Audio.play("audio/MarioJump", 0.75f, Random.Range(0.5f, 2.0f));

		// move hero
		transform.DOLocalMove(pos, duration)
			.SetEase(Ease.InOutSine)
			.SetLoops(1)
			.OnComplete(endMove);

		// make box jump
		box.rigidbody.AddForce( new Vector3(0, 8 * box.rigidbody.mass, 0), ForceMode.Impulse);
	}


	private void endMove () {
		//Audio.play("audio/Step", 0.5f, 2.0f);
	}
}
