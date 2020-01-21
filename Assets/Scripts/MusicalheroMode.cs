using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicalheroMode : CameraManagement, GameMode {

    // empty GameObject in Scene
    [SerializeField]
    private GameObject sceneObj;

    // falling keys
    private GameObject[] keyChain;
    [SerializeField]
    private float keySpeed;

    // set boundaries from Inspector
    [SerializeField]
    private float deathLine;
    [SerializeField]
    private float greatUpper, greatLower;
    [SerializeField]
    private float goodUppper, goodLower;
    [SerializeField]
    private float okayUpper, okayLower;
    [SerializeField]
    private float badUpper, badLower;

    // scoring
    [SerializeField]
    private Text scoreboard;
    private int score;
    private int highscore;

    // sound (TODO generalize)
    private AudioClip currentTrack;

    private void Start() {
        // keyChain = keys attached to sceneObj
        // TODO (already in the works)
    }

    void Update() {
        /*// let keys fall from the sky continously
        foreach (GameObject key in keyChain) {
            key.transform.Translate(Vector3.down * keySpeed * Time.deltaTime);
        }*/
    }

    private void UpdateScore(int points) { 
        
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
