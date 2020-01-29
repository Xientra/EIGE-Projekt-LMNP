using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressF : MonoBehaviour
{
    private bool canBeEnded = false;
    private bool swappedScreen = false;
    [SerializeField]
    private GameObject Cube001;
    [SerializeField]
    private Material endScreen;
    [SerializeField]
    KeyCode exitKey = KeyCode.Escape;

	bool fWasPressed = false;

    void Update()
    {
        if(fWasPressed == false && gameObject.GetComponent<Key>().isPressed == true)
        {
			fWasPressed = true;
            swapToEndScreen();
        }
        if(Input.GetKeyDown(exitKey) && canBeEnded)
        {
            Application.Quit();
        } 
    }

    private void swapToEndScreen()
    {
        if(!swappedScreen)
        {
            Cube001.GetComponent<MeshRenderer>().material = endScreen;
            swappedScreen = true;
            canBeEnded = true;
        }
    }

    
}
