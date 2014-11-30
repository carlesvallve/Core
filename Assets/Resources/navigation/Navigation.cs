using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class Navigation : MonoBehaviour {

	public float fadeDuration = 0.5f;
	public int FPS = 60;
	public bool showFPS = false;

	private int tframe = 0;

	private Image fadeImage;
	private Text fpsText;
	

    // Initialization

    void Awake () {
		// get elements
		Transform canvas = transform.Find("Canvas");
		fadeImage = canvas.GetComponent<Image>();
		fadeImage.enabled = false;
		fpsText = canvas.Find("Fps").GetComponent<Text>();
		fpsText.enabled = showFPS;

		Application.targetFrameRate = FPS;

		// fade in
		fadeIn(fadeDuration);
	}


	// Button Handlers

	public void pressButton () {
		Audio.play("audio/click", 1.0f, 1.0f);
	}


	public void releaseButton () {
		Audio.play("audio/typekey", 1.0f, 1.0f);
	}


	// Navigation Handlers

	public void gotoScene (string sceneName) {
		fadeOut(fadeDuration, sceneName);
	}


	private void loadScene (string sceneName) {
		if (sceneName == null) { return; }
		Application.LoadLevel(sceneName);
	}


	// Screen Fader

	private void fadeIn (float duration) {
		fadeImage.enabled = true;
		fadeImage.color = new Color(0,0,0,1);
		DOTween.ToAlpha(()=> fadeImage.color, x => fadeImage.color = x, 0, duration);
	}


	private void fadeOut (float duration, string sceneName) {
		fadeImage.enabled = true;
		fadeImage.color = new Color(0,0,0,0);

		DOTween.ToAlpha(()=> fadeImage.color, x => fadeImage.color = x, 1, duration)
			.OnComplete(()=>loadScene(sceneName));
	}


	// FPS Counter

	void Update () {
		if (!showFPS) { return; }

		tframe++;
		if (tframe == FPS) {
			tframe = 0;
			fpsText.text = (int)(1 / Time.deltaTime) + " fps";
		}
	}
}
