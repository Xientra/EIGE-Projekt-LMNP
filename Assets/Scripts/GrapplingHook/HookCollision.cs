using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollision : MonoBehaviour
{
    private GrapplingHook grapplingHook;
    private void Start()
    {
        grapplingHook = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<GrapplingHook>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("NotHookable"))
        {
            grapplingHook.collidedObject = other.gameObject;
            grapplingHook.collisionDetected = true;
        }
    }
}
