using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public int width = 9;
	public int height = 9;

	private Tile[,] tiles;

	private Ent hero;

	public Cam cam;


	void Awake () {
		touchControls = GameObject.Find("TouchControls").GetComponent<TouchControls>();

		cam = Camera.main.GetComponent<Cam>();

		init();

		cam.init();
	}


	void Start () {
		registerTouchEvents();
	}


	private void init() {
		tiles = new Tile[width, height];

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				tiles[x, y] = createTile(new Vector3(x, 0, y));
			}
		}

		hero = createHero(new Vector3(width / 2, 0, height / 2));
		print (hero);
	}


	private Tile createTile (Vector3 pos) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("game/Prefabs/Tile"));
		
		Tile tile = obj.GetComponent<Tile>();
		tile.init(this, pos);

		return tile;
	}

	private Ent createHero (Vector3 pos) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("game/Prefabs/Hero"));
		
		Ent hero = obj.GetComponent<Ent>();
		hero.init(this, pos);

		return hero;
	}


	public void resetHero () {
		// TODO: properties are gone when triggering this from a button...
		print (this + " >>> " + this.hero + " >>> " + hero);
		return;

		hero.transform.localPosition = new Vector3(width / 2, 0, height / 2);

		hero.box.rigidbody.velocity = Vector3.zero;
		hero.box.rigidbody.angularVelocity = Vector3.zero;
		 
		hero.box.transform.localPosition = Vector3.zero; // or whatever
		hero.box.transform.localRotation = Quaternion.identity;
	}


	// *****************************************************
	// Gestures
	// *****************************************************

	public TouchControls touchControls;
	public TouchLayer touchLayer;


	private void registerTouchEvents () {
		touchLayer = touchControls.getLayer("grid");
		touchLayer.onPress += onTouchPress;
		touchLayer.onRelease += onTouchRelease;
		touchLayer.onMove += onTouchMove;
		touchLayer.onSwipe += onTouchSwipe;
	}
	

	public void onTouchPress (TouchEvent e) {
		//print ("press " + e.activeTouch.getPos3d(Camera.main));
	}

	
	public void onTouchRelease (TouchEvent e) {
		//print ("release");
	}


	public void onTouchMove (TouchEvent e) {
		//print ("move");
	}


	public void onTouchSwipe (TouchEvent e) {
		//print ("swipe " + e.activeTouch.relativeDeltaPos + " " + e.activeTouch.getVelocity3d(Camera.main) * 0.1f);
	
		hero.moveInDirection(e.activeTouch.deltaPos);
	}
}
