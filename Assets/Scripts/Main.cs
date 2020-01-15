using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    GameModeManager modeManager = new GameModeManager();

    private void Start() {
        modeManager.NextMode();
    }
}
