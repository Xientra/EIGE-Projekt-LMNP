using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    GameModeManager modeManager;

    // hardcoded things
    string[] orderOfModes = { "TextadventureMode", "MusicalheroMode" };

    private void Start() {
        modeManager = new GameModeManager(transform.GetComponents<GameMode>());
        modeManager.SelectMode(orderOfModes[0]);

        // testing-sequence
        modeManager.PassInput(KeyCode.M);
        modeManager.PassInput(KeyCode.A);
        modeManager.PassInput(KeyCode.Y);
        modeManager.PassInput(KeyCode.B);
        modeManager.PassInput(KeyCode.E);
        modeManager.PassInput(KeyCode.Question);
        modeManager.PassInput(KeyCode.Return);
    }
}
