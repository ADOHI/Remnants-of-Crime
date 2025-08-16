using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class SirenLightController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool autoMode = true; 

        [Header("Timing & Emission")]
        [SerializeField] private float switchInterval = 0.3f;
        [SerializeField] private float onEmission = 5f;
        [SerializeField] private float offEmission = 0f;

        [Header("Audio & Materials")]
        [SerializeField] private AudioSource sirenAudio;
        [SerializeField] private List<Material> targetMaterials;

        [Header("Manual Control (non-auto mode only)")]
        [SerializeField] private bool manualToggle = false;

        private bool _isEnabled = false;
        private bool _isToggled = false;
        private bool _lastManualToggleState = false;
        private Coroutine _coroutine;

        public void TurnOn() => _isEnabled = true;

        public void TurnOff()
        {
            _isEnabled = false;
            if (_isToggled)
                ToggleSiren();
        }

        public bool HasToggle => _isEnabled;
        private void Update()
        {
            if (autoMode) return;
            if (manualToggle == _lastManualToggleState) return;
            _lastManualToggleState = manualToggle;
            ApplyToggle(manualToggle);
        }

        public void ToggleSiren()
        {
            if (!autoMode || !_isEnabled)
                return;

            ApplyToggle(!_isToggled);
        }

        private void ApplyToggle(bool turnOn)
        {
            if (turnOn)
            {
                if (_isToggled) return;
                _isToggled = true;
                sirenAudio?.Play();
                _coroutine = StartCoroutine(SwitchEmission());
            }
            else
            {
                if (!_isToggled) return;
                _isToggled = false;
                sirenAudio?.Stop();
                foreach (var mat in targetMaterials)
                    SetEmission(mat, offEmission);
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

            }
        }

        private IEnumerator SwitchEmission()
        {
            while (_isToggled)
            {
                if (targetMaterials.Count >= 2)
                {
                    SetEmission(targetMaterials[0], onEmission);
                    SetEmission(targetMaterials[1], offEmission);
                }

                yield return new WaitForSeconds(switchInterval);

                if (!_isToggled) break;

                if (targetMaterials.Count >= 2)
                {
                    SetEmission(targetMaterials[0], offEmission);
                    SetEmission(targetMaterials[1], onEmission);
                }

                yield return new WaitForSeconds(switchInterval);
            }
        }

        private void SetEmission(Material targetMaterial, float power)
        {
            var renderers = GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                foreach (var mat in renderer.sharedMaterials)
                {
                    if (mat != null && mat.name.StartsWith(targetMaterial.name))
                        mat.SetFloat("_EmissivePower", power);
                }
            }
        }
    }
}
