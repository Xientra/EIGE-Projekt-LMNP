using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    // Variables deciding GrapplingHook conditions
    [SerializeField]
    private KeyCode grappleButton = KeyCode.E;
    [SerializeField]
    private KeyCode stopGrapplingButton = KeyCode.Space;

    [SerializeField]
    private GameObject hookOrigin;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float shootingSpeed;
    [SerializeField]
    private float retractingSpeed;
    [SerializeField]
    private float maxhookDistance;
    [SerializeField]
    public float startPullingSpeed;
    [SerializeField]
    private float maxPullingSpeed;
    [SerializeField]
    private float growthExponent;

    // hook"clone" that we instantiate
    private GameObject hook;
    // Rigidbodies
    private Rigidbody playerRb;
    private Rigidbody hookRb;

    // Variables for preventing infinite hook
    private float distanceTravelled;
    private Vector3 startPosition;

    // Variable for currentPullingSpeed
    private float currentPullingSpeed;

    // Variables for positioning of hook
    private Quaternion hookRotaion;
    private Vector3 hookPosition;

    // GameObject hook collided with and bool to show when
    [HideInInspector]
    public GameObject collidedObject;
    [HideInInspector]
    public bool collisionDetected;

    // enum and corresponding Variable for hookingstate
    public enum State
    {
        waitingForShoot,
        Shooting,
        ComingBack,
        Pulling
    }
    public State state;

    private void Awake()
    {
        // get playerCamera for hook rotation
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
        // get playerRb for pulling
        playerRb = gameObject.GetComponent<Rigidbody>();
        // set currentPullingSpeed to startPullingSpeed
        currentPullingSpeed = startPullingSpeed;

    }
    private void Update()
    {
            switch (state)
            {
                case State.waitingForShoot:
                     // Grapple gets shot
                    if (Input.GetKeyDown(grappleButton))
                    {
                        shootGrappleHook();
                    }
                    break;
                case State.Shooting:
                    if(collisionDetected && !collidedObject.Equals(gameObject))
                    {
                        stuckTheHookToGameObject(collidedObject);
                        // reset collisionDetected to register new collisions
                        collisionDetected = false;
                        // reset collidedObject
                        collidedObject = null;
                        // next state
                        state = State.Pulling;
                    }
                    Shooting();
                    break;
                case State.ComingBack:
                     if (collisionDetected)
                     {
                        // Destroy grapplingHook
                        Destroy(hook);
                        // change state back
                        state = State.waitingForShoot;
                     }
                    ComingBack();
                    break;
                case State.Pulling:
                    // when player wants to stop grappling
                    if (Input.GetKeyDown(stopGrapplingButton))
                    {
                        Destroy(hook);
                        // reset state
                        state = State.waitingForShoot;
                    }

                    // Breack Connection if way(vision) is blocked
                    /*if(HookPlayerConnectionAvailable(player.transform.position)) {
                        StuckAndPulling(player);
                    } else
                    {
                        gameObject.SetActive(false);
                        player.GetComponent<PlayerGrapple>().grapplingAtm = false;
                    }*/


                    Pulling();
                    break;
                default:
                    break;
            }
    }

    private void shootGrappleHook()
    {
        // Instantiate the Hook 
        // hook rotation to the playercameras rotation so we shoot in looking direction
        hookRotaion = playerCamera.transform.rotation;
        // hook position to the players position
        hookPosition = gameObject.transform.position;
        hook = Instantiate(hookOrigin, hookPosition, hookRotaion,gameObject.transform);
        // update the startPosition for maxDistance calculation
        startPosition = hookPosition;
        // get the hooks rigidbody
        hookRb = hook.GetComponent<Rigidbody>();
        // change state
        state = State.Shooting;


    }
    private void Shooting()
    {
        // Prohibits infinite flying of the hook
        distanceTravelled = Vector3.Distance(startPosition, hook.transform.position);
        if (distanceTravelled > maxhookDistance)
        {
            state = State.ComingBack;
            // reset collisionDetection of Hook
            collisionDetected = false;
            // reset collidedObject
            collidedObject = null;
        }

        // flies in one direction
        // OnTriggerEnter changes state to stop the movement 
        hookRb.velocity = hook.transform.forward * shootingSpeed;
    }

    private void ComingBack()
    {
        // get the direction to the player
        Vector3 direction = gameObject.transform.position - hook.transform.position;
        direction.Normalize();

        // move hook to player
        // if way to is obstructed OnTriggerEnter will disable the hook
        hookRb.velocity = new Vector3(direction.x * retractingSpeed, direction.y * retractingSpeed, direction.z * retractingSpeed);
    }

    private void Pulling()
    {
        // get the direction the hook is pulling
        Vector3 playerPosition = gameObject.transform.position;
        Vector3 direction = hook.transform.position - playerPosition;
        direction.Normalize();
        

        // pull the player towards the hook
        playerRb.AddForce(new Vector3(direction.x * currentPullingSpeed, direction.y * currentPullingSpeed, direction.z * currentPullingSpeed));

        // exponential growth of speed just for fun
        if (currentPullingSpeed < maxPullingSpeed)
        {
            currentPullingSpeed = Mathf.Pow(currentPullingSpeed, 1 + growthExponent);
        }
    }

    private void stuckTheHookToGameObject(GameObject stuckTarget)
    {
        hookRb.velocity = Vector3.zero;
        hookRb.angularVelocity = Vector3.zero;
        hookRb.transform.parent = stuckTarget.transform;
        Destroy(hookRb);
    }
}
