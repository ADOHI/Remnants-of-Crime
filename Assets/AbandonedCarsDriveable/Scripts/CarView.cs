using System;
using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class CarView : MonoBehaviour
    {
        // Models
        [SerializeField] private GameObject carBody;
        [SerializeField] private Door leftDoor;
        [SerializeField] private Door rightDoor;
        [SerializeField] private Door rearLeftTrunkDoor;
        [SerializeField] private Door rearRightTrunkDoor;
        [SerializeField] private Door rearRightDoor;
        [SerializeField] private Door rearLeftDoor;
        [SerializeField] public Door trunkDoor;
        [SerializeField] public Door trunkDoorTop;
        [SerializeField] public Door slidingDoorObject;
        [SerializeField] private Door hood;
        [SerializeField] private GameObject holder;
        [SerializeField] private GameObject[] mossOnCar;
        [SerializeField] private SirenLightController frontLightBar;     
        [SerializeField] private GameObject frontProtection;

        //Materials
        [SerializeField] private Material bodyMaterial;
        [SerializeField] private Material brokenWindowMaterial;
        [SerializeField] private Material normalWindowMaterial;
        [SerializeField] private Material wheelMaterial;
        [SerializeField] private Material invisibleMaterial;
        [SerializeField] private Material originalBodyMaterial;
        [SerializeField] private Material glassMaterial;
        [SerializeField] private Material interiorMaterial;
        [SerializeField] private Material engineMaterial;
        [SerializeField] private Material railingsMaterial;
        [SerializeField] private Material[] frontLightMaterial;
        [SerializeField] private Material rearLightMaterial;
        [SerializeField] public Material[] setOfMaterials;
        [SerializeField] public DecalSet[] decalMaterials; 
        [SerializeField] private Material burntWheelMaterial; 
        [SerializeField] private Material burntEngineMaterial; 
        [SerializeField] private Material burntInteriorMaterial; 
        [SerializeField] private Material[] burntFrontLightMaterial;
        [SerializeField] private Material burntRearLightMaterial;
        [SerializeField]private Material burntRailingsMaterial;
        [SerializeField]private Material burntBodyMaterial;

        [SerializeField] public bool isUniqueInterior; 
        [SerializeField] private Material[] interiorMaterials;

        [SerializeField] private SimpleCarController carController;

        [SerializeField] private bool changeSettings ;
   
        //Exterior Settings
        [SerializeField, Range(0,4)] int setOfMaterialIndex ;
        [SerializeField, Range(0,3)]public int selectedDecalIndex ;
        [SerializeField] private bool synchronizeWearLevel;
        [SerializeField, Range(0, 1)] private float wearLevel;
        [SerializeField, Range(0, 1)] private float wearLevelDust;
        [SerializeField, Range(0, 1)] private float wearLevelRust;
        [SerializeField, Range(0, 1)] private float wearLevelGlass;
        [SerializeField, Range(0, 1)] private float levelMaskMoss;
        [SerializeField, Range(0, 1)] private float levelPeelPaint;
        [SerializeField] private float headlightEmissiveMax = 2;
        [SerializeField] private float brakeEmissiveMax = 2;
        [SerializeField] private float reverseEmissiveMax = 2;
        [SerializeField] private bool isBurnt;
        [SerializeField] private bool turnOnLight;
        [SerializeField] private bool showMossOnCar;
        [SerializeField] public bool has3DBodyKit;
        [SerializeField] public bool showFrontProtection;
        [SerializeField] public bool showLightBar;
        [SerializeField] public bool turnOnSiren;
    

        //Window Settings
        [SerializeField] private bool windowIsBroken;

        private Material _currentRailingsMaterial;
        private Material _currentEngineMaterial;
        private Material _currentInteriorMaterial;
        private Material _currentWheelMaterial;
        private Material _currentWindowMaterial;
        private Material _currentLightMaterial; 
        private Material[] _currentFrontLightMaterial; 
        private Material _currentRearLightMaterial;
    
        private WearService _wearService;
        private WindowService _windowService;
        private DoorService _doorService;
        private LightService _lightService;
    
        private float _windowOpacity;
        private bool _valuesSaved;
       


        private void Start()
        {
            InitializeServices();
            ApplyCustomization();
            DoorViewInit();
            UpdateMaterial();
            SetHeadlightsState();
            ApplyDecalMaterials();
            
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            InitializeServices();
            ApplyCustomization();
            UpdateMaterial();
            SetHeadlightsState();
            UnityEditor.EditorApplication.delayCall += OnValidateCallback;
        }

        private void OnValidateCallback()
        {
            if (this == null)
            {
                UnityEditor.EditorApplication.delayCall -= OnValidateCallback;
                return; 
            }
            ApplyDecalMaterials();
            UpdateVisibility();
        }
#endif
    
        private void InitializeServices()
        {
            try
            {
                if (trunkDoor != null && trunkDoorTop == null)
                {
                    _doorService = new DoorService(rightDoor.gameObject, leftDoor.gameObject, trunkDoor.gameObject,null,
                        hood.gameObject, holder);
                }
                else if (trunkDoorTop != null && trunkDoor != null)
                    
                {
                    _doorService = new DoorService(rightDoor.gameObject, leftDoor.gameObject, trunkDoor.gameObject,trunkDoorTop.gameObject,
                        hood.gameObject, holder);
                }
                else
                {
                    _doorService = new DoorService(rightDoor.gameObject, leftDoor.gameObject, null, null,
                        hood.gameObject, holder);
                }
                WearMaterial originalWearMaterial = new WearMaterial(bodyMaterial, _currentWheelMaterial,
                    _currentInteriorMaterial,
                    _currentRailingsMaterial, _currentEngineMaterial ,  _currentFrontLightMaterial, _currentRearLightMaterial,null);
                _wearService = new WearService(originalWearMaterial, invisibleMaterial);
                _windowService = new WindowService(null, normalWindowMaterial, brokenWindowMaterial, _currentWindowMaterial);
                DoorViewInit();
            }
            catch (Exception )
            {
                // ignored
            }
        }

        private void SetHeadlightsState()
        {
            var headLightsService = GetComponent<HeadLightsService>();
            if (headLightsService != null)
            {
                headLightsService.SetHeadlightsState(turnOnLight);
                headLightsService.SetEmissiveLimits(headlightEmissiveMax, brakeEmissiveMax, reverseEmissiveMax);
            }
                
        }

        private void ApplyCustomization()
        {
            if (!changeSettings) return;

            if (synchronizeWearLevel)
            {
                wearLevelDust = wearLevel;
                wearLevelRust = wearLevel;
                levelPeelPaint = wearLevel;
                wearLevelGlass = wearLevel;
            }
            _wearService.SetWearLevel( wearLevelRust, wearLevelDust, levelPeelPaint, levelMaskMoss);

            SetChanges();
            

        }
        private void DoorViewInit()
        {
            leftDoor?.Init(_doorService, carController);
            rightDoor?.Init(_doorService, carController);
            rearLeftDoor?.Init(_doorService, carController);
            rearRightDoor?.Init(_doorService, carController);
            rearRightTrunkDoor?.Init(_doorService, carController);
            rearLeftTrunkDoor?.Init(_doorService, carController);
            trunkDoor?.Init(_doorService, carController);
            slidingDoorObject?.Init(_doorService, carController);
            hood?.Init(_doorService, carController);
            trunkDoorTop?.Init(_doorService, carController);
        }
        private void SetChanges()
        {
            try
            {
                if (isBurnt)
                    SetBurntMaterial();
                else
                    SetDefaultMaterial();
                
                if (frontLightBar != null)
                { 
                    var materialController
                        = frontLightBar.GetComponent<SirenMaterialController>();
                    if (materialController == null) return;

                    if (isBurnt)
                        materialController.SetBurnt();
                    else
                        materialController.SetDefault();
                    
                }
                _wearService.ReplaceMaterial(carBody, _currentEngineMaterial, CarPart.Engine);
                _wearService.ReplaceMaterial(carBody, _currentInteriorMaterial, CarPart.Interior);
                _wearService.ReplaceMaterial(carBody, _currentWheelMaterial, CarPart.Wheel );
                _wearService.ReplaceMaterial(carBody, glassMaterial, CarPart.Glass);
                _wearService.ReplaceMaterial(carBody, _currentInteriorMaterial, CarPart.Interior);
                _wearService.ReplaceMaterial(carBody, _currentFrontLightMaterial, CarPart.Light);
                _wearService.ReplaceMaterial(carBody, _currentFrontLightMaterial, CarPart.Light);
                _wearService.ReplaceMaterial(carBody, _currentRearLightMaterial, CarPart.Light);
                
                if (railingsMaterial != null)
                    _wearService.ReplaceMaterial(carBody, _currentRailingsMaterial, CarPart.Railing);
                
                _windowService._isBroken = windowIsBroken;
                if (windowIsBroken)
                {
                    _currentWindowMaterial = brokenWindowMaterial;
                    _windowService.SetOpacity(carBody, brokenWindowMaterial, 0.36f, levelMaskMoss, isBurnt);
                }
                else
                {
                    _currentWindowMaterial = normalWindowMaterial;
                    _windowService.SetOpacity(carBody, normalWindowMaterial, wearLevelGlass * 3, levelMaskMoss,
                        isBurnt);
                }
            }
            catch (Exception )
            {
                // ignored
            }
           
        }
        private void UpdateVisibility()
        {
            if (mossOnCar == null) return;
            foreach (var moss in mossOnCar)
            {
                if (moss != null)
                {
                    moss.SetActive(showMossOnCar);
                }
            }
        }

        private void SetDefaultMaterial()
        {
            _currentEngineMaterial = engineMaterial;
            _currentWheelMaterial = wheelMaterial;
            if (frontLightMaterial!=null)
                _currentFrontLightMaterial = frontLightMaterial;
            if (railingsMaterial != null)
                _currentRailingsMaterial = railingsMaterial;
            if (isUniqueInterior && interiorMaterials.Length > setOfMaterialIndex)
            {
                _currentInteriorMaterial = interiorMaterials[setOfMaterialIndex];
            }
            else
            {
                _currentInteriorMaterial = interiorMaterial;
            }

            if (rearLightMaterial != null)
            {
                _currentRearLightMaterial = rearLightMaterial;
            }
            _currentWindowMaterial = windowIsBroken ? brokenWindowMaterial : normalWindowMaterial;
            
        }
        private void SetBurntMaterial()
        {
            _currentWheelMaterial = burntWheelMaterial;
            _currentEngineMaterial = burntEngineMaterial;
            _currentInteriorMaterial = burntInteriorMaterial;
            _currentRailingsMaterial = burntRailingsMaterial;
            _currentFrontLightMaterial = burntFrontLightMaterial;
            _currentRearLightMaterial = burntRearLightMaterial;
        }

        private void UpdateMaterial()
        {
            if (isBurnt && changeSettings)
            {
                _wearService.ReplaceMaterial(carBody, burntBodyMaterial, CarPart.Body);
                bodyMaterial = burntBodyMaterial;
                
            
            }
            else if (setOfMaterials is { Length: > 0 } && changeSettings)
            {
                setOfMaterialIndex = Mathf.Clamp(setOfMaterialIndex, 0, setOfMaterials.Length - 1);
                Material selectedMaterial = setOfMaterials[setOfMaterialIndex];

                if (selectedMaterial == null) return;

                _wearService.ReplaceMaterial(carBody, selectedMaterial, CarPart.Body);
                bodyMaterial = selectedMaterial;
            }
            else
            {
                setOfMaterialIndex = 0;
            }
        }
        
        private void ApplyDecalMaterials()
        {
            if (!has3DBodyKit || carBody == null || decalMaterials == null || decalMaterials.Length == 0 || !changeSettings)
                return;

            if (frontLightBar != null)
            {
                frontLightBar.gameObject.SetActive(showLightBar);
                ApplyWearForBodyKit(frontLightBar.gameObject);
                if (turnOnSiren)
                    frontLightBar.TurnOn(); 
                else
                    frontLightBar.TurnOff(); 
            }

            if (frontProtection != null)
            {
                frontProtection.SetActive(showFrontProtection);
                ApplyWearForBodyKit(frontProtection);
                
            }
            int decalIndex = Mathf.Clamp(selectedDecalIndex, 0, decalMaterials.Length - 1);
            DecalSet selectedSet = decalMaterials[decalIndex];

            if (selectedSet.decalTargetMaterial == null)
                return;
            var renderers = carBody.GetComponentsInChildren<Renderer>(true);
            Material decalMaterial = selectedSet.decalTargetMaterial;

            foreach (var renderer in renderers)
            {
                var mats = renderer.sharedMaterials;
                bool changed = false;

                for (int i = 0; i < mats.Length; i++)
                {
                    if (mats[i] != null && mats[i].name.StartsWith("MI_Decal_", StringComparison.OrdinalIgnoreCase))
                    {
                        Material instancedMaterial = Instantiate(decalMaterial);
                        instancedMaterial.name = decalMaterial.name;

                        instancedMaterial.SetFloat("_Mask_Moss", levelMaskMoss);
                        instancedMaterial.SetFloat("_Mask_Rust_Global", wearLevelRust);
                        instancedMaterial.SetFloat("_Mask_Dust_Global", wearLevelDust);
                        instancedMaterial.SetFloat("_Normal_PeelPaint", levelPeelPaint);
                        if(isBurnt)
                            instancedMaterial.SetFloat("_Opacity", 0);

                        mats[i] = instancedMaterial;
                        changed = true;
                    }
                }

                if (changed)
                    renderer.sharedMaterials = mats;
            }
            if (!isBurnt && has3DBodyKit && decalMaterials != null && decalMaterials.Length > 1 )
            {
                if (selectedSet != null && selectedSet.materialVariants != null && selectedSet.materialVariants.Length > 0)
                {
                    setOfMaterials = selectedSet.materialVariants;
                }
            }
            UpdateMaterial();
        }
        private void ApplyWearForBodyKit(GameObject gameObject)
        {
            if (gameObject == null) return;

            var renderers = gameObject.GetComponentsInChildren<Renderer>(true);

            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials;
                bool changed = false;

                for (int i = 0; i < materials.Length; i++)
                {
                    var original = materials[i];
                    if (original == null) continue;

                    if (Application.isEditor && !Application.isPlaying && original.name.EndsWith("(Instance)"))
                        continue;

                    var instancedMaterial = new Material(original);
                    instancedMaterial.SetFloat("_Mask_Moss", levelMaskMoss);
                    instancedMaterial.SetFloat("_Mask_Rust_Global", wearLevelRust);
                    instancedMaterial.SetFloat("_Mask_Dust_Global", wearLevelDust);
                    instancedMaterial.SetFloat("_Normal_PeelPaint", levelPeelPaint);
                    materials[i] = instancedMaterial;
                    changed = true;
                }

                if (changed)
                    renderer.sharedMaterials = materials;
            }
        }

    }
}