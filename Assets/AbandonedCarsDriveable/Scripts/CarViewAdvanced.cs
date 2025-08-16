using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AbandonedCarsDriveable.Scripts
{
    public class CarViewAdvanced : MonoBehaviour
    {
        // Models
        [SerializeField] private GameObject carBody;

        [SerializeField] private Door leftDoor;
        [SerializeField] private Door rightDoor;
        [SerializeField] private Door rearLeftTrunkDoor;
        [SerializeField] private Door rearRightTrunkDoor;
        [SerializeField] private Door rearRightDoor;
        [SerializeField] private Door rearLeftDoor;
        [SerializeField] private Door hood;
        [SerializeField] public Door trunkDoor;
        [SerializeField] public Door slidingDoorObject;
    
    
        [SerializeField] private HolderConfig holder;
        [SerializeField] private GameObject frontLeftWheel;
        [SerializeField] private GameObject frontRightWheel;
        [SerializeField] private GameObject rearLeftWheel;
        [SerializeField] private GameObject rearRightWheel;
        [SerializeField] private GameObject[] mossOnCar; 
        [SerializeField] private GameObject grassUnderWheels;
        [SerializeField] public GameObject[] windows;
        [SerializeField] private GameObject glassBody;
        [SerializeField] private SirenLightController frontLightBar;     
        [SerializeField] private GameObject frontProtection;

    
        [SerializeField] private new GameObject light;
        [SerializeField] public bool hasRearDoors;
        [SerializeField] public bool hasSlidingDoor;
        [SerializeField] public bool hasRearTrunkDoors;
    

        //Meshes
        [SerializeField] private Mesh[] originalWheelLODs;
        [SerializeField] private Mesh[] deflatedWheelLODs;
    
        //Materials
        [SerializeField] private Material bodyMaterial;
        [SerializeField] private Material brokenWindowMaterial;
        [SerializeField] private Material normalWindowMaterial;
        [SerializeField] private Material wheelMaterial;
        [SerializeField] private Material invisibleMaterial;
        [SerializeField] private Material originalBodyMaterial;
        [SerializeField] private Material interiorMaterial;
        [SerializeField] private Material burntBodyMaterial;
        [SerializeField] private Material engineMaterial;
        [SerializeField] private Material railingsMaterial;
        [SerializeField] private Material burntRailingsMaterial;
        [SerializeField] private Material[] frontLightMaterial;
        [SerializeField] private Material rearLightMaterial; 
        [SerializeField] public Material[] setOfMaterials;
        [SerializeField] private Material burntWheelMaterial; 
        [SerializeField] private Material burntEngineMaterial; 
        [SerializeField] private Material burntInteriorMaterial; 
        [SerializeField] private Material[] burntFrontLightMaterial;
        [SerializeField] private Material burntRearLightMaterial;
        [SerializeField] private Material[] glassMaterial;
        [SerializeField] private Material[] interiorMaterials;
        [SerializeField] public DecalSet[] decalMaterials; 

        [SerializeField] private bool changeSettings = false; 
        [SerializeField] public bool isUniqueInterior;
        //Exterior Settings
    
        [SerializeField]public int selectedDecalIndex = 0;
        [SerializeField]public int setOfMaterialIndex = 0;
        [SerializeField] private bool synchronizeWearLevel;
        [SerializeField, Range(0, 1)] private float wearLevel; 
        [SerializeField, Range(0, 1)] private float wearLevelDust;
        [SerializeField, Range(0, 1)] private float wearLevelRust;
        [SerializeField, Range(0, 1)] private float levelPeelPaint;
        [SerializeField, Range(0, 1)] private float levelMaskMoss;
        [SerializeField, Range(0, 1)] private float wearLevelGlass;
        [SerializeField] public bool isBurnt;
        [SerializeField] public bool showMossOnCar;
        [SerializeField] public bool showGrassUnderWheels;
    

        //Door Settings
        [SerializeField, Range(90, 180)] private float leftDoorOpenAngle  = 90;
        [SerializeField, Range(90, 0)] private float rightDoorOpenAngle = 90;
        [SerializeField, Range(90, 180)] private float leftRearDoorOpenAngle;
        [SerializeField, Range(90, 0)] private float rightRearDoorOpenAngle;
        [SerializeField, Range(-5, 0)] private float leftDoorTilt;
        [SerializeField, Range(-5, 0)] private float rightDoorTilt;
        [SerializeField, Range(-5, 0)] private float leftRearDoorTilt;
        [SerializeField, Range(-5, 0)] private float rightRearDoorTilt;
        [SerializeField, Range(0, 1)] private float slidingDoorOpen;
        [SerializeField] private bool openTrunkReverse;
        [SerializeField, Range(0, -35)] private float hoodOpenAngle;
        [SerializeField, Range(90, 180)] private float rearLeftTrunkOpenAngle = 90;
        [SerializeField, Range(90, 0)] private float rearRightTrunkOpenAngle = 90;
        [SerializeField, Range(0, 115)] private float trunkOpenAngle;
        [SerializeField, Range(80, 100)] private float trunkOpenTilt = 90;
        [SerializeField, Range(80, 100)] private float hoodOpenTilt = 90;

        [SerializeField, Range(0, 1)] private float holderOpen;

        [SerializeField] private bool removeLeftDoor;
        [SerializeField] private bool removeRightDoor;
        [SerializeField] private bool removeTrunk;
        [SerializeField] private bool removeHood;
        [SerializeField] public bool hasLiftRearDoor;
        [SerializeField] public bool has3DBodyKit;
        [SerializeField] public bool showLightBar;
        [SerializeField] public bool showFrontProtection;
        [SerializeField] public bool turnOnSiren;
    
        //Light Settings 
        [SerializeField, Range(0, 5)] private float frontLightIntensity;
        [SerializeField, Range(0, 5)] private float rearLightIntensity;
        [SerializeField] private bool turnOnLight;
    
        //Window Settings
        [SerializeField] private bool windowIsBroken;

        [Header("Wheel States (0 = Normal, 1 = Deflated, 2 = Hidden)")] 
        [SerializeField, Range(0, 2)] private int frontLeftWheelStateIndex;
        [SerializeField, Range(0, 2)] private int frontRightWheelStateIndex;
        [SerializeField, Range(0, 2)] private int rearLeftWheelStateIndex;
        [SerializeField, Range(0, 2)] private int rearRightWheelStateIndex;

        [SerializeField, Range(45, 135)] private float rotationFrontWheel = 90;


        //Randomizer
        [SerializeField] private bool randomizeExterior;

        [Header("0 - OpenDoor 90 - CloseDoor")]
        [SerializeField, Range(90, 0)] public float doorOpenAngleMax;  
        [SerializeField, Range(0, 90)]public float trunkOpenAngleMax;
        [SerializeField, Range(0, -35)]public float hoodOpenAngleMax;

        public bool randomizeMaterialIndex;
        public bool randomizeWearLevel;
        public bool randomizeDoors;
        public bool randomizeSlidingDoor;
        public bool randomizeRearTrunkDoors;
        public bool randomizeTrunk;
        public bool randomizeHood;
        public bool randomizeLightIntensity;
        [Header("if randomizeEachWindow = true then windowIsBroken doesn't work")]
        public bool randomizeEachWindow;
        public bool randomizeWindows;
        public bool randomizeWheelDeflation;
        public bool randomizeMoss;
        public bool randomizeOnLocation;
        public bool randomizeRemoveDoors; 
        public bool randomizeRemoveHood; 
        public bool randomizeRemoveTrunk;
    
        private Material _currentRailingsMaterial;
        private Material _currentEngineMaterial;
        private Material _currentInteriorMaterial;
        private Material _currentWheelMaterial;
        private Material[] _currentFrontLightMaterial; 
        private Material _currentRearLightMaterial; 
    
        private Quaternion _startHolderRotation; 
        private Quaternion _endHolderRotation;

        private WearService _wearService;
        private DoorService _doorService;
        private WindowService _windowService;
        private WheelService _wheelService;
        private LightService _lightService;
        private RandomizerService _randomizerService;
        private SavedValues _savedValues;
        private PositionCalculator _positionCalculator;
        private float _windowOpacity;
        private bool _valuesSaved;
        private bool[] _windowStates;
        


        private void Start()
        {
           
            SetDefaultMaterial();
            InitializeServices();
            DoorViewInit();
            UpdateDoorValue();
            
            if (frontLightBar != null)
            {
                if (turnOnSiren)
                {
                    frontLightBar.TurnOn();
                }
                else
                {
                    frontLightBar.TurnOff();
                }

                if (frontLightBar.HasToggle)
                {
                    frontLightBar.ToggleSiren();
                }
            }
        }

        private void UpdateDoorValue()
        {
            leftDoor.UpdateDoorData(leftDoorOpenAngle, leftDoorTilt);
            rightDoor.UpdateDoorData(rightDoorOpenAngle, rightDoorTilt);
            hood.UpdateDoorData(hoodOpenAngle, hoodOpenTilt);
            if (rearLeftTrunkDoor != null && hasRearTrunkDoors && !hasLiftRearDoor)
                rearLeftTrunkDoor.UpdateDoorData(rearLeftTrunkOpenAngle, 0);
            else if(rearLeftTrunkDoor != null && hasRearTrunkDoors && hasLiftRearDoor)
            {
                rearLeftTrunkDoor.UpdateDoorData(rearLeftTrunkOpenAngle, 90);
            }
            if (rearRightTrunkDoor != null && hasRearTrunkDoors && !hasLiftRearDoor)
                rearRightTrunkDoor.UpdateDoorData(rearRightTrunkOpenAngle, 0);
            else if (rearRightTrunkDoor != null && hasRearTrunkDoors && hasLiftRearDoor)
            {
                rearRightTrunkDoor.UpdateDoorData(rearRightTrunkOpenAngle, 90);
            }
            if (rearRightDoor != null)
                rearRightDoor.UpdateDoorData(rightDoorOpenAngle, rightRearDoorTilt);
            if (rearLeftDoor != null)
                rearLeftDoor.UpdateDoorData(leftDoorOpenAngle, leftRearDoorTilt);
            if (trunkDoor != null && openTrunkReverse && !hasRearTrunkDoors)
                trunkDoor.UpdateDoorData(-trunkOpenAngle, trunkOpenTilt);
            else if (trunkDoor != null && !openTrunkReverse && !hasRearTrunkDoors)
                trunkDoor.UpdateDoorData(trunkOpenAngle, trunkOpenTilt);
        }
        private void InitializeServices()
        {
            try
            {
                if (!hasRearTrunkDoors)
                {
                    _doorService = new DoorService(rightDoor.gameObject, leftDoor.gameObject, trunkDoor.gameObject,
                        null, hood.gameObject, holder.gameObject);
                }
                else if(hasLiftRearDoor)
                {
                    _doorService = new DoorService(rightDoor.gameObject, leftDoor.gameObject, rearLeftTrunkDoor.gameObject, rearRightTrunkDoor.gameObject,
                        hood.gameObject, holder.gameObject);
                }
                else 
                {
                    _doorService = new DoorService(rightDoor.gameObject, leftDoor.gameObject, null, null,
                        hood.gameObject, holder.gameObject);
                }
                WearMaterial originalWearMaterial = new WearMaterial(bodyMaterial, _currentWheelMaterial,
                    _currentInteriorMaterial, _currentRailingsMaterial, _currentEngineMaterial,
                    _currentFrontLightMaterial, _currentRearLightMaterial, glassMaterial);
                _wearService = new WearService(originalWearMaterial, invisibleMaterial);
                _windowService = new WindowService(windows, normalWindowMaterial, brokenWindowMaterial, null);
                _wheelService = new WheelService(frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel,
                    deflatedWheelLODs, originalWheelLODs, _wearService);
                _lightService = new LightService(carBody, light, _currentRearLightMaterial,null, _currentFrontLightMaterial);
                _randomizerService = new RandomizerService(_wearService, _doorService, _wheelService,
                    _lightService, _windowService);
                _positionCalculator = new PositionCalculator(carBody);
            }
            catch (Exception )
            {
                // ignored
            }
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            InitializeServices();
            ApplyCustomization();
            UpdateMaterial();
            UpdateDoorValue();
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
            SetWheelState();
        }
#endif

        private void ApplyCustomization()
        {
            SetDefaultMaterial();

            if (randomizeExterior)
            {
                var randomValues = _randomizerService.GetRandomValues(this);
                RandomizeWheelState();
                SaveRandomValues(randomValues);
                randomizeExterior = false;
                synchronizeWearLevel = false;
                changeSettings = true;
                ApplyCustomization();
            }
            else if(changeSettings)
            {
                float trunkAngle = openTrunkReverse ? -trunkOpenAngle : trunkOpenAngle;
           
                if (synchronizeWearLevel && !isBurnt)
                {
                    wearLevelDust = wearLevel;
                    wearLevelRust = wearLevel;
                    levelPeelPaint = wearLevel;
                    wearLevelGlass = wearLevel;
                }

                if (!isBurnt)
                {
                    _wearService.SetWearLevel( wearLevelRust, wearLevelDust, levelPeelPaint, levelMaskMoss);
                    _windowService.SetMoss(levelMaskMoss); 
                }
                else
                {
                    _wearService.SetWearLevel( wearLevelRust, wearLevelDust, levelPeelPaint, 0);
                    _windowService.SetMoss(0);
                }
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

                if (rearLeftTrunkDoor != null && rearRightTrunkDoor != null)
                {
                    rearLeftTrunkDoor.gameObject.SetActive(!removeTrunk);
                    rearRightTrunkDoor.gameObject.SetActive(!removeTrunk);
                }
               
            
                _windowService.SetUnbrokenOpacity(wearLevelGlass * 3);



                if (leftDoorOpenAngle > leftDoor.openAngleForTilt)
                    _doorService.OpenDoor(DoorService.DoorSide.Left, leftDoorOpenAngle, leftDoorTilt);
                else
                    _doorService.OpenDoor(DoorService.DoorSide.Left, leftDoorOpenAngle, 0);
            
                if (rightDoorOpenAngle < rightDoor.openAngleForTilt)
                    _doorService.OpenDoor(DoorService.DoorSide.Right, rightDoorOpenAngle, rightDoorTilt);
                else
                    _doorService.OpenDoor(DoorService.DoorSide.Right, rightDoorOpenAngle, 0);
            
                if (!hasRearTrunkDoors &&  trunkDoor != null && trunkOpenAngle > trunkDoor.openAngleForTilt )
                    _doorService.OpenExtraDoor(DoorService.ExtraDoor.Trunk, trunkAngle, trunkOpenTilt);
                else if (!hasRearTrunkDoors)
                {
                    _doorService.OpenExtraDoor(DoorService.ExtraDoor.Trunk, trunkAngle, 90);
                }
                
                if (hoodOpenAngle < hood.openAngleForTilt)
                    _doorService.OpenExtraDoor(DoorService.ExtraDoor.Hood, hoodOpenAngle, hoodOpenTilt);
                else
                    _doorService.OpenExtraDoor(DoorService.ExtraDoor.Hood, hoodOpenAngle, 90);

                _doorService.RemoveDoor(DoorService.DoorSide.Left, removeLeftDoor);
                _doorService.RemoveDoor(DoorService.DoorSide.Right, removeRightDoor);
            
                if (hasSlidingDoor && slidingDoorObject != null)
                {
                    _doorService.SlideDoor(slidingDoorObject.gameObject, slidingDoorOpen);
                }
                if (hasRearTrunkDoors && rearLeftTrunkDoor != null && rearRightTrunkDoor != null && !hasLiftRearDoor)
                {
                    _doorService.OpenRearDoors(rearLeftTrunkDoor.gameObject, rearRightTrunkDoor.gameObject,  rearLeftTrunkOpenAngle, rearRightTrunkOpenAngle, 0, 0, hasLiftRearDoor);
                }
                if (hasRearTrunkDoors && rearLeftTrunkDoor != null && rearRightTrunkDoor != null && hasLiftRearDoor)
                {
                    _doorService.OpenRearDoors(rearLeftTrunkDoor.gameObject, rearRightTrunkDoor.gameObject,  rearLeftTrunkOpenAngle, rearRightTrunkOpenAngle, 90, 90, hasLiftRearDoor);
                }
            
                if (hasRearDoors && rearLeftDoor != null && rearRightDoor != null)
                {
                    if (leftRearDoorOpenAngle > rearLeftDoor.openAngleForTilt)
                        _doorService.OpenDoor(rearLeftDoor.gameObject, leftRearDoorOpenAngle, leftRearDoorTilt);
                    else
                        _doorService.OpenDoor(rearLeftDoor.gameObject, leftRearDoorOpenAngle, 0);
                
                    if (rightRearDoorOpenAngle > rearRightDoor.openAngleForTilt)
                        _doorService.OpenDoor(rearRightDoor.gameObject, rightRearDoorOpenAngle, rightRearDoorTilt);
                    else
                        _doorService.OpenDoor(rearRightDoor.gameObject, rightRearDoorOpenAngle, 0);
                
                
                }
            
                if (holder != null)
                {
                    _doorService.OpenHolderCar(holderOpen, holder.StartRotationEuler, holder.EndRotationEuler);
                }

                _doorService.RemoveExtraDoor(DoorService.ExtraDoor.Trunk, removeTrunk);
                _doorService.RemoveExtraDoor(DoorService.ExtraDoor.Hood, removeHood);
            
                SetChanges(isBurnt);
                UpdateCarPosition();
            

                _lightService?.TurnOnLight(turnOnLight);

                if (!randomizeEachWindow)
                {
                    for (int i = 0; i < windows.Length; i++)
                        _windowService.BreakWindow(i, windowIsBroken);
            
                    if (isBurnt && windowIsBroken)
                        _windowService.SetBurntSettingForBrokenWindow();
                    else if(isBurnt && !windowIsBroken)
                        _windowService.SetBurntSettingForUnbrokenWindow();
                
                }
                else if (_windowStates != null && _windowStates.Length == windows.Length)
                {
                    for (int i = 0; i < windows.Length; i++)
                    {
                        _windowService.BreakWindow(i, _windowStates[i]);
                    }
                }

                if (!isBurnt) return;
                if (_windowStates != null && Array.Exists(_windowStates, state => state))
                {
                    _windowService.SetBurntSettingForBrokenWindow();
                }
                else
                {
                    _windowService.SetBurntSettingForUnbrokenWindow();
                }
                

            }
        }

        private void SetWheelState()
        {
            if (_wheelService != null && changeSettings)
            {
                _wheelService.RotateAllWheels(rotationFrontWheel);
                _wheelService.SetWheelState(frontLeftWheel, (WheelState)frontLeftWheelStateIndex);
                _wheelService.SetWheelState(frontRightWheel, (WheelState)frontRightWheelStateIndex);
                _wheelService.SetWheelState(rearLeftWheel, (WheelState)rearLeftWheelStateIndex);
                _wheelService.SetWheelState(rearRightWheel, (WheelState)rearRightWheelStateIndex);
            }
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
        private void DoorViewInit()
        {
            leftDoor?.Init(_doorService, null);
            rightDoor?.Init(_doorService, null);
            rearLeftDoor?.Init(_doorService, null);
            rearRightDoor?.Init(_doorService, null);
            rearRightTrunkDoor?.Init(_doorService, null);
            rearLeftTrunkDoor?.Init(_doorService, null);
            if (!hasRearTrunkDoors) 
                trunkDoor?.Init(_doorService, null);
            slidingDoorObject?.Init(_doorService, null);
            hood?.Init(_doorService, null);
        }
        private void SetChanges(bool isBurnt)
        {
            try
            {
                if (isBurnt)
                    SetBurntMaterial();
                else
                    SetDefaultMaterial();
                _wearService.ReplaceMaterial(carBody, _currentEngineMaterial, CarPart.Engine);
                _wearService.ReplaceMaterial(frontLeftWheel, _currentWheelMaterial, CarPart.Wheel,
                    (WheelState)frontLeftWheelStateIndex);
                _wearService.ReplaceMaterial(frontRightWheel, _currentWheelMaterial, CarPart.Wheel,
                    (WheelState)frontRightWheelStateIndex);
                _wearService.ReplaceMaterial(rearLeftWheel, _currentWheelMaterial, CarPart.Wheel,
                    (WheelState)rearLeftWheelStateIndex);
                _wearService.ReplaceMaterial(rearRightWheel, _currentWheelMaterial, CarPart.Wheel,
                    (WheelState)rearRightWheelStateIndex);
                _wearService.ReplaceMaterial(carBody, _currentInteriorMaterial, CarPart.Interior);
                _wearService.ReplaceMaterial(carBody, _currentFrontLightMaterial, CarPart.Light);
                _wearService.ReplaceMaterial(carBody, _currentRearLightMaterial, CarPart.Light);
                if (railingsMaterial != null)
                    _wearService.ReplaceMaterial(carBody, _currentRailingsMaterial, CarPart.Railing);
                _wearService.ReplaceMaterial(carBody, glassMaterial, CarPart.GlassBody);

                _lightService.SetEmissivePower(frontLightIntensity, rearLightIntensity);

                glassBody.gameObject.SetActive(!isBurnt);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        private void SetDefaultMaterial()
        {
            _currentEngineMaterial = engineMaterial;
            _currentWheelMaterial = wheelMaterial;
            _currentFrontLightMaterial = frontLightMaterial;
            _currentRearLightMaterial = rearLightMaterial;

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
            if (!isBurnt && has3DBodyKit && decalMaterials != null && decalMaterials.Length > 1 )
            {
                int decalIndex = Mathf.Clamp(selectedDecalIndex, 0, decalMaterials.Length - 1);
                DecalSet selectedSet = decalMaterials[decalIndex];

                if (selectedSet != null && selectedSet.materialVariants != null && selectedSet.materialVariants.Length > 0)
                {
                    setOfMaterials = selectedSet.materialVariants;
                }
            }

        }

        private void UpdateMaterial()
        {
            if (isBurnt)
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
        private void UpdateCarPosition()
        {
            try
            {
                _positionCalculator.UpdateCarPosition(frontLeftWheelStateIndex, rearLeftWheelStateIndex,
                    frontRightWheelStateIndex, rearRightWheelStateIndex);
            }
            catch (Exception)
            {
                // ignored
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


    
        private void SaveRandomValues(RandomValues randomValues)
        {
            if (randomValues.SetOfMaterialIndex.HasValue) setOfMaterialIndex = randomValues.SetOfMaterialIndex.Value;
            if (randomValues.WearLevelRust.HasValue) wearLevelRust = randomValues.WearLevelRust.Value;
            if (randomValues.WearLevelDust.HasValue) wearLevelDust = randomValues.WearLevelDust.Value;

            if (randomValues.LeftDoorOpenAngle.HasValue) leftDoorOpenAngle = randomValues.LeftDoorOpenAngle.Value;
            if (randomValues.RightDoorOpenAngle.HasValue) rightDoorOpenAngle = randomValues.RightDoorOpenAngle.Value;
            if (randomValues.RightRearDoorOpenAngle.HasValue) rightRearDoorOpenAngle = randomValues.RightRearDoorOpenAngle.Value;
            if (randomValues.LeftRearDoorOpenAngle.HasValue) leftRearDoorOpenAngle = randomValues.LeftRearDoorOpenAngle.Value;
        
            if (randomValues.RightRearDoorTilt.HasValue) rightRearDoorTilt = randomValues.RightRearDoorTilt.Value;
            if (randomValues.LeftRearDoorTilt.HasValue) leftRearDoorTilt = randomValues.LeftRearDoorTilt.Value;
            if (randomValues.LeftDoorTilt.HasValue) leftDoorTilt = randomValues.LeftDoorTilt.Value;
            if (randomValues.RightDoorTilt.HasValue) rightDoorTilt = randomValues.RightDoorTilt.Value;

            if (randomValues.SlidingDoorOpen.HasValue) slidingDoorOpen = randomValues.SlidingDoorOpen.Value;

            if (randomValues.RearRightTrunkOpenAngle.HasValue) rearRightTrunkOpenAngle = randomValues.RearRightTrunkOpenAngle.Value;
            if (randomValues.RearLeftTrunkOpenAngle.HasValue) rearLeftTrunkOpenAngle = randomValues.RearLeftTrunkOpenAngle.Value;

            if (randomValues.TrunkOpenAngle.HasValue) trunkOpenAngle = randomValues.TrunkOpenAngle.Value;
            if (randomValues.TrunkOpenTilt.HasValue) trunkOpenTilt = randomValues.TrunkOpenTilt.Value;

            if (randomValues.HoodOpenAngle.HasValue) hoodOpenAngle = randomValues.HoodOpenAngle.Value;
            if (randomValues.HoodOpenTilt.HasValue) hoodOpenTilt = randomValues.HoodOpenTilt.Value;

            if (randomValues.FrontLightIntensity.HasValue) frontLightIntensity = randomValues.FrontLightIntensity.Value;
            if (randomValues.RemoveLeftDoor.HasValue) removeLeftDoor = randomValues.RemoveLeftDoor.Value;
            if (randomValues.RemoveRightDoor.HasValue) removeRightDoor = randomValues.RemoveRightDoor.Value;
            if (randomValues.RemoveHood.HasValue) removeHood = randomValues.RemoveHood.Value;
            if (randomValues.LevelMaskMoss.HasValue) levelMaskMoss = randomValues.LevelMaskMoss.Value;
            if (randomValues.RemoveTrunk.HasValue) removeTrunk = randomValues.RemoveTrunk.Value;

            if (randomValues.WindowIsBroken == null || randomValues.WindowIsBroken.Count <= 0) return;
            if (windows.Length != randomValues.WindowIsBroken.Count)
            {
                Debug.LogWarning("Mismatch between windows and WindowIsBroken count!");
                return;
            }

            if (_windowStates == null || _windowStates.Length != randomValues.WindowIsBroken.Count)
            {
                _windowStates = new bool[randomValues.WindowIsBroken.Count];
            }

            for (int i = 0; i < randomValues.WindowIsBroken.Count; i++)
            {
                _windowStates[i] = randomValues.WindowIsBroken[i] ?? false; 
            }
        }
        private void RandomizeWheelState()
        {
            if (!randomizeWheelDeflation) return;
            frontLeftWheelStateIndex = Random.Range(0, 3);
            frontRightWheelStateIndex = Random.Range(0, 3);
            rearLeftWheelStateIndex = Random.Range(0, 3);
            rearRightWheelStateIndex = Random.Range(0, 3);

            _wheelService.SetWheelState(frontLeftWheel, (WheelState)frontLeftWheelStateIndex);
            _wheelService.SetWheelState(frontRightWheel, (WheelState)frontRightWheelStateIndex);
            _wheelService.SetWheelState(rearLeftWheel, (WheelState)rearLeftWheelStateIndex);
            _wheelService.SetWheelState(rearRightWheel, (WheelState)rearRightWheelStateIndex);

            UpdateCarPosition();
        }
        private void UpdateVisibility()
        {
            if (mossOnCar != null)
            {
                foreach (var moss in mossOnCar)
                {
                    if (moss != null)
                    {
                        moss.SetActive(showMossOnCar);
                    }
                }
            }

            if (grassUnderWheels != null)
            {
                grassUnderWheels.SetActive(showGrassUnderWheels);
            }
        }
    
    
        [ContextMenu("Save Current Values")]
        public void SaveCurrentValues()
        {
            _savedValues = new SavedValues(this);
            _valuesSaved = true;
        }

        [ContextMenu("Apply Saved Values")]
        public void ApplySavedValues()
        {
            if (!_valuesSaved)
                return;

            _savedValues.ApplyTo(this);
            InitializeServices();
            ApplyCustomization();
            UpdateMaterial();
        }
        [Serializable]
        private class SavedValues
        {
            public int setOfMaterialIndex;
            public float wearLevelDust;
            public float wearLevelRust;
            public float leftDoorOpenAngle;
            public float rightDoorOpenAngle;
            public float leftDoorTilt;
            public float rightDoorTilt;
            public float trunkOpenAngle;
            public float hoodOpenAngle;
            public float trunkOpenTilt;
            public float hoodOpenTilt;
            public float holderOpen;
            public bool removeLeftDoor;
            public bool removeRightDoor;
            public bool removeTrunk;
            public bool removeHood;
            public float frontLightIntensity;
            public float rearLightIntensity;
            public bool windowIsBroken;
            public float windowOpacity;
            public int frontLeftWheelStateIndex;
            public int frontRightWheelStateIndex;
            public int rearLeftWheelStateIndex;
            public int rearRightWheelStateIndex;
            public float rotationFrontWheel;
            public float levelMaskMoss;
       

            public SavedValues(CarViewAdvanced car)
            {
                setOfMaterialIndex = car.setOfMaterialIndex;
                wearLevelDust = car.wearLevelDust;
                wearLevelRust = car.wearLevelRust;
                leftDoorOpenAngle = car.leftDoorOpenAngle;
                rightDoorOpenAngle = car.rightDoorOpenAngle;
                leftDoorTilt = car.leftDoorTilt;
                rightDoorTilt = car.rightDoorTilt;
                trunkOpenAngle = car.trunkOpenAngle;
                hoodOpenAngle = car.hoodOpenAngle;
                trunkOpenTilt = car.trunkOpenTilt;
                hoodOpenTilt = car.hoodOpenTilt;
                holderOpen = car.holderOpen;
                removeLeftDoor = car.removeLeftDoor;
                removeRightDoor = car.removeRightDoor;
                removeTrunk = car.removeTrunk;
                removeHood = car.removeHood;
                frontLightIntensity = car.frontLightIntensity;
                rearLightIntensity = car.rearLightIntensity;
                windowIsBroken = car.windowIsBroken;
                windowOpacity = car._windowOpacity;
                frontLeftWheelStateIndex = car.frontLeftWheelStateIndex;
                frontRightWheelStateIndex = car.frontRightWheelStateIndex;
                rearLeftWheelStateIndex = car.rearLeftWheelStateIndex;
                rearRightWheelStateIndex = car.rearRightWheelStateIndex;
                rotationFrontWheel = car.rotationFrontWheel;
                levelMaskMoss = car.levelMaskMoss;
            }

            public void ApplyTo(CarViewAdvanced car)
            {
                car.setOfMaterialIndex = setOfMaterialIndex ;
                car.wearLevelDust = wearLevelDust;
                car.wearLevelRust = wearLevelRust;
                car.leftDoorOpenAngle = leftDoorOpenAngle;
                car.rightDoorOpenAngle = rightDoorOpenAngle;
                car.leftDoorTilt = leftDoorTilt;
                car.rightDoorTilt = rightDoorTilt;
                car.trunkOpenAngle = trunkOpenAngle;
                car.hoodOpenAngle = hoodOpenAngle;
                car.trunkOpenTilt = trunkOpenTilt;
                car.hoodOpenTilt = hoodOpenTilt;
                car.holderOpen = holderOpen;
                car.removeLeftDoor = removeLeftDoor;
                car.removeRightDoor = removeRightDoor;
                car.removeTrunk = removeTrunk;
                car.removeHood = removeHood;
                car.frontLightIntensity = frontLightIntensity;
                car.rearLightIntensity = rearLightIntensity;
                car.windowIsBroken = windowIsBroken;
                car._windowOpacity = windowOpacity;
                car.frontLeftWheelStateIndex = frontLeftWheelStateIndex;
                car.frontRightWheelStateIndex = frontRightWheelStateIndex;
                car.rearLeftWheelStateIndex = rearLeftWheelStateIndex;
                car.rearRightWheelStateIndex = rearRightWheelStateIndex;
                car.rotationFrontWheel = rotationFrontWheel;
                car.levelMaskMoss = levelMaskMoss;
            }
        }

        public void ApplyRandomization()
        {
            InitializeServices();
            var randomValues = _randomizerService.GetRandomValues(this);
            SaveRandomValues(randomValues);
            RandomizeWheelState();
            ApplyCustomization();
            UpdateMaterial();
        }
    }
    [System.Serializable]
    public class DecalSet
    {
        public Material[] materialVariants; 
        public Material decalTargetMaterial; 
    }
}
