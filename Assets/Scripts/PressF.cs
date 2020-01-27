using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressF : MonoBehaviour
{
    private bool canBeEnded = false;
    private bool swappedScreen = false;
    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    KeyCode exitKey = KeyCode.Escape;

    void Update()
    {
        if(gameObject.GetComponent<Key>().pressState > 0)
        {
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
            endScreen.SetActive(true);
            swappedScreen = true;
            canBeEnded = true;
        }
    }

    
}
