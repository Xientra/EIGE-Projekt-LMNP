using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

    GameMode currentMode;

    // TODO outsource
    private void Start() {
       currentMode = new TextadventureMode();
       currentMode.SetupScene();
    }

    public void SetMode(GameMode mode) {
        currentMode.CloseScene();
        currentMode = mode;
        currentMode.SetupScene();
    }

    public GameMode GetMode() {
        return currentMode;
    }
}
