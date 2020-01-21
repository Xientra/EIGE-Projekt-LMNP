using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBase : MonoBehaviour
{
    [Header("Base Settings:")]
    public BaseSettings baseSettings;

    public Camera playerCamera;
    protected Rigidbody playerRigidbody;

    protected void setAttributes()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();

        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
    }
}

[System.Serializable]
public class BaseSettings
{
    public float distanceToGround = 0.3f;
    public LayerMask ground = 8;
}