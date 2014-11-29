using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

	private float fadeDuration = 0.5f;

	void Start () {
		ScreenFader.FadeIn (fadeDuration);
	}


	public void startGame () {
		Audio.play("audio/typekey", 1.0f, 1.0f);
		StartCoroutine(gotoLevel("Game", fadeDuration));
	}


	public void exitGame () {
		Audio.play("audio/typekey", 1.0f, 1.0f);
		StartCoroutine(gotoLevel("Intro", fadeDuration));
	}


	private IEnumerator gotoLevel (string levelName, float duration) {
		ScreenFader.FadeOut (duration);

		yield return new WaitForSeconds(duration);

		Application.LoadLevel(levelName);
	}
}
