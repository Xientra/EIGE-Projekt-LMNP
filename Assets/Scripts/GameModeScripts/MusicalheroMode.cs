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
    private int penaltyPoints = 75;

    private float[] firstZone;
    private float[] secondZone;
    private float[] thirdZone;

	// sound
	[SerializeField]
	private AudioClip currentTrack;

    void Update() {
		if (AudioManager.instance.tetrisTheme.isPlaying == true) {
			// let keys fall from the sky continously
			keyChain.transform.Translate(Vector3.down * keySpeed * Time.deltaTime);
		}
        if (keyChain.transform.childCount == 0) {
            // show score 
            // wait 
            //GameModeManager.Instance.NextScene();
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

    /*private void LoadTrack(string name) {
        currentTrack = Resources.Load<AudioClip>("Musicalhero/" + name);
    }

    private void PlayTrack(string name) {
        LoadTrack(name);
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = currentTrack;
        source.Play();
    }*/

    public void ProcessInput(KeyCode keyCode) {
        Debug.Log("Processing...");

        GameObject keyObj;
        if (scene.isVisible(keyCode, out keyObj)) {

            float position = keyObj.transform.position.y;
            int points = DecidePoints(position);
            MusicalheroScore.Instance.AddPoints(points);

            //Keyboard.instance.LetKeyFall(keyCode); TODO
        }
	}

    public void SetupScene() {
        TurnOnCamera();

        firstZone = scene.GetFirstZone();
        secondZone = scene.GetSecondZone();
        thirdZone = scene.GetThirdZone();

        MusicalheroScore.Instance.SetPenalty(penaltyPoints);
        MusicalheroScore.Instance.AddPoints(0);
    }

    public void CloseScene() {
        TurnOffCamera();
    }

    override public string ToString() {
        return "Musicalhero";
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
