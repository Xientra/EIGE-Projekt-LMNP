using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    // set order of GameModes from Inspector
    [SerializeField]
    string[] orderOfModes = { "TextadventureMode", "MusicalheroMode" };

    private void Start() {
        GameModeManager.Instance.AcceptModes(transform.GetComponents<GameMode>());
        //GameModeManager.Instance.SelectMode(orderOfModes[0]);

        /*// testing-sequence for TextadventureMode
        GameModeManager.Instance.PassInput(KeyCode.M);
        GameModeManager.Instance.PassInput(KeyCode.A);
        GameModeManager.Instance.PassInput(KeyCode.Y);
        GameModeManager.Instance.PassInput(KeyCode.D);
        GameModeManager.Instance.PassInput(KeyCode.Backspace);
        GameModeManager.Instance.PassInput(KeyCode.B);
        GameModeManager.Instance.PassInput(KeyCode.E);
        GameModeManager.Instance.PassInput(KeyCode.Question);
        GameModeManager.Instance.PassInput(KeyCode.Return);*/

        // switch GameMode
        GameModeManager.Instance.SelectMode(orderOfModes[1]);

        // testing-sequence for MusicalheroMode
        GameModeManager.Instance.PassInput(KeyCode.A);
    }
}
