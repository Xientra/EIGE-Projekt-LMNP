using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour {

	public static Keyboard instance;


	public Transform[] keyParents;

	void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Debug.LogError("There can only be one keyboard.");
			Destroy(this.gameObject);
		}
	}

	private void Start() {
		LetKeyFall(KeyCode.F);
	}

	public void LetKeyFall(KeyCode keyCode) {
		foreach (Transform row in keyParents) {
			foreach (Transform t in row) {
				Debug.Log(t.name);
				Key k = t.GetComponent<Key>();
				if (k != null) {
					if (k.keyCode == keyCode) {
						k.Fall();
					}
				}
			}
		}
	}
}
