using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	private int width = 3;
	private int height = 3;

	private Tile[,] tiles;

	private Ent hero;

	public Cam cam;

	void Awake () {
		cam = Camera.main.GetComponent<Cam>();

		init();

		cam.init();
	}


	private void init() {
		tiles = new Tile[width, height];

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				tiles[x, y] = createTile(new Vector3(x, 0, y));
			}
		}

		hero = createHero(new Vector3(width / 2, 0, height / 2));
	}
	
	
	private Tile createTile (Vector3 pos) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("game/grid/Tile"));
		
		Tile tile = obj.GetComponent<Tile>();
		tile.init(this, pos);

		return tile;
	}

	private Ent createHero (Vector3 pos) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("game/grid/Hero"));
		
		Ent hero = obj.GetComponent<Ent>();
		hero.init(this, pos);

		return hero;
	}
}
