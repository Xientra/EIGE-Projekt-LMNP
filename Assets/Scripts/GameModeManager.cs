using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

    private GameMode[] modes;
    private GameMode current;

    public static GameModeManager Instance { get; set; }

    private void Awake() {
        if (Instance == null) {
            Debug.Log("Instantiate...");
            Instance = this;
        }
    }

    public void AcceptModes(GameMode[] modes) {
        this.modes = modes;
    }

    /*public GameModeManager(GameMode[] modes) {
        if (modes == null) {
            Debug.Log("null argument");
        }
        this.modes = modes;
    }*/

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

    private void SetMode(GameMode mode) {
        if (current != null) {
            current.CloseScene();
        }
        current = mode;
        current.SetupScene();
    }

    public void PassInput(KeyCode keyCode) {
        Debug.Log("Input?");
        current.ProcessInput(keyCode);
    }
}
