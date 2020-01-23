using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicalheroMode : CameraManagement, GameMode {

    // falling keys
    [SerializeField]
    private GameObject keyChain;

	[SerializeField]
	private MusicalHeroScreen musicalHeroScreen;

	[SerializeField]
    private float keySpeed;

    // set boundaries from Inspector
    [SerializeField]
    private float deathLine;
    [SerializeField]
    private float greatUpper, greatLower;
    [SerializeField]
    private float goodUpper, goodLower;
    [SerializeField]
    private float okayUpper, okayLower;

    // scoring
    [SerializeField]
    private Text scoreboard;
    private int score;
    private int highscore;

    // sound (TODO generalize)
    private AudioClip currentTrack;

    void Update() {
        // let keys fall from the sky continously
        keyChain.transform.Translate(Vector3.down * keySpeed * Time.deltaTime);
    }

    private int DecidePoints(float position) {
        if (position > deathLine) {
            return -100;

        } else if (position < okayUpper && position > okayLower) {
            return 100;

        } else if (position < goodUpper && position > goodLower) {
            return 200;

        } else if (position < greatUpper && position > greatLower) {
            return 300;

        } else {
            return 0;
        }
    }

    private void UpdateScore(int points) {
        score += points;
        scoreboard.text = score.ToString();
    }

    // asesses if suitable GameObject is in camera's field of view 
    private bool isInFOV(KeyCode keyCode, out GameObject key) {
        GameObject[] keys = keyChain.GetComponentsInChildren<GameObject>();

        foreach (GameObject obj in keys) {
            if (obj.name == keyCode.ToString()) {
                if (obj.GetComponent<Renderer>().isVisible) {
                    key = obj;
                    return true;
                }
            }
        }

        // nothing found
        key = null;
        return false;
    }

    private void LoadTrack(string name) {
        currentTrack = Resources.Load<AudioClip>("Musicalhero/" + name);
    }

    private void PlayTrack() {
        LoadTrack("tetris");
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = currentTrack;
        source.Play();
    }

    public void ProcessInput(KeyCode keyCode) {
        GameObject keyObj;
        if (isInFOV(keyCode, out keyObj)) {
            float position = keyObj.transform.position.y;
            int points = DecidePoints(position);
            UpdateScore(points);

            Destroy(keyObj);
			// Script.instance.LetKeyFall(KeyCode keyCode); <------------------------ @Paul
		}
	}

    public void SetupScene() {
        TurnOnCamera();
        PlayTrack();
    }

    public void CloseScene() {
        TurnOffCamera();
    }

    override public string ToString() {
        return "MusicalheroMode";
    }
}
