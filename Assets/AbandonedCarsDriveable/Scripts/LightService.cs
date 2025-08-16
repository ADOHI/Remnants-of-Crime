using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class LightService
    {
        private Material[] _frontLightMaterialInstance;
        private Material _rearLightMaterialInstance;
        private Material _rearViewCameraMaterialInstance;

        private GameObject _car;
        private GameObject _light;

        private static readonly int EmissivePower = Shader.PropertyToID("_EmissivePower");
        private static readonly int EmissivePowerCamera = Shader.PropertyToID("_EmissivePowerCamera"); 

        public LightService(GameObject car, GameObject light, Material rearLightMaterial, Material rearViewCameraMaterial, params Material[] frontLightMaterial)
        {
            _car = car;

            _frontLightMaterialInstance = new Material[frontLightMaterial.Length];
            if (frontLightMaterial != null)
            {
                for (int i = 0; i < frontLightMaterial.Length; i++)
                {
                    _frontLightMaterialInstance[i] = new Material(frontLightMaterial[i]);
                }
            }
            if (light != null) 
            {
                _light = light;
            }

            if (rearViewCameraMaterial != null)
            {
                _rearViewCameraMaterialInstance = new Material(rearViewCameraMaterial);
            }
            _rearLightMaterialInstance = new Material(rearLightMaterial);
        }
        public void SetEmissivePower(float frontEmissivePower, float rearEmissivePower)
        {
            if (_frontLightMaterialInstance == null || _rearLightMaterialInstance == null)
                return;
            ApplyEmissivePower(_car.transform, frontEmissivePower, _frontLightMaterialInstance);
            ApplyEmissivePower(_car.transform, rearEmissivePower, _rearLightMaterialInstance);
        }

        public void TurnOnLight(bool isTurn)
        {
            if(_light != null)
                _light.gameObject.SetActive(isTurn);
        } 
        public void SetEmissivePower(float frontEmissivePower, float rearEmissivePower, float cameraEmissivePower)
        {
            if (_frontLightMaterialInstance == null || _rearLightMaterialInstance == null || _rearViewCameraMaterialInstance == null)
                return;

            ApplyEmissivePower(_car.transform, frontEmissivePower, _frontLightMaterialInstance);
            ApplyEmissivePower(_car.transform, rearEmissivePower, _rearLightMaterialInstance);
            ApplyEmissivePower(_car.transform, cameraEmissivePower, _rearViewCameraMaterialInstance);
        }

        private void ApplyEmissivePower(Transform parent, float emissivePower, params Material[] targetMaterialInstance)
        {
            foreach (Transform child in parent.transform)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null)
                {
                    Material[] materials = renderer.sharedMaterials;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        for (int j = 0; j < targetMaterialInstance.Length; j++)
                        {
                            if (materials[i].name.Contains(targetMaterialInstance[j].name))
                            {
                                materials[i] = targetMaterialInstance[j];
                                targetMaterialInstance[j].SetFloat(EmissivePower, emissivePower);
                                targetMaterialInstance[j].SetFloat(EmissivePowerCamera, emissivePower); 
                            }
                        }
                    }

                    renderer.sharedMaterials = materials;
                }

                if (child.childCount > 0)
                {
                    ApplyEmissivePower(child, emissivePower, targetMaterialInstance);
                }
            }
        }
    }
}
