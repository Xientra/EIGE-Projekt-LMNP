using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IMPORTED_PlayerMovement : MonoBehaviour {

	public Camera playerCamera;
	private Quaternion cameraAnchorRotationOffset = Quaternion.identity;

	public float deathZoneHeight = -50;

	[Header("Settings: ")]
	public MovementSettings movementSettings;
	public InputSettings inputSettings;


	private Rigidbody playerRigidbody;
	private Vector3 velocity;
	private Quaternion targetRotation;

	private float forwardInput = 0;
	private float sidewaysInput = 0;
	private Vector2 turnInput = Vector2.zero;
	private float jumpInput = 0;




	private void Awake() {
		playerRigidbody = gameObject.GetComponent<Rigidbody>();

		if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();

		targetRotation = transform.rotation;
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
			transform.position = new Vector3(0, 3, 0);
			playerRigidbody.velocity = Vector3.zero;
		}
	}


	void GetInput() {
		if (inputSettings.FORWARD_AXIS.Length != 0) forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);

		if (inputSettings.SIDEWAYS_AXIS.Length != 0) sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);

		if (inputSettings.TURN_AXIS_X.Length != 0) turnInput.x = Input.GetAxis(inputSettings.TURN_AXIS_X);
		if (inputSettings.TURN_AXIS_Y.Length != 0) turnInput.y = -Input.GetAxis(inputSettings.TURN_AXIS_Y);

		if (inputSettings.JUMP_AXIS.Length != 0) jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
	}

	void Move() {
		velocity.z = forwardInput * movementSettings.runVelocity;
		velocity.x = sidewaysInput * movementSettings.runVelocity;

		velocity.y = playerRigidbody.velocity.y;
		playerRigidbody.velocity = transform.TransformDirection(velocity);
	}

	void Turn() {
		if (turnInput.x != 0f) {
			targetRotation *= Quaternion.AngleAxis(movementSettings.rotateVelocity * turnInput.x * Time.deltaTime, Vector3.up);
		}
		transform.rotation = targetRotation;

		if (turnInput.y != 0f) {
			cameraAnchorRotationOffset *= Quaternion.AngleAxis(movementSettings.rotateVelocity * turnInput.y * Time.deltaTime, Vector3.right);
		}
	}

	void UpdateCameraAnchor() {

		playerCamera.transform.rotation = transform.rotation * cameraAnchorRotationOffset;
	}

	void Jump() {
		bool _isGrounded = IsGrounded();

		// if jump is pressed player is on ground
		if (jumpInput != 0f && _isGrounded) {
			performJump();
		}

		// this is still jumping but if jumping is not pressed add lowjumpForce
		if (jumpInput == 0 && _isGrounded == false && !(playerRigidbody.velocity.y < 0)) {
			// playerRigidbody.AddForce(-transform.up * movementSettings.lowjumpForce);
			playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y * movementSettings.lowJumpMultiplier, playerRigidbody.velocity.z);
		}

		if (_isGrounded == true) {
			movementSettings.jumpingMovementSpeed = 1;
		}
		else {
			movementSettings.jumpingMovementSpeed = movementSettings.jumpingMovementSpeedMultiplier;
		}
	}

	public void performJump() {
		performJump(1);
	}

	public void performJump(float jumpVelMultiplier) {
		playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, movementSettings.jumpVelocity * jumpVelMultiplier, playerRigidbody.velocity.z);
	}

	bool IsGrounded() {

		float avgSize = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.95f;

		Vector3 boxSize = new Vector3(avgSize / 2, movementSettings.distanceToGround, avgSize / 2);

		bool hit = Physics.BoxCast(transform.position, boxSize / 2, -transform.up, transform.rotation, transform.lossyScale.y + boxSize.y / 2, movementSettings.ground);

		return hit;
	}
}

[System.Serializable]
public class MovementSettings {
	public float runVelocity = 12;

	public float rotateVelocity = 100;

	[Header("Jumping: ")]
	public float jumpVelocity = 8;
	public float distanceToGround = 1.3f;
	public LayerMask ground = 0;
	public float lowjumpForce = 10f;
	[Range(0f, 1f)]
	public float lowJumpMultiplier = 0.9f;
	public float additionalFallingForce = 2f;
	[Range(0f, 1f)]
	public float jumpingMovementSpeedMultiplier = 0.5f;
	[HideInInspector]
	public float jumpingMovementSpeed = 1;

	[Header("Dashing: ")]

	public float dashDistance = 7f;
	public float dashTime = 1f;
}

[System.Serializable]
public class InputSettings {
	public string FORWARD_AXIS = "Vertical";
	public string SIDEWAYS_AXIS = "Horizontal";
	public string TURN_AXIS_X = "Mouse X";
	public string TURN_AXIS_Y = "Mouse Y";
	public string JUMP_AXIS = "Jump";
}