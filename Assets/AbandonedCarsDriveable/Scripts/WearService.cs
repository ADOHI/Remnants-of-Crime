using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class WearService
    {
        private WearMaterial _originalMaterial;
        private WearMaterial _currentMaterial;

        private Material _invisibleMaterial;

        private float _levelRust;
        private float _levelDust;
        private float _levelPeelPaint;
        private float _levelMaskMoss;

        private static readonly int MaskRustGlobal = Shader.PropertyToID("_Mask_Rust_Global");
        private static readonly int MaskDustGlobal = Shader.PropertyToID("_Mask_Dust_Global");
        private static readonly int NormalPeelPaint = Shader.PropertyToID("_Normal_PeelPaint");
        private static readonly int MaskMoss = Shader.PropertyToID("_Mask_Moss");

        public WearService(WearMaterial originalMaterial, Material invisibleMaterial)
        {
            _invisibleMaterial = invisibleMaterial;
            _originalMaterial = originalMaterial;
            _currentMaterial = new WearMaterial();

            if (originalMaterial.InteriorMaterial != null)
                _currentMaterial.InteriorMaterial = new Material(_originalMaterial.InteriorMaterial);

            if (originalMaterial.WheelsMaterial != null)
                _currentMaterial.WheelsMaterial = new Material(_originalMaterial.WheelsMaterial);

            if (originalMaterial.EngineMaterial != null)
                _currentMaterial.EngineMaterial = new Material(_originalMaterial.EngineMaterial);

            if (originalMaterial.RailingsMaterial != null)
                _currentMaterial.RailingsMaterial = new Material(_originalMaterial.RailingsMaterial);

            if (originalMaterial.BodyMaterial != null)
                _currentMaterial.BodyMaterial = new Material(_originalMaterial.BodyMaterial);

            if (originalMaterial.FrontLightMaterialArray != null)
            {
                _currentMaterial.FrontLightMaterialArray = new Material[originalMaterial.FrontLightMaterialArray.Length];
                for (int i = 0; i < originalMaterial.FrontLightMaterialArray.Length; i++)
                {
                    _currentMaterial.FrontLightMaterialArray[i] = new Material(originalMaterial.FrontLightMaterialArray[i]);
                }
            }
            if (originalMaterial.RearLightMaterial != null)
            {
                _currentMaterial.RearLightMaterial = new Material(originalMaterial.RearLightMaterial);
            }
        
            if (originalMaterial.GlassBodyMaterial != null)
            {
                _currentMaterial.GlassBodyMaterial = new Material[originalMaterial.GlassBodyMaterial.Length];
                for (int i = 0; i < originalMaterial.GlassBodyMaterial.Length; i++)
                {
                    _currentMaterial.GlassBodyMaterial[i] = new Material(originalMaterial.GlassBodyMaterial[i]);
                }
            
            }

        }

        public void SetWearLevel(float levelRust, float levelDust, float levelPeelPaint, float levelMaskMoss)
        {
            _levelRust = levelRust;
            _levelDust = levelDust;
            _levelPeelPaint = levelPeelPaint;
            _levelMaskMoss = levelMaskMoss;
        }

        public void ReplaceMaterial(GameObject target, Material material, CarPart carPart, WheelState wheelState = WheelState.Normal)
        {
            if (target == null || material == null)
                return;

            switch (carPart)
            {
                case CarPart.Body:
                    _currentMaterial.BodyMaterial = new Material(material);
                    ApplyMaterialInstance(target, _originalMaterial.BodyMaterial, _currentMaterial.BodyMaterial);
                    break;
                case CarPart.Engine:
                    _currentMaterial.EngineMaterial = new Material(material);
                    ApplyMaterialInstance(target, _originalMaterial.EngineMaterial, _currentMaterial.EngineMaterial);
                    break;
                case CarPart.Interior:
                    _currentMaterial.InteriorMaterial = new Material(material);
                    ApplyMaterialInstance(target, _originalMaterial.InteriorMaterial, _currentMaterial.InteriorMaterial);
                    break;
                case CarPart.Railing:
                    _currentMaterial.RailingsMaterial = new Material(material);
                    ApplyMaterialInstance(target, _originalMaterial.RailingsMaterial, _currentMaterial.RailingsMaterial);
                    break;
                case CarPart.Light:
                    ReplaceRearLightMaterial(target, material);
                    break;
                case CarPart.Glass:
                    var glass= new Material(material);
                    ApplyMaterialInstance(target, material, glass);
                    break;

                case CarPart.Wheel:
                    switch (wheelState)
                    {
                        case WheelState.Normal:
                            _currentMaterial.WheelsMaterial = new Material(material);
                            break;
                        case WheelState.Deflated:
                            _currentMaterial.WheelsMaterial = new Material(material);
                            break;
                        case WheelState.Hidden:
                            _currentMaterial.WheelsMaterial = new Material(_invisibleMaterial);
                            break;
                    }
                    ApplyMaterialInstance(target, _originalMaterial.WheelsMaterial, _currentMaterial.WheelsMaterial);
                    break;
            }
        }
        public void ReplaceMaterial(GameObject target, Material[] materials, CarPart carPart)
        {
            if (target == null || materials == null)
                return;

            switch (carPart)
            {
                case CarPart.Light:
                    ReplaceFrontLightMaterials(target, materials);
                    break;
                case CarPart.GlassBody:
                    ReplaceGlassBodyMaterials(target, materials);
                    break;
          
            }
        }
    

        private void ReplaceFrontLightMaterials(GameObject target, Material[] materials)
        {
            if (materials.Length != _currentMaterial.FrontLightMaterialArray.Length)
                return;

            for (int i = 0; i < materials.Length; i++)
            {
                _currentMaterial.FrontLightMaterialArray[i] = new Material(materials[i]);
            }

            ApplyMaterialInstances(target, _originalMaterial.FrontLightMaterialArray, _currentMaterial.FrontLightMaterialArray);
        }
        private void ReplaceRearLightMaterial(GameObject target, Material material)
        {
            _currentMaterial.RearLightMaterial = new Material(material);
            ApplyMaterialInstance(target, _originalMaterial.RearLightMaterial, _currentMaterial.RearLightMaterial);
        }
        private void ReplaceGlassBodyMaterials(GameObject target, Material[] materials)
        {
            if (materials.Length != _currentMaterial.GlassBodyMaterial.Length)
                return;

            for (int i = 0; i < materials.Length; i++)
            {
                _currentMaterial.GlassBodyMaterial[i] = new Material(materials[i]);
            }

            ApplyMaterialInstances(target, _originalMaterial.GlassBodyMaterial, _currentMaterial.GlassBodyMaterial);
        }
        private void ApplyMaterialInstances(GameObject parentObject, Material[] targetMaterials, Material[] newMaterialInstances)
        {
            foreach (Transform child in parentObject.transform)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null)
                {
                    Material[] materials = renderer.sharedMaterials;
                    bool materialUpdated = false;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        for (int j = 0; j < targetMaterials.Length; j++)
                        {
                            if (materials[i].name == targetMaterials[j].name || materials[i].name == _invisibleMaterial?.name)
                            {
                                materials[i] = newMaterialInstances[j];
                                materials[i].SetFloat(MaskRustGlobal, _levelRust);
                                materials[i].SetFloat(MaskDustGlobal, _levelDust);
                                materials[i].SetFloat(NormalPeelPaint, _levelPeelPaint);
                                materials[i].SetFloat(MaskMoss, _levelMaskMoss);
                                materialUpdated = true;
                            }
                        }
                    }

                    if (materialUpdated)
                    {
                        renderer.sharedMaterials = materials;
                    }
                }

                if (child.childCount > 0)
                {
                    ApplyMaterialInstances(child.gameObject, targetMaterials, newMaterialInstances);
                }
            }
        }

        private void ApplyMaterialInstance(GameObject parentObject, Material targetMaterial, Material newMaterialInstance)
        {
            if (newMaterialInstance == null)
            {
                return;
            }

            newMaterialInstance.SetFloat(MaskRustGlobal, _levelRust);
            newMaterialInstance.SetFloat(MaskDustGlobal, _levelDust);
            newMaterialInstance.SetFloat(NormalPeelPaint, _levelPeelPaint);
            newMaterialInstance.SetFloat(MaskMoss, _levelMaskMoss);

            foreach (Transform child in parentObject.transform)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null)
                {
                    Material[] materials = renderer.sharedMaterials;
                    bool materialUpdated = false;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].name == targetMaterial.name || materials[i].name == _invisibleMaterial?.name)
                        {
                            materials[i] = newMaterialInstance;
                            materialUpdated = true;
                        }
                    }

                    if (materialUpdated)
                    {
                        renderer.sharedMaterials = materials;
                    }
                }

                if (child.childCount > 0)
                {
                    ApplyMaterialInstance(child.gameObject, targetMaterial, newMaterialInstance);
                }
            }
        }
    }
}
