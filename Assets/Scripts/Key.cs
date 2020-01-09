using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key : MonoBehaviour {


	public KeyCode keyCode;

	public string keyString;

	[Tooltip("If this is true, that means this key can be added as ac character to a string.")]
	public bool isCharacter = false;

	public TextMeshProUGUI text;

	private Vector3 originalPosition;
	[Header("Animation:")]

	[Tooltip("How far down the key moves (in world units).")]
	public float maxDepth = 1.5f;

	private bool animating = false;
	[Tooltip("How long it takes to play the animation.")]
	public float time = 1f;

	public AnimationCurve animationCurve; // a curve that holds the values at what point the key should be at what time

	[Range(0f, 1f)]
	public float pressState = 0; // how far the key has been pressed

	void Start() {
		originalPosition = transform.position;
	}

	// moves the key based on pressState and maxDepth
	private void FixedUpdate() {
		transform.position = (Vector3.down * maxDepth * pressState) + originalPosition;
	}

	// starts the animation
	public void Press() {
		if (animating == false) {
			if (isCharacter) {
				//ModeManager.instace.GetCurrentGameMode().InputImpusle(keyCode); // <------------------------ here
				TextInTheSky.instance.textUI.text += keyString; // just for testing
			}
			StartCoroutine(PressAnimation());
		}
	}

	// sets pressState to the coresponding value in the animaiton cure over the span of (the var) time
	private IEnumerator PressAnimation() {
		animating = true;

		for (float f = 0; f < time; f += Time.fixedDeltaTime) {
			pressState = animationCurve.Evaluate(f / time);
			yield return new WaitForFixedUpdate();
		}

		animating = false;
	}

	// get the time from the animation curve when the key is fully pressed the first time
	public float GetTimeUntilPressed() {
		foreach (Keyframe kf in animationCurve.keys) {
			if (kf.value == 1) {
				return kf.time * time;
			}
		}

		return time / 2; // just a random guess to at least have a default value
	}


	/* -===== Debug =====- */

	[Header("Debug:")]
	[Tooltip("If this is enabled the Name of the Object and the text on the key will be set to the keyString")]
	public bool updateApperance;

	public bool debugging = false;
	public Vector3 originalPositionPlaceHolder;

	// this is called everytime the inspector updates
	private void OnValidate() {

		if (updateApperance) {
			/*	
			if (keyString != "") {
				text.text = keyString;
				gameObject.name = "Key \"" + keyString + "\"";
			}
			else {
				text.text = "(X)";
				gameObject.name = "Key \"" + keyString + "\"";
			}
			*/
			
			if (keyCode != KeyCode.None) {
				gameObject.name = "Key \"" + keyCode + "\"";
			}
			else {
				gameObject.name = "Key \"" + keyCode + "\"";
			}
			
		}

		if (debugging) {
			transform.position = (Vector3.down * maxDepth * pressState) + originalPositionPlaceHolder;
		}
	}
}