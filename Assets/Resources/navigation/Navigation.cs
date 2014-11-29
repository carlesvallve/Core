using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class Navigation : MonoBehaviour {

	public float fadeDuration = 0.5f;
	public int FPS = 60;
	public bool showFPS = false;

	private Image fadeImage;
	private Text fpsText;
	
    private int m_fps;
    private int tframe = 0;
    
    private string sceneToLoad;


    // Initialization

    void Awake () {
		// get elements
		Transform canvas = transform.Find("Canvas");
		fadeImage = canvas.GetComponent<Image>();
		fadeImage.enabled = false;
		fpsText = canvas.Find("Fps").GetComponent<Text>();
		fpsText.enabled = showFPS;

		// fade in
		fadeIn(fadeDuration);
	}


	// Navigation Handlers

	public void startGame () {
		sceneToLoad = "Game";
		//Audio.play("audio/typekey", 1.0f, 1.0f);
		fadeOut(fadeDuration);
	}


	public void exitGame () {
		sceneToLoad = "Intro";
		//Audio.play("audio/typekey", 1.0f, 1.0f);
		fadeOut(fadeDuration);
	}


	public void pressButton () {
		Audio.play("audio/click", 1.0f, 1.0f);
	}


	public void releaseButton () {
		Audio.play("audio/typekey", 1.0f, 1.0f);
	}


	private void loadNextScene () {
		Application.LoadLevel(sceneToLoad);
	}


	// Screen Fader

	public void fadeIn (float duration) {
		fadeImage.enabled = true;
		fadeImage.color = new Color(0,0,0,1);
		DOTween.ToAlpha(()=> fadeImage.color, x => fadeImage.color = x, 0, duration);
	}


	public void fadeOut (float duration) {
		fadeImage.enabled = true;
		fadeImage.color = new Color(0,0,0,0);

		DOTween.ToAlpha(()=> fadeImage.color, x => fadeImage.color = x, 1, duration)
			.OnComplete(loadNextScene);
	}


	// FPS Counter

	void Update () {
		tframe++;
		if (tframe == FPS) {
			tframe = 0;
			m_fps = (int)(1 / Time.deltaTime);
		}

		fpsText.text = m_fps + " fps";
	}
}
