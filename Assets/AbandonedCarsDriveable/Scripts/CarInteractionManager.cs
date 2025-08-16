using System.Collections;
using UnityEngine;

#if UNITY_2023_1_OR_NEWER
using Unity.Cinemachine;
#else
using Cinemachine;
#endif

namespace AbandonedCarsDriveable.Scripts
{
    public class CarInteractionManager : MonoBehaviour
    {
        private DoorHandler player;
    
        [SerializeField] private CinemachineFreeLook carCamera;
        [SerializeField] private SimpleCarController carController;
        [SerializeField] private Door driverDoor;
        [SerializeField] private Transform exitPoint;

        private bool _lockInput = false;
    
        public bool InCar { get; private set; } = false;
    
        public void Init(DoorHandler player)
        {
            this.player = player;
        }

        private void Update()
        {
            if (InCar && _lockInput == false && Input.GetKeyUp(KeyCode.E))
            {
                ExitCar();
            }
        }

        public void EnterCar()
        {
            if (InCar || player == null) 
                return;

            StartCoroutine(EnterCarRoutine());
        }

        private IEnumerator EnterCarRoutine()
        {
            InCar = true;
            _lockInput = true;
            yield return new WaitForSeconds(0.5f);

            player.gameObject.SetActive(false);
        
            carController.SetPlayerInCar(true);
        
            player.Camera.enabled = false;
            carCamera.enabled = true;

            if (driverDoor != null)
            {
                driverDoor.CloseAfterEntry();
            }

            _lockInput = false;
            Debug.Log("Player entered the car.");
        }

        public void ExitCar()
        {
            if (!InCar) return;

            StartCoroutine(ExitCarRoutine());
        }

        private IEnumerator ExitCarRoutine()
        {
            _lockInput = true;
            carCamera.enabled = false;
            player.Camera.enabled = true;
        
            yield return new WaitForSeconds(0.1f);
            if (exitPoint != null)
            {
                player.transform.position = exitPoint.position;
                player.transform.rotation = exitPoint.rotation;
            }
            player.gameObject.SetActive(true);
        
            player.TryHandle();

            yield return new WaitForSeconds(.5f);
        
            player.TryHandle();
            InCar = false;
            player = null;
            carController.SetPlayerInCar(false);
            _lockInput = false;
        }
    }
}
