#if UNITY_2023_1_OR_NEWER
using UnityEngine.InputSystem;
#endif

using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class FPSController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float groundDistance = 0.4f;

#if UNITY_2023_1_OR_NEWER
        private PlayerInput playerInput; 
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        
#endif

        private float xRotation = 0f;
        private CharacterController controller;
        private Vector3 velocity;
        private bool isGrounded;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            controller = GetComponent<CharacterController>();

#if UNITY_2023_1_OR_NEWER
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            jumpAction = playerInput.actions["Jump"];
#endif
        }

        void Update()
        {
            HandleMouseLook();
            HandleMovement();
        }

        void HandleMouseLook()
        {
#if UNITY_2023_1_OR_NEWER
            Vector2 lookInput = lookAction.ReadValue<Vector2>();
            float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
#else
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
#endif
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        void HandleMovement()
        {
            isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

#if UNITY_2023_1_OR_NEWER
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            float moveX = moveInput.x;
            float moveZ = moveInput.y;
#else
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
#endif
            Vector3 moveDirection = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
            moveDirection.y = 0f;

            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

#if UNITY_2023_1_OR_NEWER
            if (jumpAction.triggered && isGrounded)
#else
            if (Input.GetButtonDown("Jump") && isGrounded)
#endif
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
