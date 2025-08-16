using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class WindowService
    {
        private GameObject[] _windows;
        private Material _originalUnbrokenMaterial;
        private Material _originalBrokenMaterial;
        private Material _curentMaterial;

        private Dictionary<int, Material> _unbrokenMaterials = new Dictionary<int, Material>();
        private Dictionary<int, Material> _brokenMaterials = new Dictionary<int, Material>();
        public bool _isBroken;

        public WindowService(GameObject[] windows, Material unbrokenMaterial, Material brokenMaterial,Material curentMaterial)
        {
            if (windows != null)
            {
                _windows = windows;
                _originalUnbrokenMaterial = unbrokenMaterial;
                _originalBrokenMaterial = brokenMaterial;

                for (int i = 0; i < _windows.Length; i++)
                {
                    _unbrokenMaterials[i] = new Material(_originalUnbrokenMaterial);
                    _brokenMaterials[i] = new Material(_originalBrokenMaterial);
                }
            }

            if (curentMaterial!=null)
            {
                _curentMaterial = new Material(curentMaterial);
            }

        }

        public void BreakWindow(int windowIndex, bool broken)
        {
            if (windowIndex < 0 || windowIndex >= _windows.Length) return;

            Renderer[] childRenderers = _windows[windowIndex].GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in childRenderers)
            {
                renderer.material = broken ? _brokenMaterials[windowIndex] : _unbrokenMaterials[windowIndex];
            }
        }

        public void SetUnbrokenOpacity(float opacity)
        {
            opacity = Mathf.Clamp(opacity, 0, 3);

            foreach (var material in _unbrokenMaterials.Values)
            {
                if (material != null)
                {
                    material.SetFloat("_Opacity_B", opacity);
                }
            }
        }
   
        public void SetBurntSettingForUnbrokenWindow()
        {
            foreach (var material in _unbrokenMaterials.Values)
            {
                if (material != null)
                {
                    material.SetFloat("_Brightness", 0.98f);          
                    material.SetFloat("_A", -0.18f);                
                    material.SetFloat("_B", 1.86f);                   
                    material.SetFloat("_Desaturation", 0);        
                    material.SetFloat("_Metallic", 1);            
                    material.SetFloat("_Opacity_A", 0.9f);        
                    material.SetFloat("_Opacity_B", 0.64f);
                }
            }
        }

        public void SetMoss(float moss)
        {
            foreach (var material in _unbrokenMaterials.Values.Where(material => material != null))
            {
                material.SetFloat("_Mask_Moss", moss);
            }

            foreach (var material in _brokenMaterials.Values.Where(material => material != null))
            {
                material.SetFloat("_Mask_Moss", moss);
            }
        }
        public void SetBurntSettingForBrokenWindow()
        {
            foreach (var material in _brokenMaterials.Values)
            {
                if (material != null)
                {
                    material.SetFloat("_Brightness", 0f);          
                    material.SetFloat("_A", -1f);                
                    material.SetFloat("_B", 3f);                   
                    material.SetFloat("_Desaturation", 0);        
                    material.SetFloat("_Metallic", 0);            
                    material.SetFloat("_Opacity_A", 0);        
                    material.SetFloat("_Opacity_B", 0.91f);
                }
            }
        }

        public void SetOpacity(GameObject parentObject, Material newMaterialInstance, float opacity, float moss, bool isBurnt)
        {
            ChangeMaterial(parentObject, _curentMaterial, new Material(newMaterialInstance), opacity , moss ,isBurnt);

        }
        private void ChangeMaterial(GameObject parentObject, Material targetMaterial, Material newMaterialInstance, float opacity, float moss,bool isBurnt)
        {
            if (newMaterialInstance == null)
            {
                return;
            }

            if (isBurnt)
            {
                if (_isBroken)
                {
                    newMaterialInstance.SetFloat("_Brightness", 0f);          
                    newMaterialInstance.SetFloat("_A", -1f);                
                    newMaterialInstance.SetFloat("_B", 3f);                   
                    newMaterialInstance.SetFloat("_Desaturation", 0);        
                    newMaterialInstance.SetFloat("_Metallic", 0);            
                    newMaterialInstance.SetFloat("_Opacity_A", 0);        
                    newMaterialInstance.SetFloat("_Opacity_B", 0.91f);
                }
                else
                {
                    newMaterialInstance.SetFloat("_Brightness", 0.98f);
                    newMaterialInstance.SetFloat("_A", -0.18f);
                    newMaterialInstance.SetFloat("_B", 1.86f);
                    newMaterialInstance.SetFloat("_Desaturation", 0);
                    newMaterialInstance.SetFloat("_Metallic", 1);
                    newMaterialInstance.SetFloat("_Opacity_A", 0.9f);
                    newMaterialInstance.SetFloat("_Opacity_B", 0.64f);
                }

            }
            else
            {
                newMaterialInstance.SetFloat("_Opacity_B", opacity);
                newMaterialInstance.SetFloat("_Mask_Moss", moss);
            }
        
            foreach (Transform child in parentObject.transform)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null)
                {
                    Material[] materials = renderer.sharedMaterials;
                    bool materialUpdated = false;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].name == targetMaterial.name)
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
                    SetOpacity(child.gameObject, newMaterialInstance, opacity, moss,isBurnt);
                }
            }
        }
    }
}