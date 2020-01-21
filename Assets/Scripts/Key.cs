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

	public bool hold = false;

	void Start() {
		originalPosition = transform.position;
	}

	// moves the key based on pressState and maxDepth
	private void FixedUpdate() {
		transform.position = (Vector3.down * maxDepth * pressState /* * transform.lossyScale.y*/) + originalPosition;
	}

	public void Press() {
		if (animating == false) {
			if (isCharacter) {
				//ModeManager.instace.GetCurrentGameMode().InputImpusle(keyCode); // <------------------------ here @Nathalie
				TextInTheSky.instance.textUI.text += keyString; // just for testing
			}

			// starts the animation
			StartCoroutine(PressAnimation());
		}
	}

	/// <summary> 
	/// Sets pressState to a range from 0 to 1 based on the animaiton cure over the span of 'time'. Can be paused with hold
	/// </summary>
	private IEnumerator PressAnimation() {
		animating = true;

		float animationTime = 0;
		while (animationTime < time) {

			pressState = animationCurve.Evaluate(animationTime / time);
			yield return new WaitForFixedUpdate();

			// stops moving if hold == true
			if (hold == false) animationTime += Time.fixedDeltaTime;
		}

		pressState = 0; // just to be sure
		animating = false;
	}

	//public float timeUntilPressed { get => GetTimeUntilPressed(); }

	/// <summary> 
	/// Gets the time from the animation curve where the key is fully pressed the first time (results animationTime / 2 if none found)
	/// </summary>
	public float GetTimeUntilPressed() {
		foreach (Keyframe kf in animationCurve.keys) {
			if (kf.value == 1) {
				return kf.time * time;
			}
		}

		return time / 2;
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
			if (isCharacter)
				text.text = keyString;
		}

		if (debugging) {
			transform.position = (Vector3.down * maxDepth * pressState) + originalPositionPlaceHolder;
		}
	}
}