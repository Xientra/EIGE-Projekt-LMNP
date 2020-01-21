using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MovementBase {

	private Quaternion cameraAnchorRotationOffset = Quaternion.identity;

	private Vector3 respawnPoint;
	public float deathZoneHeight = -50;

	[Header("Settings: ")]
	public PlayerSettings playerSettings;
	public InputSettings inputSettings;

	
	private Vector3 velocity;
	private Quaternion targetRotation;

	private float forwardInput = 0;
	private float sidewaysInput = 0;
	private Vector2 turnInput = Vector2.zero;
	private float jumpInput = 0;
	private bool lowerLowjumping = false;

	// this is for the stomp script, so it can modify movement
	public bool preventJumping = false;
	[HideInInspector]
	[Range(0f, 1f)]
	public float speedMultiplier = 1;
    public float dashSpeed = 0f;

	private void Awake() {
        setAttributes();

		respawnPoint = transform.position;

		targetRotation = transform.rotation;

		speedMultiplier = 1;
	}

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		cameraAnchorRotationOffset = transform.rotation;
	}

	// strg + k + d code convertierung

	void Update() {
		GetInput();

		UpdateCameraAnchor();
	}

	void FixedUpdate() {

		Turn();

		Move();

		Jump();


		UpdateCameraAnchor();


		// makeshift death zone
		if (transform.position.y < deathZoneHeight) {
			Respawn();
		}
	}


	void GetInput() {
		if (inputSettings.FORWARD_AXIS.Length != 0) forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);

		if (inputSettings.SIDEWAYS_AXIS.Length != 0) sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);

		if (inputSettings.TURN_AXIS_X.Length != 0) turnInput.x = Input.GetAxisRaw(inputSettings.TURN_AXIS_X);
		if (inputSettings.TURN_AXIS_Y.Length != 0) turnInput.y = -Input.GetAxisRaw(inputSettings.TURN_AXIS_Y);

		if (inputSettings.JUMP_AXIS.Length != 0) jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
	}

	void Move() {

        velocity.z = forwardInput * playerSettings.runVelocity + dashSpeed; //* speedMultiplier; //@Paul pls fix this
        velocity.x = sidewaysInput * playerSettings.runVelocity; //* speedMultiplier;

		velocity.y = playerRigidbody.velocity.y;
        
        playerRigidbody.velocity = transform.TransformDirection(velocity);
	}

	void Turn() {
		if (turnInput.x != 0f) {
			targetRotation *= Quaternion.AngleAxis(playerSettings.rotateVelocity * turnInput.x * Time.deltaTime, Vector3.up);
		}
		transform.rotation = targetRotation;

		if (turnInput.y != 0f) {
			cameraAnchorRotationOffset *= Quaternion.AngleAxis(playerSettings.rotateVelocity * turnInput.y * Time.deltaTime, Vector3.right);

			/*
			Vector3 eulerRot = cameraAnchorRotationOffset.eulerAngles;
			Debug.Log(eulerRot.x - 360);
			if (eulerRot.x < playerSettings.maxAngle + 360) {
				cameraAnchorRotationOffset = Quaternion.Euler(playerSettings.maxAngle, eulerRot.y, eulerRot.z);
			}
			if (eulerRot.x + 360 > playerSettings.minAngle + 360) {
				cameraAnchorRotationOffset = Quaternion.Euler(playerSettings.minAngle, eulerRot.y, eulerRot.z);
			}
			*/
		}
	}

	void UpdateCameraAnchor() {
		playerCamera.transform.rotation = transform.rotation * cameraAnchorRotationOffset;
	}

	void Jump() {
		if (preventJumping == false) {
			bool grounded = isGrounded();

			// if jump is pressed player is on ground
			if (jumpInput != 0f && grounded) {
				performJump();

				lowerLowjumping = false;
				// counts time to decide if player if lowerLowJumping
				StartCoroutine(LowerLowJump());
			}

			// this is still jumping but if jumping is not pressed add lowjumpForce
			if (jumpInput == 0 && grounded == false && playerRigidbody.velocity.y > 0) {

				playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y * playerSettings.lowJumpMultiplier, playerRigidbody.velocity.z);
			}

			if (lowerLowjumping == true) {
				playerRigidbody.AddForce(Vector3.down * playerSettings.shortHopForce);

				if (grounded)
					lowerLowjumping = false;
			}
		}
	}

	private IEnumerator LowerLowJump() {
		yield return new WaitForSeconds(playerSettings.shortHopTime);

		if (jumpInput <= 0)
			lowerLowjumping = true;
	}

	public void performJump() {
		performJump(1);
	}

	public void performJump(float jumpVelMultiplier) {
		playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerSettings.jumpVelocity * jumpVelMultiplier, playerRigidbody.velocity.z);
	}

	public void SetPreventJumpingAfterDelay(bool value, float delay) {
		StartCoroutine(SetPreventJumping(value, delay));
	}

	private IEnumerator SetPreventJumping(bool value, float delay) {
		yield return new WaitForSeconds(delay);
		preventJumping = value;
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("DeathCollider")) {
			Respawn();
		}
	}

	private void Respawn() {
		transform.position = respawnPoint;
		playerRigidbody.velocity = Vector3.zero;
    }

    public bool isGrounded()
    {
        float avgSize = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.95f;

        Vector3 boxSize = new Vector3(avgSize / 2, baseSettings.distanceToGround, avgSize / 2);

        bool hit = Physics.BoxCast(transform.position, boxSize / 2, -transform.up, transform.rotation, transform.lossyScale.y + boxSize.y / 2, baseSettings.ground);

        return hit;
    }
}

[System.Serializable]
public class PlayerSettings {

	public float runVelocity = 15;

	[Header("Rotation:")]
	public float rotateVelocity = 100;

	public float maxAngle = -80;
	public float minAngle = 80;

	[Header("Jumping:")]
	public float jumpVelocity = 8;

	[Tooltip("")]
	[Range(0f, 1f)]
	public float lowJumpMultiplier = 0.9f;

	[Tooltip("If the Jumpkey if held shorter than this time the jump will be extra low (shortHop).")]
	public float shortHopTime = 0.15f;

	[Tooltip("Additional gravity that is applied to the player if he is shortHopping.")]
	public float shortHopForce = 10f;
}

[System.Serializable]
public class InputSettings {
	public string FORWARD_AXIS = "Vertical";
	public string SIDEWAYS_AXIS = "Horizontal";
	public string TURN_AXIS_X = "Mouse X";
	public string TURN_AXIS_Y = "Mouse Y";
	public string JUMP_AXIS = "Jump";
}