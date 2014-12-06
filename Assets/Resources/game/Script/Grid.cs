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
		Transform container = new GameObject("Tiles").transform;
		container.parent = transform;

		tiles = new Tile[width, height];

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				tiles[x, y] = createTile(container, new Vector3(x, 0, y));
			}
		}

		createTrees(8);

		hero = createHero(new Vector3(width / 2, 0, height / 2));
	}


	private Tile createTile (Transform parent, Vector3 pos) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("game/Prefabs/Tile"));
		
		Tile tile = obj.GetComponent<Tile>();
		tile.init(this, parent, pos);

		return tile;
	}


	public Tile getTileAtPos (Vector3 pos) {
		if (pos.x < 0 || pos.x > width - 1 || pos.z < 0 || pos.z > height - 1) { return null; }
		return tiles[(int)pos.x, (int)pos.z];
	}


	private Ent createHero (Vector3 pos) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("game/Prefabs/Hero2"));
		
		Hero hero = obj.GetComponent<Hero>();

		hero.init(this, transform, pos);
		//hero.setColors(Color.black, Color.black, Color.black, Color.blue);

		return hero;
	}


	private void createTrees (int max) {
		Transform container = new GameObject("Trees").transform;
		container.parent = transform;

		for (int i = 0; i < max; i++) {
			GameObject tree = (GameObject)Instantiate(Resources.Load("game/Prefabs/Tree"));
			tree.name = "Tree" + i;
			tree.transform.parent = container.transform;
			tree.transform.localPosition = new Vector3(Random.Range(0, width), 0, Random.Range(0, height));
		
			Tile tile = getTileAtPos(tree.transform.localPosition);
			tile.setWalkable(false);
		}
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
		hero.crouch(true);
		//StartCoroutine(hero.startTargetState());
	}

	
	public void onTouchRelease (TouchEvent e) {
		/*if (hero.targeting) {
			hero.endTargetState();
			return;
		}*/

		//print ("release");
		hero.moveInSameDirection();
	}


	public void onTouchMove (TouchEvent e) {
		//print ("move");
	}


	public void onTouchSwipe (TouchEvent e) {
		//hero.endTargetState();

		//print ("swipe " + e.activeTouch.relativeDeltaPos + " " + e.activeTouch.getVelocity3d(Camera.main) * 0.1f);
		hero.moveInDirection(e.activeTouch.deltaPos);
	}
}
