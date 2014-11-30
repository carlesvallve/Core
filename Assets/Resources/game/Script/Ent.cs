using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Ent : MonoBehaviour {

	private Grid grid;
	public GameObject box;

	private Vector3 stepPos;

	public float speed = 0.2f;
	public Ease easing = Ease.Linear; //InOutQuad; //InOutSine;
	

	public void init (Grid grid, Vector3 pos) {
		this.grid = grid;
		this.box = transform.Find("Box").gameObject;

		transform.parent = grid.transform;
		transform.localPosition = pos;

		stepPos = transform.localPosition;

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
		stepPos = new Vector3(
			Mathf.Round(stepPos.x) + delta.x, 
			0, 
			Mathf.Round(stepPos.z) + delta.y
		);

		// move
		moveTo(stepPos, speed);
	}


	private void moveTo(Vector3 pos, float duration) {
		Audio.play("audio/MarioJump", 0.75f, Random.Range(1.0f, 3.0f));

		// move hero
		transform.DOLocalMove(pos, duration)
			.SetEase(easing)
			.SetLoops(1)
			.OnComplete(endMove);

		// make box jump
		box.rigidbody.AddForce( new Vector3(0, 8f * box.rigidbody.mass, 0), ForceMode.Impulse);
	}


	private void endMove () {
		//Audio.play("audio/Step", 0.5f, 2.0f);
	}
}
