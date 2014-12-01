using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Ent : MonoBehaviour {

	private Grid grid;
	private GameObject body;

	private Vector3 stepPos;
	private Vector2 delta = new Vector2(0, 1);

	public float speed = 0.1f;
	public Ease easing = Ease.Linear; //Linear; InOutQuad; InOutSine;

	private bool moving = false;
	private bool goingToMove = false;
	

	public void init (Grid grid, Vector3 pos) {
		this.grid = grid;
		this.body = transform.Find("Body").gameObject;

		transform.parent = grid.transform;
		transform.localPosition = pos;
		body.transform.eulerAngles = new Vector3(0, 180, 0);

		stepPos = transform.localPosition;

		grid.cam.target = transform;
	}


	public void reset () {
		// TODO: class properties are gone when triggering this from a button...
		//print (">>> " + this + " >>> " + this.body);
		//return;

		transform.localPosition = new Vector3(grid.width / 2, 0, grid.height / 2);

		body.rigidbody.velocity = Vector3.zero;
		body.rigidbody.angularVelocity = Vector3.zero;
		body.transform.localPosition = Vector3.zero;
		body.transform.localRotation = Quaternion.identity;
	}


	public void moveInSameDirection () {
		moveInDirection(delta);
	}


	public void moveInDirection(Vector2 delta) {
		if (moving) {
			goingToMove = true;
			return;
		}

		moving = true;

		// get direction
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			delta.x = delta.x > 0 ? 1 : -1;
			delta.y = 0;
		} else {
			delta.y = delta.y > 0 ? 1 : -1;
			delta.x = 0;
		}

		// record delta
		this.delta = delta;

		// get body rotation
		float rot = 0;
		if (delta.y == 1) {
			rot = 180;
		} else if (delta.y == -1) {
			rot = 0;
		} else if (delta.x == 1) {
			rot = -90;
		} else if (delta.x == -1) {
			rot = 90;
		}

		// rotate body
		body.transform.DOLocalRotate(new Vector3(0, rot, 0), 0.05f).SetEase(Ease.InOutSine);

		// get new position
		stepPos = new Vector3(Mathf.Round(stepPos.x) + delta.x, 0, Mathf.Round(stepPos.z) + delta.y);

		// move to new position
		moveTo(stepPos, speed);
	}


	private void moveTo(Vector3 pos, float duration) {
		Audio.play("audio/MarioJump", 0.75f, Random.Range(1.0f, 3.0f));

		// move hero
		transform.DOLocalMove(pos, duration)
			.SetEase(easing)
			.SetLoops(1)
			.OnComplete(endMove);

		transform.DOScale(new Vector3(1, 1, 1), duration + 0.1f)
		.SetEase(Ease.OutQuad);

		// reset box physics
		body.rigidbody.velocity = Vector3.zero;
		body.rigidbody.angularVelocity = Vector3.zero;

		// make box jump
		body.rigidbody.AddForce( new Vector3(0, 7f * body.rigidbody.mass, 0), ForceMode.Impulse);
	}


	private void endMove () {
		StartCoroutine(setNextMove());
	}


	private IEnumerator setNextMove() {
		yield return new WaitForSeconds(0.1f);
		
		moving = false; 

		if (goingToMove) { 
			moveInSameDirection(); 
			goingToMove = false;
		}
	}

	public void crouch () {
		transform.DOScale(new Vector3(1.2f, 0.75f, 1.1f), 0.1f)
		.SetEase(Ease.OutQuad);
	}
}
