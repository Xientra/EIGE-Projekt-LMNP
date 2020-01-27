using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForLazyPeople : MonoBehaviour {

	public GameObject prefabToSpawn;
	public Transform toParentTo;
	public Transform spawnPosition;


	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			Instantiate(prefabToSpawn, spawnPosition.position, prefabToSpawn.transform.rotation, toParentTo);
		}
	}
}
