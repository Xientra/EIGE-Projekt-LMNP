using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

    private GameMode[] modes = { new TextadventureMode(), new MusicalheroMode() };
    private GameMode current;
    private int index = -1;

    public void NextMode() { 
        if (index < modes.Length) {
            SetMode(modes[++index]);
        }
    }

    public void SetMode(GameMode mode) {
        current.CloseScene();
        current = mode;
        current.SetupScene();
    }

    public GameMode GetMode() {
        return current;
    }
}
