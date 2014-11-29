using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	private int width = 3;
	private int height = 3;

	private Tile[,] tiles;

	private Ent hero;

	public Cam cam;

	public TouchControls touchControls;
	public TouchLayer touchLayer;

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

		hero = createHero(new Vector3(0, 0, 1));
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


	// *****************************************************
	// Gestures
	// *****************************************************

	private void registerTouchEvents () {
		touchLayer = touchControls.getLayer("grid");
		touchLayer.onPress += onTouchPress;
		touchLayer.onRelease += onTouchRelease;
		touchLayer.onMove += onTouchMove;
		touchLayer.onSwipe += onTouchSwipe;
	}
	


	public void onTouchPress (TouchEvent e) {
		print ("press");
	}

	
	public void onTouchRelease (TouchEvent e) {
		print ("release");
	}


	public void onTouchMove (TouchEvent e) {
		print ("move");
	}


	public void onTouchSwipe (TouchEvent e) {
		print ("swipe");
	}
}
