using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookOfGrapple : MonoBehaviour
{
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

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Rigidbody playerRb;
    [HideInInspector]
    public PlayerGrapple playerGrappleScript;
    [HideInInspector]
    public Rigidbody hookRb;
    [HideInInspector]
    public float currentPullingSpeed;
    [HideInInspector]
    public Vector3 startPosition;

    private float distanceTravelled;
    
    public enum State
    {
        Shooting,
        ComingBack,
        Pulling
    }
    public State state;

    private void Awake()
    {
        hookRb = gameObject.GetComponent<Rigidbody>();

        currentPullingSpeed = startPullingSpeed;
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case State.Shooting:
                Shooting();
                break;
            case State.ComingBack:
                ComingBack(player.transform.position);
                break;
            case State.Pulling:


                // Breack Connection if way(vision) is blocked
                /*if(HookPlayerConnectionAvailable(player.transform.position)) {
                    StuckAndPulling(player);
                } else
                {
                    gameObject.SetActive(false);
                    player.GetComponent<PlayerGrapple>().grapplingAtm = false;
                }*/


                Pulling(player);
                break;
        }
    }

    private void Shooting()
    {
        // Prohibits infinite flying of the hook
        distanceTravelled = Vector3.Distance(startPosition, gameObject.transform.position); 
        if(distanceTravelled > maxhookDistance)
        {
            state = State.ComingBack;
        }

        // flies in one direction
        // OnTriggerEnter changes state to stop the movement 
        hookRb.velocity = gameObject.transform.forward * shootingSpeed;
    }

    private void ComingBack(Vector3 playerPosition)
    {
        // get the direction to the player
        Vector3 direction = playerPosition - gameObject.transform.position;
        direction.Normalize();

        // move hook to player
        // if way to is obstructed OnTriggerEnter will disable the hook
        hookRb.velocity = new Vector3(direction.x * retractingSpeed, direction.y * retractingSpeed, direction.z * retractingSpeed);
    }

    private void Pulling(GameObject player)
    {
        // get the direction the hook is pulling
        Vector3 playerPosition = player.transform.position;
        Vector3 direction = gameObject.transform.position - playerPosition;
        direction.Normalize();

        // pull the player towards the hook
        playerRb.AddForce(new Vector3(direction.x * currentPullingSpeed, direction.y * currentPullingSpeed, direction.z * currentPullingSpeed));

        // exponential growth of speed just for fun
        if (currentPullingSpeed < maxPullingSpeed)
        {
            currentPullingSpeed = Mathf.Pow(currentPullingSpeed, 1 + growthExponent);
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // Destroys this hook-instance if it collides with walls while coming back
        if(state == State.ComingBack)
        {
            // we're not grappling anymore
            playerGrappleScript.grapplingAtm = false;
            // reset the speed, which was exponetially increased
            currentPullingSpeed = startPullingSpeed;
            // set the state back to shooting for safety reasons
            state = State.Shooting;
            // disable the hook, so noone sees it
            gameObject.SetActive(false);
        // Stucks the hook to the collided object if it's not the player
        } else if (!collision.collider.tag.Equals("Player"))
        {
            // to make the hook stuck: all velocities to zero and make the collision-object its parent 
            // and delete the hooks rigidbody
            hookRb.velocity = Vector3.zero;
            hookRb.angularVelocity = Vector3.zero;
            hookRb.transform.parent = collision.collider.transform;
            Destroy(hookRb);

            // change state to pulling
            state = State.Pulling;
        }
    }

    // Test if vision is blocked
    /*private bool HookPlayerConnectionAvailable(Vector3 playerPosition)
    {
        Vector3 directionToPlayer = playerPosition - gameObject.transform.position;
        directionToPlayer.Normalize();

        // send out a raycast to test for a wall-free-connection between hook and player
        Physics.Raycast(gameObject.transform.position, directionToPlayer, out RaycastHit hit);

        bool unnecessaryBoolThatOtherwiseWouldCauseAnError = hit.collider.tag.Equals("Player");
        Debug.Log(unnecessaryBoolThatOtherwiseWouldCauseAnError);
        return unnecessaryBoolThatOtherwiseWouldCauseAnError;
    }*/
}
