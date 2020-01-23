using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicalheroMode : CameraManagement, GameMode {

    [SerializeField]
    private MusicalheroScene scene;

    // falling keys
    [SerializeField]
    private GameObject keyChain;
	[SerializeField]
    private float keySpeed;

    // pointing system
    [SerializeField]
    private int firstPoints = 50;
    [SerializeField]
    private int secondPoints = 100;
    [SerializeField]
    private int thirdPoints = 50;
    [SerializeField]
    private int penalty = 75;

    private float[] firstZone;
    private float[] secondZone;
    private float[] thirdZone;

    // scoring
    [SerializeField]
    private Text scoreboard;
    private int score = 0;
    private int highscore;

    // sound (TODO generalize)
    private AudioClip currentTrack;

    void Update() {
        // let keys fall from the sky continously
        keyChain.transform.Translate(Vector3.down * keySpeed * Time.deltaTime);

        int penaltyPoints = scene.getPenaltyPoints();
        if (penaltyPoints != 0) {
            UpdateScore(penaltyPoints);
        }
    }

    private int DecidePoints(float position) {
        if (position < firstZone[0] && position > firstZone[1]) {
            return firstPoints;

        } else if (position < secondZone[0] && position > secondZone[1]) {
            return secondPoints;

        } else if (position < thirdZone[0] && position > thirdZone[1]) {
            return thirdPoints;

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
        List<GameObject> visible = scene.GetVisibleKeys();

        foreach (GameObject obj in visible) {
            if (obj.ToString() == keyCode.ToString()) {
                visible.Remove(obj);
                key = obj;
                return true;
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
			// Script.instance.LetKeyFall(keyCode); <------------------------ @Paul
		}
	}

    public void SetupScene() {
        TurnOnCamera();

        firstZone = scene.GetFirstZone();
        secondZone = scene.GetSecondZone();
        thirdZone = scene.GetThirdZone();
        scene.SetPenalty(penalty);

        PlayTrack();
    }

    public void CloseScene() {
        TurnOffCamera();
    }

    override public string ToString() {
        return "MusicalheroMode";
    }
}
