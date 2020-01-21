using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MovementBase
{
    [Header("Settings:")]
    public DashSettings dashSettings;

    private void Awake()
    {
        setAttributes();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(dashSettings.dashKey))
        {
            Vector3 velocity = transform.localRotation.eulerAngles;
            

            print(velocity.ToString());

            velocity.Normalize();
            velocity *= dashSettings.dashForce;

            playerRigidbody.velocity += velocity;
        }
    }
}

[System.Serializable]
public class DashSettings
{
    public KeyCode dashKey = KeyCode.LeftShift;
    public int maxAmountOfDashes = 1;

    public float dashForce = 2f;
    public float dashDistance = 7f;
    public float dashTime = 1f;

    //Winkel, die der Spieler maximal nach oben oder unten dashen kann
    public float maxAngleUp = 90f;
    public float maxAngleDown = 10f;
}