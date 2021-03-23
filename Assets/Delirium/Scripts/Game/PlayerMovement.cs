using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float movementSpeed;
		[SerializeField] private float jumpForce;
		[SerializeField] private float mouseSensitivity;
		[SerializeField, Range(1.0f, 2.0f)] private float sprintMultiplier;

		private bool IsGrounded => Physics.Raycast(cachedTransform.position, Vector3.down, collider.bounds.extents.y + 0.1f);

		private new Collider collider;
		private float cameraRotationX;
		private float cameraRotationY;
		private new Rigidbody rigidbody;
		private Transform cachedTransform;
		private Transform cameraTransform;

		private void Awake()
		{
			cachedTransform = transform;
			cameraTransform = GetComponentInChildren<Camera>().transform;
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
			
			Vector3 movementInput = cameraTransform.right * Input.GetAxis("Horizontal") + cameraTransform.forward * Input.GetAxis("Vertical");
			movementInput.Normalize();
			movementInput *= Time.deltaTime * movementSpeed * (Input.GetAxis("Sprint") > 0 ? sprintMultiplier : 1);

			rigidbody.velocity = new Vector3(movementInput.x, rigidbody.velocity.y, movementInput.z);


			if (Input.GetAxis("Jump") > 0 && IsGrounded) { rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }
		}

		private void Rotate()
		{
			cameraRotationY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

			cameraRotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
			cameraRotationX = Mathf.Clamp(cameraRotationX, -90.0f, 90.0f);

			cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0.0f);
		}
	}
}