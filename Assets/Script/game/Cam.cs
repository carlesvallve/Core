using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {

	public float interval = 5.0f; // bigger numbers increase interpolation speed

	public Transform target;
	public Vector3 center = new Vector3(0, 0, 0);
	public Vector3 angle = new Vector3(45, 45, 0);
	public float distance = 10;

	public bool isOrtographic;

	private bool initialized = false;


	public void init () {
		if (!target) {
			throw new System.Exception("Camera target is required!");
		}

		Camera.main.orthographic = isOrtographic;


		initialized = true;

		setCameraOnTarget(0);
	}


	void Update () {
		if (!initialized) { return; }

		setCameraOnTarget(interval);
	}


	private void setCameraOnTarget (float interval) {
		Quaternion rotation = Quaternion.Euler(angle.x, angle.y, 0);

		Vector3 position = rotation *
			new Vector3(0, 0, -distance) +
			target.position + center;

		if (interval > 0) {
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * interval);
			transform.position = Vector3.Slerp(transform.position, position, Time.deltaTime * interval);
		} else {
			transform.rotation = rotation;
			transform.position = position;
		}
		
		Camera.main.orthographic = isOrtographic;
		if(isOrtographic) {
			Camera.main.orthographicSize = distance;
			//Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, distance, Time.time * interval);
		}
	}
}
