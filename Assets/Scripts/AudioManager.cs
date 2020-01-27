using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	[Space(5)]
	public AudioSource dashSound;
	public AudioSource stompSound;
	public AudioSource keyClackSound;
	public AudioSource grappelHookShoot;
	public AudioSource grappelHookHit;
	[Space(5)]
	public AudioSource tetrisTheme;

	private void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(this.gameObject);
	}


	public void PlaySound(string name) {
		switch (name) {
			case "dash":
				dashSound.Play();
				break;
			case "stomp":
				stompSound.Play();
				break;
			case "keyClack":
				keyClackSound.pitch = Random.Range(0.3f, 1f);
				keyClackSound.Play();
				break;
			case "grappleShoot":
				grappelHookShoot.Play();
				break;
			case "grappleHit":
				grappelHookHit.Play();
				break;

			case "tetrisTheme":
				tetrisTheme.Play();
				break;
		}
	}

	public void StopSound(string name) {
		switch (name) {
			case "dash":
				dashSound.Stop();
				break;
			case "stomp":
				stompSound.Stop();
				break;
			case "keyClack":
				keyClackSound.Stop();
				break;
			case "grappleShoot":
				grappelHookShoot.Stop();
				break;
			case "grappleHit":
				grappelHookHit.Stop();
				break;

			case "tetrisTheme":
				tetrisTheme.Stop();
				break;
		}
	}
}
