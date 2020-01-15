using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    // ingame screen-object
    [SerializeField]
    private RenderTexture target;
    
    protected void TurnOnCamera() {
        if (cam == null) {
            Debug.LogError("Camera not found");

        } else {
            cam.enabled = true;
            cam.targetTexture = target;
        }
    }

    protected void TurnOffCamera() {
        if (cam == null) {
            Debug.LogError("Camera not found");

        } else {
            cam.enabled = false;
        }
    }
}
