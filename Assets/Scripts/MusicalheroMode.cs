using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalheroMode : CameraManagement, GameMode {

    void Update() {

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
