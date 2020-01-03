using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key : MonoBehaviour {

	public KeyCode keyCode;
	public bool updateApperance;
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
			//ModeManager.instace.GetCurrentGameMode().InputImpusle(keyCode);
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
	public bool debugging = false;
	public Vector3 originalPositionPlaceHolder;

	private void OnValidate() {

		if (updateApperance) {
			if (keyCode != KeyCode.None) {
				text.text = keyCode.ToString();
				gameObject.name = "Key \"" + keyCode.ToString() + "\"";
			}
			else {
				text.text = "(X)";
				gameObject.name = "Key \"" + keyCode.ToString() + "\"";
			}
		}

		if (debugging) {
			transform.position = (Vector3.down * maxDepth * pressState) + originalPositionPlaceHolder;
		}
	}
}