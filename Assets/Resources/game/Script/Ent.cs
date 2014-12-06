using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Ent : MonoBehaviour {

	protected Grid grid;
	protected GameObject body;

	protected Vector3 stepPos;
	protected Vector2 delta = new Vector2(0, 1);
	protected bool moving = false;
	protected bool goingToMove = false;

	public float speed = 0.1f; 			// speed of movement
	public float rotationSpeed = 0.05f;  // speed of rotation
	public float reaction = 0; //0.1f;  // delay time between moves
	public float jumpForce = 7f;		// jump impulse force

	
	public virtual void init (Grid grid, Transform parent, Vector3 pos) {
		this.grid = grid;
		this.body = transform.Find("Body").gameObject;

		name = "Hero";
		transform.parent = parent;
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


	public void moveInDirection(Vector2 _delta) {
		if (moving) {
			goingToMove = true;
			return;
		}

		moving = true;

		// set final delta
		setDelta(_delta);

		// make body rotate
		rotateTo(delta);

		// make body jump
		jump();

		// get new position
		Vector3 newPos = new Vector3(
			Mathf.Round(stepPos.x) + delta.x, 0, Mathf.Round(stepPos.z) + delta.y
		);

		// check next tile walkability
		Tile tile = grid.getTileAtPos(newPos);
		if (!tile || !tile.getWalkable()) {
			newPos = transform.position;
		}

		// move to new position
		moveTo(newPos, speed);
	}


	private void setDelta(Vector2 delta) {
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			delta.x = delta.x > 0 ? 1 : -1;
			delta.y = 0;
		} else {
			delta.y = delta.y > 0 ? 1 : -1;
			delta.x = 0;
		}

		this.delta = delta;
	}


	private void rotateTo (Vector2 delta) {
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

		body.transform.DOLocalRotate(new Vector3(0, rot, 0), rotationSpeed)
			.SetEase(Ease.InOutSine);
	}


	private void moveTo(Vector3 pos, float duration) {
		// move hero
		transform.DOLocalMove(pos, duration)
			.SetEase(Ease.Linear)
			.SetLoops(1)
			.OnComplete(endMove);

		transform.DOScale(new Vector3(1, 1, 1), duration + 0.1f)
		.SetEase(Ease.OutQuad);

		// update step position
		stepPos = pos;
	}


	private void jump () {
		// reset box physics
		body.rigidbody.velocity = Vector3.zero;
		body.rigidbody.angularVelocity = Vector3.zero;

		// make box jump
		Audio.play("audio/MarioJump", 0.5f, Random.Range(2.0f, 3.0f));
		body.rigidbody.AddForce( new Vector3(0, jumpForce * body.rigidbody.mass, 0), ForceMode.Impulse);
	}


	private void endMove () {
		StartCoroutine(setNextMove());
	}


	private IEnumerator setNextMove() {
		bool _goingToMove = goingToMove;
		if (goingToMove) { 
			crouch(false); 
			goingToMove = false;
		}

		yield return new WaitForSeconds(reaction);
		
		moving = false; 

		if (_goingToMove) { 
			moveInSameDirection(); 
		}
	}


	public void crouch (bool escapeIfMoving) {
		if (escapeIfMoving && moving) { return; }

		transform.DOScale(new Vector3(1.2f, 0.8f, 1.0f), 0.1f)
		.SetEase(Ease.OutQuad);
	}
}
