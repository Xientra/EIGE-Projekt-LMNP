using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour {

    // ingame screen-object
    [SerializeField]
    private RenderTexture target;

    // all gamemode cameras
    [SerializeField]
    private Camera[] cameras = new Camera[3];
    
    protected void TurnOnCamera(string name) {
        Camera cam = FindCamera(name);

        if (cam == null) {
            Debug.LogError("Camera not found");

        } else {
            cam.enabled = true;
            cam.targetTexture = target;
        }
    }

    protected void TurnOffCamera(string name) {
        Camera cam = FindCamera(name);

        if (cam == null) {
            Debug.LogError("Camera not found");

        } else {
            cam.enabled = false;
        }
    }

    private Camera FindCamera(string name) {
        // find camera by name
        foreach (Camera cam in cameras) {
            if (cam != null && cam.name == name) {
                return cam;
            }
        }

        // not found
        return null;
    }
}
