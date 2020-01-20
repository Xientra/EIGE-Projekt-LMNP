using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalheroMode : CameraManagement, GameMode {

    // empty GameObject in the Scene
    [SerializeField]
    private GameObject sceneObj;

    private GameObject[] keyChain;

    private float speed;

    private void Start() {
        // keyChain = keys attached to sceneObj
        // TODO (already in the works)
    }

    void Update() {
        // let keys fall from the sky continously
        foreach (GameObject key in keyChain) {
            key.transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    public void ProcessInput(KeyCode keyCode) {
        
    }

    public void SetupScene() {
        TurnOnCamera();
    }

    public void CloseScene() {
        TurnOffCamera();
    }

    override public string ToString() {
        return "MusicalheroMode";
    }
}
