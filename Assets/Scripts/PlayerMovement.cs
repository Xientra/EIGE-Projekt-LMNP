﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MovementBase {

	public Camera playerCamera;
	private Quaternion cameraAnchorRotationOffset = Quaternion.identity;

	public Vector3 respawnPoint;
	public float deathZoneHeight = -50;

	[Header("Settings: ")]
	public PlayerSettings playerSettings;
	public InputSettings inputSettings;

	private Rigidbody playerRigidbody;
	private Vector3 velocity;
	private Quaternion targetRotation;

	private float forwardInput = 0;
	private float sidewaysInput = 0;
	private Vector2 turnInput = Vector2.zero;
	private float jumpInput = 0;

	public bool preventJumping = false;

	private void Awake() {
		playerRigidbody = gameObject.GetComponent<Rigidbody>();

		if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
		if (respawnPoint == null) respawnPoint = transform.position;

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

		if (inputSettings.TURN_AXIS_X.Length != 0) turnInput.x = Input.GetAxisRaw(inputSettings.TURN_AXIS_X);
		if (inputSettings.TURN_AXIS_Y.Length != 0) turnInput.y = -Input.GetAxisRaw(inputSettings.TURN_AXIS_Y);

		if (inputSettings.JUMP_AXIS.Length != 0) jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
	}

	void Move() {
		velocity.z = forwardInput * playerSettings.runVelocity;
		velocity.x = sidewaysInput * playerSettings.runVelocity;

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
			bool _isGrounded = IsGrounded();

			// if jump is pressed player is on ground
			if (jumpInput != 0f && _isGrounded) {
				performJump();
			}

			// this is still jumping but if jumping is not pressed add lowjumpForce
			if (jumpInput == 0 && _isGrounded == false && playerRigidbody.velocity.y > 0) {
				//playerRigidbody.AddForce(Vector3.down * movementSettings.lowjumpForce);
				playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y * playerSettings.lowJumpMultiplier, playerRigidbody.velocity.z);
			}
		}
	}

	public void performJump() {
		performJump(1);
	}

	public void performJump(float jumpVelMultiplier) {
		playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerSettings.jumpVelocity * jumpVelMultiplier, playerRigidbody.velocity.z);
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

	[Tooltip("")]
	public float lowjumpFallingForce = 2f;
}

[System.Serializable]
public class InputSettings {
	public string FORWARD_AXIS = "Vertical";
	public string SIDEWAYS_AXIS = "Horizontal";
	public string TURN_AXIS_X = "Mouse X";
	public string TURN_AXIS_Y = "Mouse Y";
	public string JUMP_AXIS = "Jump";
}