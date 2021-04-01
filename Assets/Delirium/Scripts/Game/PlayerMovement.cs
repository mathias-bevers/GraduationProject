using System;
using System.Globalization;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerMovement : MonoBehaviour
	{
		private const float MAX_Y_VELOCITY = 5.5f;
		private const float MIN_Y_VELOCITY = -5.5f;
		
		[SerializeField] private float movementSpeed;
		[SerializeField] private float jumpForce;
		[SerializeField] private float mouseSensitivity;
		[SerializeField, Range(1.0f, 2.0f)] private float sprintMultiplier;
		public Transform CameraTransform { get; private set; }

		private bool IsGrounded
		{
			get
			{
				float maxDistance = collider.bounds.extents.y + 0.1f;
				Vector3 position = cachedTransform.position;

				// 0,0
				if (Physics.Raycast(position, Vector3.down, maxDistance)) { return true; }

				// 1,0
				if (Physics.Raycast(position + Vector3.right * collider.bounds.extents.x, Vector3.down, maxDistance)) { return true; }

				// -1,0
				if (Physics.Raycast(position + Vector3.left * collider.bounds.extents.x, Vector3.down, maxDistance)) { return true; }

				// 0,1
				return Physics.Raycast(position + Vector3.forward * collider.bounds.extents.z, Vector3.down, maxDistance) ||
				       // 0,-1
				       Physics.Raycast(position + Vector3.back * collider.bounds.extents.z, Vector3.down, maxDistance);
			}
		}

		private new Collider collider;
		private float cameraRotationX;
		private float cameraRotationY;
		private new Rigidbody rigidbody;
		private Transform cachedTransform;

		private void Awake()
		{
			cachedTransform = transform;
			CameraTransform = GetComponentInChildren<Camera>().transform;
			rigidbody = GetComponent<Rigidbody>();
			collider = GetComponent<Collider>();
		}

		private void Update()
		{
			if (MenuManager.Instance.IsAnyOpen) { return; }

			Rotate();
		}

		private void FixedUpdate()
		{
			if (MenuManager.Instance.IsAnyOpen) { return; }

			Move();
		}

		private void Move()
		{
			if (!IsGrounded) { return; }

			Vector3 movementInput = CameraTransform.right * Input.GetAxis("Horizontal") + CameraTransform.forward * Input.GetAxis("Vertical");
			movementInput.Normalize();
			movementInput *= Time.deltaTime * movementSpeed * (Input.GetAxis("Sprint") > 0 ? sprintMultiplier : 1);

			float yVelocity = Mathf.Clamp(rigidbody.velocity.y, MIN_Y_VELOCITY, MAX_Y_VELOCITY);

			rigidbody.velocity = new Vector3(movementInput.x, yVelocity, movementInput.z);

			if (Input.GetAxis("Jump") > 0 && IsGrounded) { rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }
		}

		private void Rotate()
		{
			cameraRotationY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

			cameraRotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
			cameraRotationX = Mathf.Clamp(cameraRotationX, -90.0f, 90.0f);

			CameraTransform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0.0f);
		}
	}
}