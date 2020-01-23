using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicKey : MonoBehaviour {
    [SerializeField]
    private KeyCode keyCode;

    public override string ToString() {
        return keyCode.ToString();
    }
}
