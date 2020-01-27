using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private TextMeshProUGUI scoreboard;
    private int score = 0;
    private int highscore;

	// sound (TODO generalize)
	[SerializeField]
	private AudioClip currentTrack;

    void Update() {
		if (AudioManager.instance.tetrisTheme.isPlaying == true) {
			// let keys fall from the sky continously
			keyChain.transform.Translate(Vector3.down * keySpeed * Time.deltaTime);
		}
		
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
        Debug.Log(visible.Count);

        foreach (GameObject obj in visible) {
            Debug.Log(obj.ToString());
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
		/*
        LoadTrack("tetris");
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = currentTrack;
        source.Play();
		*/
    }

    public void ProcessInput(KeyCode keyCode) {
        Debug.Log("Processing...");
        GameObject keyObj;
        if (isInFOV(keyCode, out keyObj)) {
            float position = keyObj.transform.position.y;
            int points = DecidePoints(position);
            UpdateScore(points);

            Destroy(keyObj);
            Debug.Log("Destroyed!");
			// Script.instance.LetKeyFall(keyCode); <------------------------ @Paul

        }
	}

    public void SetupScene() {
        TurnOnCamera();

        firstZone = scene.GetFirstZone();
        secondZone = scene.GetSecondZone();
        thirdZone = scene.GetThirdZone();
        scene.SetPenalty(penalty);

        UpdateScore(0);
        PlayTrack();
    }

    public void CloseScene() {
        TurnOffCamera();
    }

    override public string ToString() {
        return "MusicalheroMode";
    }


	/* -----------------------=== Debug Stuff Paul did ===----------------------- */

	[Header("DEBUG:")]
	public bool updateInOnValidate = false;
	[Range(0f, 1f)]
	public float songCompleation = 0;

	// OnValidate is called whenever the instector updates (including whenever something changed in it)
	private void OnValidate() {
		if (updateInOnValidate == true) {
			keyChain.transform.position = new Vector3(keyChain.transform.position.x, -keySpeed * currentTrack.length * songCompleation, keyChain.transform.position.z);
		}
	}


	private void Start() {
		StartCoroutine(StartAfter(2));
	}

	private IEnumerator StartAfter(float delay) {
		yield return new WaitForSeconds(delay);

		AudioManager.instance.PlaySound("tetrisTheme");
	}
}
