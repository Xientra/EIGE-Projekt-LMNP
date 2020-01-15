using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour {

    // ingame screen-object
    [SerializeField]
    private RenderTexture target; 
    
    protected void TurnOnCamera(string name) {
        Camera cam = FindCamera(name);

        if (cam == null) {
            Debug.LogError("Camera not found");

        } else {
            cam.targetTexture = target;
        }
    }

    protected void TurnOffCamera(string name) { 
    
    }

    private Camera FindCamera(string name) {
        // find camera by name
        foreach (Camera cam in Camera.allCameras) {
            if (cam.gameObject.name == name) {
                return cam;
            }
        }

        // not found
        return null;
    }
}
