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

    protected bool IsGrounded()
    {
        float avgSize = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.95f;

        Vector3 boxSize = new Vector3(avgSize / 2, baseSettings.distanceToGround, avgSize / 2);

        bool hit = Physics.BoxCast(transform.position, boxSize / 2, -transform.up, transform.rotation, transform.lossyScale.y + boxSize.y / 2, baseSettings.ground);

        return hit;
    }
}

[System.Serializable]
public class BaseSettings
{
    public float distanceToGround = 0.3f;
    public LayerMask ground = 8;
}