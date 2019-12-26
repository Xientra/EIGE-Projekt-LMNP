using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Stomp : MonoBehaviour {

	private Rigidbody rb;
	public KeyCode stompKeyCode = KeyCode.LeftControl;

	public float stompSpeed = 20;

	private bool stomping = false; // while this is true, vel down is stompSpeed
	private bool pressKey = true; // once this is false the stomp will no longer press the key

	private void Start() {
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		if (stomping == false && Input.GetKeyDown(stompKeyCode)) {
			stomping = true; 
			pressKey = true;
		}

		Debug.DrawLine(transform.position, transform.position + Vector3.down * (transform.lossyScale.y - (rb.velocity.y * Time.fixedDeltaTime * 2)), Color.red);
	}

	private void FixedUpdate() {

		if (stomping == true) {
			rb.velocity = new Vector3(rb.velocity.x, -stompSpeed, rb.velocity.z);
		}

		Key hitKey = CheckForKey();
		if (hitKey != null) {
			if (stomping == true && pressKey == true) {
				hitKey.Press();
				pressKey = false;

				StartCoroutine(StopStomping(hitKey.time, hitKey));
			}
		}
	}

	// parents the player to the key for the duration of the animation to have smothe movement (can be changed if that conficts with something else)
	private IEnumerator StopStomping(float delay, Key hitKey) {
		stomping = false;

		transform.SetParent(hitKey.transform);

		yield return new WaitForSeconds(delay);

		transform.SetParent(null);
	}

	// checks if there is a key beneath the player
	private Key CheckForKey() {

		// an avg between width and height of the player to use as the sphere radius
		float avgSize = (transform.lossyScale.x + transform.lossyScale.z) / 2;
		float sphereRadius = (avgSize / 2) * 0.95f;

		float halfPlayerHeight = transform.lossyScale.y; // this might vary depending on what mesh is used, use:
		// float halfPlayerHeight = GetComponent<Collider>().bounds.extents.y; // for more accuracy (i think ;-;)
		
		// the distance the player will travel down next frame
		//(-rb.velocity.y * Time.fixedDeltaTime)

		// casts a "thick ray" and checks all results if there are a Key 
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, Vector3.down, halfPlayerHeight + (-rb.velocity.y * Time.fixedDeltaTime));
		foreach (RaycastHit hit in hits) {
			Key keyHit = hit.transform.GetComponent<Key>();
			if (keyHit != null) {
				return keyHit;
			}
		}

		return null;
	}

}