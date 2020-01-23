using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalHeroScreen : MonoBehaviour {

	public Queue<GameObject> keyInScreen = new Queue<GameObject>();

	public Transform limit1;
	public Transform limit2;

	private void OnTriggerEnter(Collider other) {
		keyInScreen.Enqueue(other.gameObject);
	}


	private void OnTriggerExit(Collider other) {
		keyInScreen.Dequeue();
	}

	// Script.instance.LetKeyFall(KeyCode keyCode);
}
