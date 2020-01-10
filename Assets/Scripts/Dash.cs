using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MovementBase
{
    [Header("Settings:")]
    public DashSettings dashSettings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class DashSettings
{
    public KeyCode dashKey = KeyCode.LeftShift;
    public int maxAmountOfDashes = 1;

    public float dashDistance = 7f;
    public float dashTime = 1f;

    //Winkel, die der Spieler maximal nach oben oder unten dashen kann
    public float maxAngleUp = 90f;
    public float maxAngleDown = 10f;
}