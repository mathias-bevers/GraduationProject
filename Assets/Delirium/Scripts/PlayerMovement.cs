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

        private new Collider collider;
        private float cameraRotationX;
        private new Rigidbody rigidbody;
        private Transform cachedTransform;
        private Transform cameraTransform;

        private void Awake()
        {
            cachedTransform = transform;
            cameraTransform = GetComponentInChildren<Camera>().transform;
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            //TODO: Add a can move 
            if (Cursor.visible) { return; }

            Rotate();
        }

        private void FixedUpdate()
        {
            if (Cursor.visible) { return; }
            
            Move();
        }


        private void Move()
        {
            Vector3 movement = cachedTransform.right * Input.GetAxis("Horizontal") + cachedTransform.forward * Input.GetAxis("Vertical");

            rigidbody.position += movement * Time.deltaTime * movementSpeed;

            if (Input.GetAxis("Jump") > 0 && IsGrounded) { rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }
        }

        private void Rotate()
        {
            cachedTransform.Rotate(Vector3.up * (Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime));
			
            cameraRotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90.0f, 90.0f);
            cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
        }
    }
}