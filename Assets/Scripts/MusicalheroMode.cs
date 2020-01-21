using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int score;
    private int highscore;

    // TODO
    //private AudioSource currentAudio = Resources.Load("Musicalhero/trackname"); 

    private void Start() {
        // keyChain = keys attached to sceneObj
        // TODO (already in the works)
    }

    void Update() {
        // let keys fall from the sky continously
        foreach (GameObject key in keyChain) {
            key.transform.Translate(Vector3.down * keySpeed * Time.deltaTime);
        }
    }

    public void ProcessInput(KeyCode keyCode) {
        
    }

    public void SetupScene() {
        TurnOnCamera();
        // TurnOnMusic();
    }

    public void CloseScene() {
        TurnOffCamera();
        // TurnOffMusic();
    }

    override public string ToString() {
        return "MusicalheroMode";
    }
}
