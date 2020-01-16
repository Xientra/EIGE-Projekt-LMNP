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

        //testing
        modeManager.GetMode().ProcessInput(KeyCode.M);
        modeManager.GetMode().ProcessInput(KeyCode.A);
        modeManager.GetMode().ProcessInput(KeyCode.Y);
        modeManager.GetMode().ProcessInput(KeyCode.B);
        modeManager.GetMode().ProcessInput(KeyCode.E);
        modeManager.GetMode().ProcessInput(KeyCode.Question);
        modeManager.GetMode().ProcessInput(KeyCode.Return);

    }
}
