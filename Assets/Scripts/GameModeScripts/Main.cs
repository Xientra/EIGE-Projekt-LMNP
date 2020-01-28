using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    [SerializeField]
    string gameMode;

    private void Start() {
        GameModeManager.Instance.AcceptModes(transform.GetComponents<GameMode>());
        GameModeManager.Instance.SelectMode(gameMode);
    }
}
