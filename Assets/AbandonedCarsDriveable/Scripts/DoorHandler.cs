#if UNITY_2023_1_OR_NEWER
using Unity.Cinemachine;
using UnityEngine.InputSystem;
#else
using Cinemachine;
using UnityEngine.InputSystem;
#endif
using UnityEngine;



namespace AbandonedCarsDriveable.Scripts
{
    public class DoorHandler : MonoBehaviour
    {
#if UNITY_2023_1_OR_NEWER
        [SerializeField] private InputActionReference interactAction; 
#else
        [SerializeField] private KeyCode _interactableKey = KeyCode.E;
#endif
        [SerializeField] private CinemachineVirtualCamera _camera;

        private DoorCollider _doorCollider;
        public CinemachineVirtualCamera Camera => _camera;

#if UNITY_2023_1_OR_NEWER
        private void OnEnable()
        {
            interactAction.action.Enable();
            interactAction.action.performed += HandleInteraction;
        }

        private void OnDisable()
        {
            interactAction.action.performed -= HandleInteraction;
            interactAction.action.Disable();
        }

        private void HandleInteraction(InputAction.CallbackContext context)
        {
            _doorCollider?.Handle(this);
        }
#else
        private void Update()
        {
            if (Input.GetKeyUp(_interactableKey))
                _doorCollider?.Handle(this);
        }
#endif

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DoorCollider doorCollider))
            {
                _doorCollider = doorCollider;
            }
        }
        public void TryHandle()
        {
            if (_doorCollider)
                _doorCollider?.Handle(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_doorCollider != null && other.gameObject == _doorCollider.gameObject)
            {
                _doorCollider = null;
            }
        }
    }
}

