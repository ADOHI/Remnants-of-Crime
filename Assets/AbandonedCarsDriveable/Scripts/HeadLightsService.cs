using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class HeadLightsService : MonoBehaviour
    {
        [SerializeField] private Light[] headlights;
        [SerializeField] private Light[] brakeLights;
        [SerializeField] private Light[] reverseLights;

        [Header("Light Materials")]
        [SerializeField] private Material[] frontLightMaterials;
        [SerializeField] private Material rearLightMaterial;
        [SerializeField] private Material rearViewCameraMaterial;
        [SerializeField] private GameObject car;

        [Header("Emissive Intensity Limits")] 
        private float headlightEmissiveMin = 0.1f;
        private float headlightEmissiveMax = 2f;

        private float brakeEmissiveMin = 0.1f;
        private float brakeEmissiveMax = 2f;

        private float reverseEmissiveMin = 0f;
        private float reverseEmissiveMax = 2f;

        private float _frontEmissivePower;
        private float _rearEmissivePower;
        private float _rearViewCameraEmissivePower;
        private bool _turnOnLight;

        private LightService _lightService;

        private void Start()
        {
            if (car == null || frontLightMaterials == null || rearLightMaterial == null)
                return;

            _lightService = new LightService(car, null, rearLightMaterial, rearViewCameraMaterial, frontLightMaterials);
            SetHeadlightsState(_turnOnLight);
        }

        public void SetHeadlightsState(bool turnOnLight)
        {
            _turnOnLight = turnOnLight;
        }

        public void UpdateLights(float motorInput, bool isBraking, float steeringInput)
        {
            if (!_turnOnLight)
                return;

            if (Input.GetKeyDown(KeyCode.L))
            {
                bool headlightsEnabled = !headlights[0].enabled;

                foreach (var light in headlights)
                    light.enabled = headlightsEnabled;

                _frontEmissivePower = headlightsEnabled ? headlightEmissiveMax : headlightEmissiveMin;
            }

            foreach (var light in brakeLights)
                light.enabled = isBraking;

            _rearEmissivePower = isBraking ? brakeEmissiveMax : brakeEmissiveMin;

            bool isReversing = motorInput < 0;

            foreach (var light in reverseLights)
                light.enabled = isReversing;

            _rearViewCameraEmissivePower = isReversing ? reverseEmissiveMax : reverseEmissiveMin;

            _lightService.SetEmissivePower(_frontEmissivePower, _rearEmissivePower, _rearViewCameraEmissivePower);
        }

        public void SetEmissiveManually(float front, float brake, float reverse)
        {
            _frontEmissivePower = front;
            _rearEmissivePower = brake;
            _rearViewCameraEmissivePower = reverse;

            if (_lightService != null)
            {
                _lightService.SetEmissivePower(front, brake, reverse);
            }
        }


        public void SetEmissiveLimits(
            float headlightMax,
            float brakeMax,
            float reverseMax,
            float headlightMin = 0.1f,
            float brakeMin = 0.1f,
            float reverseMin = 0f)
        {
            headlightEmissiveMax = headlightMax;
            brakeEmissiveMax = brakeMax;
            reverseEmissiveMax = reverseMax;

            headlightEmissiveMin = headlightMin;
            brakeEmissiveMin = brakeMin;
            reverseEmissiveMin = reverseMin;
        }
    }
}
