using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalheroMain : MonoBehaviour {

    private void Start() {
        GameModeManager.Instance.AcceptModes(transform.GetComponents<GameMode>());
        GameModeManager.Instance.SelectMode("Musicalhero");
    }
}
