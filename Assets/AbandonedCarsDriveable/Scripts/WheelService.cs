using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class WheelService
    {
        private const float HEIGHT_NORMAL_WHEEL = 0.362f;
        private const float HEIGHT_DEFLATED_WHEEL = 0.285f;
        private const float HEIGHT_HIDDEN_WHEEL = 0.178f;

        private GameObject _frontLeftWheel;
        private GameObject _frontRightWheel;
        private GameObject _rearLeftWheel;
        private GameObject _rearRightWheel;

        private Mesh[] _originalWheelMeshes;
        private Mesh[] _deflatedWheelMeshes;
        private WearService _wearService;

        public WheelService(GameObject frontLeftWheel, GameObject frontRightWheel, GameObject rearLeftWheel, GameObject rearRightWheel,
            Mesh[] deflatedWheelMeshes, Mesh[] originalWheelMeshes, WearService wearService)
        {
            _frontLeftWheel = frontLeftWheel;
            _frontRightWheel = frontRightWheel;
            _rearLeftWheel = rearLeftWheel;
            _rearRightWheel = rearRightWheel;

            _originalWheelMeshes = originalWheelMeshes;
            _deflatedWheelMeshes = deflatedWheelMeshes;
            _wearService = wearService;
        }

        public void RotateAllWheels(float rotationFront)
        {
            RotateWheel(_frontLeftWheel, rotationFront);
            RotateWheel(_frontRightWheel, rotationFront);
        }

        public void SetWheelState(GameObject wheel, WheelState state)
        {
            switch (state)
            {
                case WheelState.Normal:
                    DeflateWheel(wheel, false);
                    break;
                case WheelState.Deflated:
                    DeflateWheel(wheel, true);
                    break;
                case WheelState.Hidden:
                    break;
            }

            SetHeightWheel(wheel, state);
        }

        private static void SetHeightWheel(GameObject wheel, WheelState state)
        {
            Vector3 wheelLocalPosition = wheel.transform.localPosition;

            switch (state)
            {
                case WheelState.Normal:
                    wheelLocalPosition.y = HEIGHT_NORMAL_WHEEL;
                    break;
                case WheelState.Deflated:
                    wheelLocalPosition.y = HEIGHT_DEFLATED_WHEEL;
                    break;
                case WheelState.Hidden:
                    wheelLocalPosition.y = HEIGHT_HIDDEN_WHEEL;
                    break;
            }

            wheel.transform.localPosition = wheelLocalPosition;
        }

        private void RotateWheel(GameObject wheel, float rotationAngle)
        {
            if (wheel == _frontLeftWheel || wheel == _rearLeftWheel)
                wheel.transform.localRotation = Quaternion.Euler(0, -rotationAngle, 0);
            else if (wheel == _frontRightWheel || wheel == _rearRightWheel)
                wheel.transform.localRotation = Quaternion.Euler(0, 180 - rotationAngle, 0);
        }

        private void DeflateWheel(GameObject wheel, bool deflated)
        {
            Mesh[] targetMeshes = deflated ? _deflatedWheelMeshes : _originalWheelMeshes;
            SetWheelMeshes(wheel, targetMeshes);
        }

        private void SetWheelMeshes(GameObject wheel, Mesh[] meshes)
        {
            if (wheel == null || meshes == null || meshes.Length < 4) return;

            if (Application.isPlaying)
            {
                ApplyMeshChanges(wheel, meshes);
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    if (wheel != null)
                    {
                        ApplyMeshChanges(wheel, meshes);
                    }
                };
#endif
            }
        }

        private void ApplyMeshChanges(GameObject wheel, Mesh[] meshes)
        {
            for (int i = 0; i < 4; i++)
            {
                Transform child = wheel.transform.GetChild(i);
                MeshFilter meshFilter = child.GetComponent<MeshFilter>();

                if (meshFilter != null)
                {
                    meshFilter.sharedMesh = meshes[i];
                }
            }
        }

    }
}
