using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerMovement : MonoBehaviour
	{
		//TODO: Add sprint modifier.

		[SerializeField] private float movementSpeed;
		[SerializeField] private float jumpForce;
		[SerializeField] private float mouseSensitivity;

		private bool IsGrounded => Physics.Raycast(cachedTransform.position, Vector3.down, collider.bounds.extents.y + 0.01f);

		private Collider collider;
		private float cameraRotationX;
		private float cameraRotationY;
		private Rigidbody rigidbody;
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

		private void FixedUpdate() { Move(); }


		private void Move()
		{
			Vector3 movement = cameraTransform.right * Input.GetAxis("Horizontal") + cameraTransform.forward * Input.GetAxis("Vertical");
			movement.Normalize();

			rigidbody.position += movement * Time.deltaTime * movementSpeed;

			if (Input.GetAxis("Jump") > 0 && IsGrounded) { rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }
		}

		private void Rotate()
		{
			cameraRotationY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			cameraRotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
			cameraRotationX = Mathf.Clamp(cameraRotationX, -90.0f, 90.0f);
			
			cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0f);
		}
	}
}