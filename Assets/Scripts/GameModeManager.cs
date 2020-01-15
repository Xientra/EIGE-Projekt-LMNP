using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

    GameMode currentMode;

    public void setMode(GameMode mode) {
        currentMode.closeScene();
        currentMode = mode;
        currentMode.setupScene();
    }

    public GameMode getMode() {
        return currentMode;
    }
}
