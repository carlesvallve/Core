using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Ent : MonoBehaviour {

	protected Grid grid;
	protected GameObject body;
	protected GameObject head;
	protected GameObject hair;
	protected GameObject torax;
	protected GameObject armR;
	protected GameObject armL;
	protected GameObject subArmR;
	protected GameObject subArmL;
	protected GameObject hip;
	protected GameObject legR;
	protected GameObject legL;
	protected GameObject gun;

	protected Vector3 stepPos;
	protected Vector2 delta = new Vector2(0, 1);
	protected bool moving = false;
	protected bool goingToMove = false;
	public bool targeting = false;

	public float speed = 0.1f; 			// speed of movement
	public float rotationSpeed = 0.05f;  // speed of rotation
	public float reaction = 0; //0.1f;  // delay time between moves
	public float jumpForce = 7f;		// jump impulse force


	

	
	public virtual void init (Grid grid, Transform parent, Vector3 pos) {
		this.grid = grid;
		
		getBodyParts();
		//setColors();

		name = "Hero";
		transform.parent = parent;
		transform.localPosition = pos;
		body.transform.eulerAngles = new Vector3(0, 180, 0);

		stepPos = transform.localPosition;

		grid.cam.target = transform;
	}


	private void getBodyParts () {
		body = transform.Find("Body").gameObject;

		head = transform.Find("Body/Skin/Head").gameObject;
		hair = transform.Find("Body/Skin/Head/Hair").gameObject;
		
		torax = transform.Find("Body/Skin/Torax").gameObject;
		armR = transform.Find("Body/Skin/Torax/ArmR").gameObject;
		armL = transform.Find("Body/Skin/Torax/ArmL").gameObject;
		subArmR = transform.Find("Body/Skin/Torax/ArmR/SubArmR").gameObject;
		subArmL = transform.Find("Body/Skin/Torax/ArmL/SubArmL").gameObject;

		hip = transform.Find("Body/Skin/Hip").gameObject;
		legR = transform.Find("Body/Skin/Hip/LegR").gameObject;
		legL = transform.Find("Body/Skin/Hip/LegL").gameObject;

		gun = transform.Find("Body/Skin/Torax/ArmR/SubArmR/Gun").gameObject;
	}


	public void setColors (Color haircolor, Color skincolor, Color shirtcolor, Color pantscolor) {
		hair.renderer.material.color = haircolor;
		
		head.renderer.material.color = skincolor;
		subArmR.renderer.material.color = skincolor;
		subArmL.renderer.material.color = skincolor;

		torax.renderer.material.color = shirtcolor;
		armR.renderer.material.color = shirtcolor;
		armL.renderer.material.color = shirtcolor;

		hip.renderer.material.color = pantscolor;
		legR.renderer.material.color = pantscolor;
		legL.renderer.material.color = pantscolor;
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
		// uncrouch
		uncrouch(duration + 0.1f);

		// move hero
		transform.DOLocalMove(pos, duration)
			.SetEase(Ease.Linear)
			.SetLoops(1)
			.OnComplete(endMove);

		// update step position
		stepPos = pos;
	}


	private void jump () {
		// reset box physics
		body.rigidbody.velocity = Vector3.zero;
		body.rigidbody.angularVelocity = Vector3.zero;

		// make box jump
		//Audio.play("audio/Squish", 0.5f, Random.Range(1.0f, 2.0f));
		body.rigidbody.AddForce( new Vector3(0, jumpForce * body.rigidbody.mass, 0), ForceMode.Impulse);
	}


	private void endMove () {
		Audio.play("audio/Squish", 0.5f, Random.Range(1.0f, 2.0f));
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


	private void uncrouch (float duration) {
		transform.DOScale(new Vector3(1, 1, 1), duration)
		.SetEase(Ease.OutQuad);
	}


	/*public IEnumerator startTargetState () {
		targeting = true;

		yield return new WaitForSeconds(0.5f);

		if (!targeting) {
			yield break;
		}

		//armR.transform.localEulerAngles = new Vector3(90, 0, 0);
		uncrouch(0.1f);

		armR.transform.DOLocalRotate(new Vector3(90, 0, 0), rotationSpeed)
			.SetEase(Ease.InOutSine);

		//gun.SetActive(true);
	}

	public void endTargetState () {
		//armR.transform.localEulerAngles = Vector3.zero;
		targeting = false;

		armR.transform.DOLocalRotate(new Vector3(0, 0, 0), rotationSpeed)
			.SetEase(Ease.InOutSine);
	}*/
}
