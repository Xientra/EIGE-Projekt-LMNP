using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    [SerializeField]
    private KeyCode grappleButton = KeyCode.E;
    [SerializeField]
    private KeyCode stopGrapplingButton = KeyCode.Space;

    [SerializeField]
    private HookOfGrapple hook;
    [SerializeField]
    private Camera playerCamera;

    public bool grapplingAtm = false;

    private Quaternion hookRotaion;
    private Vector3 hookPosition;

    private void Awake()
    {
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();

        // Instantiate the Hook 
        // hook rotation to the playercameras rotation so we shoot in looking direction
        hookRotaion = playerCamera.transform.rotation;
        // hook position to the players position
        hookPosition = gameObject.transform.position;
        hook = Instantiate(hook, hookPosition, hookRotaion);
        // the hook needs his hookRb variable assigned here for some reason
        hook.hookRb = hook.GetComponent<Rigidbody>();
        // the hook needs certain variables of the player
        hook.GetComponent<HookOfGrapple>().player = this.gameObject;
        hook.playerRb = gameObject.GetComponent<Rigidbody>();
        hook.playerGrappleScript = gameObject.GetComponent<PlayerGrapple>();
        // disable the hook so noone sees it
        hook.gameObject.SetActive(false);
    }
    private void Update()
    {
        // Grapple gets shot
        if (Input.GetKeyDown(grappleButton) && !grapplingAtm)
        {
            shootGrappleHook();
        }
        // when player wants to stop grappling
        else if (hook.state == HookOfGrapple.State.Pulling && Input.GetKeyDown(stopGrapplingButton))
        {
            // it was stuck to an object
            // to make it stuck we make it its parent
            // this we change back to normal
            hook.transform.parent = null;
            // to make it stuck we also deleted its rigidbody
            hook.hookRb = hook.gameObject.AddComponent<Rigidbody>();
            // reset the pullingSpeed, which was exponentially increased
            hook.currentPullingSpeed = hook.startPullingSpeed;
            // disable the hook so noone sees it
            hook.gameObject.SetActive(false);
            // we're not grappling atm
            grapplingAtm = false;
        }

    }

    private void shootGrappleHook()
    {
        // we're grappling
        grapplingAtm = true;

        // update the hook rotation to the playercameras rotation so we shoot in looking direction
        hook.transform.rotation = playerCamera.transform.rotation;
        // update the hook position to the players position
        hook.transform.position = gameObject.transform.position;
        // update the startPosition for maxDistance calculation
        hook.startPosition = hookPosition;
        // set all its velocities to zero for safety reasons
        hook.hookRb.velocity = Vector3.zero;
        hook.hookRb.angularVelocity = Vector3.zero;
        // set its state to shooting
        hook.state = HookOfGrapple.State.Shooting;
        // enable it
        hook.gameObject.SetActive(true);
        
    }

}
