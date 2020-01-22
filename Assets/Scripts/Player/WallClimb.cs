using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MovementBase
{
    [Header("Settings:")]
    public WallClimbSettings wallClimbSettings;

    public bool isClimbing = false;
    private PlayerMovement playerMovement;

    public int currentNumberOfClimbs;
    public float climbTimeRemaining;

    private bool hitWall = false;

    private void Awake()
    {
        setAttributes();
        playerMovement = GetComponent<PlayerMovement>();

        currentNumberOfClimbs = wallClimbSettings.maxNumberOfClimbs;
        climbTimeRemaining = wallClimbSettings.maxTimePerClimb;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(wallClimbSettings.climbKey) && currentNumberOfClimbs > 0 && !isClimbing && hitWall)
        {
            print("hit wall");

            isClimbing = true;

            currentNumberOfClimbs--;

            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        

        if (playerMovement.isGrounded())
        {
            currentNumberOfClimbs = wallClimbSettings.maxNumberOfClimbs;
        }

        if (isClimbing)
        {
            if (Input.GetKeyDown(wallClimbSettings.jumpKey))
            {
                playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                isClimbing = false;

                playerRigidbody.velocity = new Vector3(0, playerMovement.playerSettings.jumpVelocity, 0);

                climbTimeRemaining = wallClimbSettings.maxTimePerClimb;
            }

            if (climbTimeRemaining >= 0)
            {
                climbTimeRemaining -= Time.deltaTime;
            }
            else
            {
                playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                isClimbing = false;

                climbTimeRemaining = wallClimbSettings.maxTimePerClimb;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            hitWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            hitWall = false;
        }
    }
}

[System.Serializable]
public class WallClimbSettings
{
    public LayerMask wallClimbMask;
    public KeyCode climbKey = KeyCode.F;
    public KeyCode jumpKey = KeyCode.Space;

    public int maxNumberOfClimbs = 2;
    public float maxTimePerClimb = 10f;

    public Vector3 boxDimensions = new Vector3(2, 0.5f, 2);
}
