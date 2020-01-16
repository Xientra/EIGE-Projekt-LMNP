using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

    private GameMode[] modes;
    private GameMode current;

    /*// TODO hardcoded 
    public void OurModes() {
        modes = new GameMode[2];
        modes[0] = transform.GetComponent<TextadventureMode>();
        modes[1] = transform.GetComponent<MusicalheroMode>();
    }*/

    public GameModeManager(GameMode[] modes) {
        if (modes == null) {
            Debug.Log("null argument");
        }
        this.modes = modes;
    }

    public void SelectMode(string name) {
        GameMode selected = FindMode(name);

        if (selected != null){
            SetMode(selected);
        } else {
            Debug.Log("GameMode not found");
        }
    }

    private GameMode FindMode(string name) {
        foreach (GameMode mode in modes) {
            if (mode.ToString() == name) {
                return mode;
            }
        }
        return null;
    }

    public void SetMode(GameMode mode) {
        if (current != null) {
            current.CloseScene();
        }
        current = mode;
        current.SetupScene();
    }

    public GameMode GetMode() {
        return current;
    }

    public void PassInput(KeyCode keyCode) {
        current.ProcessInput(keyCode);
    }
}
