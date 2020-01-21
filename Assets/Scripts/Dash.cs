using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MovementBase
{
    [Header("Settings:")]
    public DashSettings dashSettings;

    private PlayerMovement playerMovement;

    public int currentAmountOfDashesRemaining = 1;
    public float dashTimeRemaining = 0f;

    public bool isDashing = false;

    private void Awake()
    {
        setAttributes();
        playerMovement = GetComponent<PlayerMovement>();
        currentAmountOfDashesRemaining = dashSettings.maxAmountOfDashes;
        dashTimeRemaining = dashSettings.dashTime;
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
        if (Input.GetKeyDown(dashSettings.dashKey) && currentAmountOfDashesRemaining > 0 && !isDashing)
        {
            currentAmountOfDashesRemaining--;
            playerMovement.dashSpeed = dashSettings.dashSpeed;
            isDashing = true;
        }

        if (playerMovement.isGrounded())
        {
            currentAmountOfDashesRemaining = dashSettings.maxAmountOfDashes;
        }

        if(dashTimeRemaining >= 0 && isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
            playerMovement.dashSpeed = 0f;
            dashTimeRemaining = dashSettings.dashTime;
        }
    }
}

[System.Serializable]
public class DashSettings
{
    public KeyCode dashKey = KeyCode.LeftShift;
    public int maxAmountOfDashes = 1;

    public float dashSpeed = 80f;
    public float dashTime = 1f;
}