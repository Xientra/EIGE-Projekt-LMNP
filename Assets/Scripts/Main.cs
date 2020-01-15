using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    GameModeManager modeManager = new GameModeManager();

    // possibly move into GameModeManager
    GameMode[] modes = { new TextadventureMode(), new MusicalheroMode() };

    private void Start() {
        modeManager.SetMode(modes[1]);
    }
}
