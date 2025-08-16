using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    [RequireComponent(typeof(EngineSoundService), typeof(HeadLightsService))]
    public class SimpleCarController : MonoBehaviour
    {
        [SerializeField] private WheelCollider frontLeftWheel;
        [SerializeField] private WheelCollider frontRightWheel;
        [SerializeField] private WheelCollider rearLeftWheel;
        [SerializeField] private WheelCollider rearRightWheel;

        [SerializeField] private Transform frontLeftTransform;
        [SerializeField] private Transform frontRightTransform;
        [SerializeField] private Transform rearLeftTransform;
        [SerializeField] private Transform rearRightTransform;

        [SerializeField] private float motorForce = 1500f;
        [SerializeField] private float brakeForce = 3000f;
        [SerializeField] private float maxSteeringAngle = 30f;
        [SerializeField] private float decelerationForce = 1000f;
        [SerializeField] private SirenLightController _sirenController;

        private float currentBrakeForce = 0f;
     

        private EngineSoundService engineSoundService;
        private HeadLightsService _headLightsService;
    
        private bool isBraking = false;
        private bool isCarStarted = false;
        private bool isPlayerInCar = false; 
    
        public void SetPlayerInCar(bool inCar)
        {
            isPlayerInCar = inCar;
        }

        private void Start()
        {
            engineSoundService = GetComponent<EngineSoundService>();
            _headLightsService = GetComponent<HeadLightsService>();
        }

        void Update()
        {
            if (isPlayerInCar && !isCarStarted)
            {
                isCarStarted = true;
                engineSoundService.PlayEngineStartSound();
            }
            else if (isCarStarted && !isPlayerInCar)
            {
                isCarStarted = false;
                engineSoundService.Stop();
            }

            if (isPlayerInCar && _sirenController != null && _sirenController.HasToggle)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    _sirenController.ToggleSiren();
                }
            }

            if (!isCarStarted) return;

            float motorInput = Input.GetAxis("Vertical");
            float steeringInput = Input.GetAxis("Horizontal");

            float speed = GetComponent<Rigidbody>().linearVelocity.magnitude * 3.6f;
            float maxSpeed = 220f;

            HandleMotor(motorInput);
            HandleSteering(steeringInput);
            UpdateWheelVisuals();

            engineSoundService.UpdateEngineSoundWithSpeed(speed, maxSpeed);
            _headLightsService.UpdateLights(motorInput, isBraking, steeringInput);
        }

        private void HandleMotor(float motorInput)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            float speed = rb.linearVelocity.magnitude;
            bool isMovingForward = Vector3.Dot(rb.linearVelocity, transform.forward) > 0;

            switch (motorInput)
            {
                case > 0 when !isMovingForward && speed > 0.1f:
                case < 0 when isMovingForward && speed > 0.1f:
                    ApplyBraking(true); 
                    ClearMotorTorque(); 
                    return;
                case 0:
                    ApplyDeceleration();
                    return;
            }

            ApplyBraking(false);
            float motorTorque = motorInput * motorForce;
            rearLeftWheel.motorTorque = motorTorque;
            rearRightWheel.motorTorque = motorTorque;
        }

        private void HandleSteering(float steeringInput)
        {
            float steeringAngle = steeringInput * maxSteeringAngle;
            frontLeftWheel.steerAngle = steeringAngle;
            frontRightWheel.steerAngle = steeringAngle;
        }

        private void ApplyBraking(bool braking)
        {
            isBraking = braking;
            currentBrakeForce = braking ? brakeForce : 0f;

            rearLeftWheel.brakeTorque = currentBrakeForce;
            rearRightWheel.brakeTorque = currentBrakeForce;
            frontLeftWheel.brakeTorque = currentBrakeForce;
            frontRightWheel.brakeTorque = currentBrakeForce;
        }

        private void ApplyDeceleration()
        {
            currentBrakeForce = decelerationForce;

            rearLeftWheel.brakeTorque = currentBrakeForce;
            rearRightWheel.brakeTorque = currentBrakeForce;
            frontLeftWheel.brakeTorque = currentBrakeForce;
            frontRightWheel.brakeTorque = currentBrakeForce;
        }

        private void ClearMotorTorque()
        {
            rearLeftWheel.motorTorque = 0f;
            rearRightWheel.motorTorque = 0f;
        }

        private void UpdateWheelVisuals()
        {
            UpdateWheel(frontLeftWheel, frontLeftTransform);
            UpdateWheel(frontRightWheel, frontRightTransform);
            UpdateWheel(rearLeftWheel, rearLeftTransform);
            UpdateWheel(rearRightWheel, rearRightTransform);
        }

        private void UpdateWheel(WheelCollider collider, Transform transform)
        {
            collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            transform.position = pos;
            transform.rotation = rot;
        }
    
    }
}
