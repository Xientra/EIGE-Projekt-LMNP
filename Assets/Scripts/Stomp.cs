using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Stomp : MonoBehaviour {

	private Rigidbody rb;
	private PlayerMovement playerMovement;

	public KeyCode stompKeyCode = KeyCode.LeftControl;
	private bool stompInputThisFrame = false;

	public float stompSpeed = 20;

	[Header("Key Jump:")]
	[Tooltip("How long the key as to be held down to activate a key jump.")]
	public float keyJumpTime = 2f;
	[Tooltip("A key jump performs a normal jump but with this as jumpForce multipier.")]
	public float keyJumpJumpMultiplier = 3f;
	[Tooltip("How long the player can not jump after performing a keyJump to not overwrite the keyJump.")]
	public float preventJumpTime = 0.2f;

	[Header("Effects:")]
	public EffectPrefabs effectPrefabs;

	enum Phases { waitForInput, decending, holdingKey, releasedKey };
	Phases currentPhase = Phases.waitForInput;
	//private bool stomping = false; // while this is true, vel down is stompSpeed
	//private bool pressKey = true; // once this is false the stomp will no longer press the key

	private void Start() {
		rb = GetComponent<Rigidbody>();
		playerMovement = GetComponent<PlayerMovement>();
		if (playerMovement == null) Debug.LogWarning("playerMovement in the Stomp Script on " + gameObject.name + "i s not assinged!");
	}

	void Update() {
		stompInputThisFrame = Input.GetKey(stompKeyCode);

		if (currentPhase == Phases.waitForInput && Input.GetKeyDown(stompKeyCode)) {
			currentPhase = Phases.decending;
			if (playerMovement != null) playerMovement.preventJumping = true;
		}

		if (currentPhase == Phases.decending) {
			Debug.DrawLine(transform.position, transform.position + Vector3.down * (transform.lossyScale.y - (rb.velocity.y * Time.fixedDeltaTime * 2)), Color.red);
		}

		// changed the Phase if the player released stomp, while holding the Key down
		if (currentPhase == Phases.holdingKey && stompInputThisFrame == false) {
			currentPhase = Phases.releasedKey;
		}
	}

	private void FixedUpdate() {

		if (currentPhase == Phases.decending) {
			rb.velocity = new Vector3(rb.velocity.x, -stompSpeed, rb.velocity.z);

			// checks if a key is hit
			Key hitKey = CheckForKey();
			if (hitKey != null) {
				hitKey.Press();
				currentPhase = Phases.holdingKey;

				StartCoroutine(HitKey(hitKey));
			}
		}
	}

	// parents the player to the key for the duration of the animation to have smothe movement (can be changed if that conficts with something else)
	private IEnumerator HitKey(Key hitKey) {

		transform.SetParent(hitKey.transform);

		float timeUntilPressed = hitKey.GetTimeUntilPressed();
		float pressTime = hitKey.time;

		// wait until the key is fully pressed down
		yield return new WaitForSeconds(timeUntilPressed);

		// if the player is still holding stomp then hold the key here
		if (currentPhase == Phases.holdingKey) {
			hitKey.hold = true;
		}

		float keyJumpTimeStamp = Time.time + keyJumpTime;
		bool willKeyJump = false;

		GameObject continiousEffect = null;

		// wait until the player releases the stomp key
		while (currentPhase != Phases.releasedKey) {
			yield return new WaitForEndOfFrame();

			if (keyJumpTimeStamp < Time.time && willKeyJump == false) {
				willKeyJump = true;

				// creates visual effects
				Destroy(Instantiate(effectPrefabs.keyJumpReady, transform.position, Quaternion.identity), 5f);
				continiousEffect = Instantiate(effectPrefabs.keyJumpContinious, transform);
			}
		}
		//if (currentPhase != Phases.releasedKey) yield return new WaitUntil(() => currentPhase == Phases.releasedKey);

		hitKey.hold = false; // tells the key to animate again

		// wait until the key finished it's animation
		yield return new WaitForSeconds(pressTime - timeUntilPressed);

		currentPhase = Phases.waitForInput;
		if (playerMovement != null) {
			playerMovement.preventJumping = false;

			// On KeyJump
			if (willKeyJump == true) {
				
				// prevents the player from jumping for a few milliseconds so he cant override the keyJump
				playerMovement.preventJumping = true;
				playerMovement.SetPreventJumpingAfterDelay(false, preventJumpTime);

			
				// tells the player to jump
				playerMovement.performJump(keyJumpJumpMultiplier);


				// ends previous visual effect and creates new
				continiousEffect.GetComponentInChildren<ParticleSystem>().Stop();
				Destroy(continiousEffect, 3);

				Destroy(Instantiate(effectPrefabs.keyJumpReady, transform.position, Quaternion.identity), 5f);
			}
		}

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

		// casts a "thick ray" and checks all results if there is a Key 
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

[System.Serializable]
public class EffectPrefabs {
	public GameObject keyJumpReady;
	public GameObject keyJumpContinious;
	public GameObject keyJumpRelease;
}