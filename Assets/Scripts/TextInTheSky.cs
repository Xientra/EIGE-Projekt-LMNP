using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInTheSky : MonoBehaviour {

	public static TextInTheSky instance;
	public TextMeshProUGUI textUI;

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
	}
}