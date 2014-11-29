using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

	private float fadeDuration = 0.5f;

	void Start () {
		ScreenFader.FadeIn (fadeDuration);
	}


	public void startGame () {
		print ("Start Game!");
		//Application.LoadLevel("Game");
		StartCoroutine(gotoLevel("Game", fadeDuration));
	}


	public void exitGame () {
		print ("Exit Game!");
		//Application.LoadLevel("Intro");
		StartCoroutine(gotoLevel("Intro", fadeDuration));
	}


	private IEnumerator gotoLevel (string levelName, float duration) {
		ScreenFader.FadeOut (duration);

		yield return new WaitForSeconds(duration);

		Application.LoadLevel(levelName);

		
	}
}
