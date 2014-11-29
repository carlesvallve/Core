using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenFader : MonoBehaviour {

	/*
	 * ------------------------------------------------------------------------
	 * 
	 * ScreenFader.cs 1.1 for Unity 4.6
	 * Fades the screen in and out to specified color.
	 * 
	 * Samuel Johansson 2014, samuel@phaaxgames.com
	 * http://www.phaaxgames.com
	 * 
	 * ------------------------------------------------------------------------
	 * License:
	 * Free to use as long as you give some credit in your application/game.
	 * ------------------------------------------------------------------------
	 * Notes: 
	 * Pre-Unity 4.6 GUI elements will be shown above this fade.
	 * This is useful if you want to debug out information on the screen.
	 *
	 * If there are fades running then any new fade will be queued
	 * and a warning will be printed to the log.
	 * ------------------------------------------------------------------------
	 * Usage:
	 * Drop it into your assets folder somewhere.
	 * Don't attach it to any object.
	 * ------------------------------------------------------------------------
	 * Functions:
	 * 
	 * ScreenFader.FadeIn (float duration);
	 *	Fades in with the specified duration (seconds).
	 * 
	 * ScreenFader.FadeOut (float duration);
	 *	Fades out with the specified duration (seconds).
	 * 
	 * ScreenFader.SetColor (Color newColor);
	 *	Changes the color of the fade.
	 * 
	 * ScreenFader.SetActive (bool active, float alpha);
	 *	active: enables/disables fade image.
	 *	alpha: optional, the alpha value of the image.
	 * 
	 * ScreenFader.FadeOut (float duration, bool stayActive);
	 * 	Fades out with the specified duration (seconds)
	 *	and keeps the screen faded (active) afterwards.
	 * 
	 * ScreenFader.FadeCustom (float from, float to, float duration);
	 * 	from: Alpha value to start at.
	 * 	to: Alpha value to end at.
	 * 	duration: Time in seconds.
	 * 	
	 * ScreenFader.FadeCustom (float from, float to, float duration, bool stayActive);
	 * 	Same as above but with the option to keep screen active after
	 *	completing the fade.
	 * 
	 * ScreenFader.IsFading()
	 * 	Check if a fade is currently running.
	 * 	returns true/false.
	 * 
	 * ScreenFader.IsActive()
	 * 	Check if the fade is active (showing).
	 * 	returns true/false.
	 * ------------------------------------------------------------------------
	 */

	private Image fadeImage;
	private bool isFading = false;

	private List<int> activeFades = new List<int>();
	private int callCount = 0;

	private static ScreenFader _instance = null;
	public static ScreenFader Instance {
		get	{
			if (!_instance)	{
				_instance = GameObject.FindObjectOfType<ScreenFader>();
				if (!_instance)	{
					GameObject container = new GameObject("ScreenFader");
					_instance = container.AddComponent<ScreenFader>();
				}
			}

			return _instance;
		}
	}

	void Awake() {
		if(_instance == null) {
			_instance = this;
			_instance.gameObject.name = "ScreenFader";
			Initialize();
		} else {
			Debug.LogError("<<<ScreenFader>>> There's more than one ScreenFader component in this scene! [Removing]");
			if(this != _instance) Destroy(this);
		}
	}

	private void Initialize () {
		gameObject.AddComponent<Canvas>();
		gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		gameObject.GetComponent<Canvas>().sortingOrder = 32767;

		fadeImage = gameObject.AddComponent<Image>();
		fadeImage.color = new Color(0,0,0,1);

		fadeImage.enabled = false;
	}

	public static void SetColor (Color newColor) {
		Instance.fadeImage.color = newColor;
	}

	public static bool IsFading () {
		return Instance.isFading;
	}

	public static bool IsActive () {
		return Instance.fadeImage.enabled;
	}

	public static void FadeIn (float duration) {
		Instance.StartCoroutine(Instance.FadeAlpha(1,0,duration,false));
	}

	public static void FadeOut (float duration) {
		Instance.StartCoroutine(Instance.FadeAlpha(0,1,duration,false));
	}

	public static void FadeOut (float duration, bool stayActive) {
		Instance.StartCoroutine(Instance.FadeAlpha(0,1,duration,true));
	}

	public static void FadeCustom (float from, float to, float duration) {
		Instance.StartCoroutine(Instance.FadeAlpha(from,to,duration,false));
	}

	public static void FadeCustom (float from, float to, float duration, bool stayActive) {
		Instance.StartCoroutine(Instance.FadeAlpha(from,to,duration,stayActive));
	}

	public static void SetActive (bool active) {
		SetActive(active, 1);
	}

	public static void SetActive (bool active, float alpha) {
		Color imageColor = Instance.fadeImage.color;
		imageColor.a = alpha;
		Instance.fadeImage.color = imageColor;
		Instance.fadeImage.enabled = active;
	}

	private IEnumerator FadeAlpha (float from, float to, float duration, bool stayActive) {
		isFading = true;
		int callNumber = callCount++; 
		activeFades.Add(callNumber);

		if (activeFades[0] != callNumber)
			Debug.LogWarning("<<<ScreenFader>>> Fade "+callNumber+" has been queued!");

		while (activeFades[0] != callNumber) {
			// Fade will wait until it is its turn to run
			yield return new WaitForEndOfFrame();
		}

		float startTime = Time.realtimeSinceStartup;
		Color imageColor = fadeImage.color;
		fadeImage.enabled = true;
		while (Time.time - startTime < duration) {
			imageColor.a = Mathf.Lerp(from, to, (Time.realtimeSinceStartup - startTime) / duration);
			fadeImage.color = imageColor;
			yield return new WaitForEndOfFrame();
		}
		fadeImage.enabled = stayActive;

		activeFades.Remove(callNumber);
		isFading = false;
	}
}
