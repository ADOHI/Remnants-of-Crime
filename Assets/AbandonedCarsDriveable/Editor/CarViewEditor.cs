using AbandonedCarsDriveable.Scripts;
using UnityEditor;
using UnityEngine;

namespace AbandonedCarsDriveable.Editor
{
    [CustomEditor(typeof(CarViewAdvanced))]
    public class CarViewEditor : UnityEditor.Editor
    {
        private SerializedProperty carBody, leftDoor, rightDoor, hood, holder, frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel, windows, trunkDoor,rearRightDoor,rearLeftDoor, light, glassBody,grassUnderWheels,mossOnCar;
        private SerializedProperty hasSlidingDoor, slidingDoorObject, slidingDoorOpen, hasRearTrunkDoors, hasRearDoors, rearLeftTrunkDoor, rearRightTrunkDoor;
        private SerializedProperty originalWheelLODs, deflatedWheelLODs;
        private SerializedProperty bodyMaterial, brokenWindowMaterial, normalWindowMaterial, wheelMaterial, invisibleMaterial, originalBodyMaterial, frontLightMaterial, rearLightMaterial, setOfMaterials, interiorMaterial, engineMaterial, railingsMaterial, burntBodyMaterial, burntWheelMaterial;
        private SerializedProperty burntEngineMaterial,burntInteriorMaterial,burntFrontLightMaterial,burntRearLightMaterial,burntRailingsMaterial,glassMaterial;
        private SerializedProperty changeSettings, setOfMaterialIndex, synchronizeWearLevel, wearLevel,wearLevelGlass, wearLevelDust, wearLevelRust, levelPeelPaint, levelMaskMoss;
        private SerializedProperty leftDoorOpenAngle, rightDoorOpenAngle, leftDoorTilt, rightDoorTilt, trunkOpenAngle, openTrunkReverse, hoodOpenAngle, trunkOpenTilt, hoodOpenTilt, rearLeftTrunkOpenAngle, rearRightTrunkOpenAngle,rightRearDoorOpenAngle,leftRearDoorOpenAngle,rightRearDoorTilt,leftRearDoorTilt;
        private SerializedProperty holderOpen, removeLeftDoor, removeRightDoor, removeTrunk, removeHood;
        private SerializedProperty frontLightIntensity, rearLightIntensity;
        private SerializedProperty windowIsBroken, isBurnt, turnOnLight, showMossOnCar, showGrassUnderWheels;
        private SerializedProperty frontLeftWheelStateIndex, frontRightWheelStateIndex, rearLeftWheelStateIndex, rearRightWheelStateIndex, rotationFrontWheel;
        private SerializedProperty randomizeExterior, randomizeMaterialIndex, randomizeSlidingDoor, randomizeRearTrunkDoors, randomizeDoors, trunkOpenAngleMin, trunkOpenAngleMax, trunkOpenTiltMin,randomizeRemoveHood,randomizeRemoveDoors,randomizeRemoveTrunk, trunkOpenTiltMax, randomizeTrunk,randomizeMoss;
        private SerializedProperty randomizeHood, randomizeOnLocation, doorOpenAngleMax, hoodOpenAngleMax, randomizeFrontWheelRotation, randomizeLightIntensity, randomizeWindows, randomizeWearLevel, randomizeHolderPosition, randomizeEachWindow, randomizeWheelDeflation, interiorMaterials ,isUniqueInterior;
        private SerializedProperty hasLiftRearDoor, has3DBodyKit, frontLightBar, frontProtection, decalMaterials, selectedDecalIndex, showLightBar, showFrontProtection,turnOnSiren;

        private bool showModels = true;
        private bool showMeshes = true;
        private bool showMaterials = true;
        private bool showSettings = true;
        private bool showRandomization = true;
        private Vector3 _lastPosition; 
        private float _lastUpdateTime;
        private const float UpdateInterval = 0.5f; 

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            _lastPosition = ((CarViewAdvanced)target).transform.localPosition;
            doorOpenAngleMax = serializedObject.FindProperty("doorOpenAngleMax");
            trunkOpenAngleMax = serializedObject.FindProperty("trunkOpenAngleMax");
            carBody = serializedObject.FindProperty("carBody");
            light = serializedObject.FindProperty("light");
            slidingDoorOpen = serializedObject.FindProperty("slidingDoorOpen");
            leftDoor = serializedObject.FindProperty("leftDoor");
            glassBody = serializedObject.FindProperty("glassBody");
            rearRightDoor = serializedObject.FindProperty("rearRightDoor");
            rightRearDoorOpenAngle = serializedObject.FindProperty("rightRearDoorOpenAngle");
            leftRearDoorOpenAngle = serializedObject.FindProperty("leftRearDoorOpenAngle");
            leftRearDoorTilt = serializedObject.FindProperty("leftRearDoorTilt");
            rightRearDoorTilt = serializedObject.FindProperty("rightRearDoorTilt");
            rearLeftDoor = serializedObject.FindProperty("rearLeftDoor");
            rightDoor = serializedObject.FindProperty("rightDoor");
            hood = serializedObject.FindProperty("hood");
            holder = serializedObject.FindProperty("holder");
            frontLeftWheel = serializedObject.FindProperty("frontLeftWheel");
            frontRightWheel = serializedObject.FindProperty("frontRightWheel");
            rearLeftWheel = serializedObject.FindProperty("rearLeftWheel");
            rearRightWheel = serializedObject.FindProperty("rearRightWheel");
            windows = serializedObject.FindProperty("windows");
            trunkDoor = serializedObject.FindProperty("trunkDoor");
            hasSlidingDoor = serializedObject.FindProperty("hasSlidingDoor");
            slidingDoorObject = serializedObject.FindProperty("extraDoorObject");
            hasRearTrunkDoors = serializedObject.FindProperty("hasRearTrunkDoors");
            hasRearDoors = serializedObject.FindProperty("hasRearDoors");
            rearLeftTrunkDoor = serializedObject.FindProperty("rearLeftTrunkDoor");
            rearRightTrunkDoor = serializedObject.FindProperty("rearRightTrunkDoor");
            originalWheelLODs = serializedObject.FindProperty("originalWheelLODs");
            deflatedWheelLODs = serializedObject.FindProperty("deflatedWheelLODs");
            bodyMaterial = serializedObject.FindProperty("bodyMaterial");
            interiorMaterial = serializedObject.FindProperty("interiorMaterial");
            brokenWindowMaterial = serializedObject.FindProperty("brokenWindowMaterial");
            normalWindowMaterial = serializedObject.FindProperty("normalWindowMaterial");
            engineMaterial = serializedObject.FindProperty("engineMaterial");
            railingsMaterial = serializedObject.FindProperty("railingsMaterial");
            wheelMaterial = serializedObject.FindProperty("wheelMaterial");
            invisibleMaterial = serializedObject.FindProperty("invisibleMaterial");
            originalBodyMaterial = serializedObject.FindProperty("originalBodyMaterial");
            frontLightMaterial = serializedObject.FindProperty("frontLightMaterial");
            rearLightMaterial = serializedObject.FindProperty("rearLightMaterial");
            setOfMaterials = serializedObject.FindProperty("setOfMaterials");
            changeSettings = serializedObject.FindProperty("changeSettings");
            setOfMaterialIndex = serializedObject.FindProperty("setOfMaterialIndex");
            synchronizeWearLevel = serializedObject.FindProperty("synchronizeWearLevel");
            wearLevel = serializedObject.FindProperty("wearLevel");
            wearLevelDust = serializedObject.FindProperty("wearLevelDust");
            wearLevelRust = serializedObject.FindProperty("wearLevelRust");
            wearLevelGlass = serializedObject.FindProperty("wearLevelGlass");
            levelPeelPaint = serializedObject.FindProperty("levelPeelPaint");
            levelMaskMoss = serializedObject.FindProperty("levelMaskMoss");
            leftDoorOpenAngle = serializedObject.FindProperty("leftDoorOpenAngle");
            rightDoorOpenAngle = serializedObject.FindProperty("rightDoorOpenAngle");
            leftDoorTilt = serializedObject.FindProperty("leftDoorTilt");
            rightDoorTilt = serializedObject.FindProperty("rightDoorTilt");
            trunkOpenAngle = serializedObject.FindProperty("trunkOpenAngle");
            openTrunkReverse = serializedObject.FindProperty("openTrunkReverse");
            hoodOpenAngle = serializedObject.FindProperty("hoodOpenAngle");
            trunkOpenTilt = serializedObject.FindProperty("trunkOpenTilt");
            rearRightTrunkOpenAngle = serializedObject.FindProperty("rearRightTrunkOpenAngle");
            rearLeftTrunkOpenAngle = serializedObject.FindProperty("rearLeftTrunkOpenAngle");
            hoodOpenTilt = serializedObject.FindProperty("hoodOpenTilt");
            holderOpen = serializedObject.FindProperty("holderOpen");
            removeLeftDoor = serializedObject.FindProperty("removeLeftDoor");
            removeRightDoor = serializedObject.FindProperty("removeRightDoor");
            removeTrunk = serializedObject.FindProperty("removeTrunk");
            removeHood = serializedObject.FindProperty("removeHood");
            frontLightIntensity = serializedObject.FindProperty("frontLightIntensity");
            rearLightIntensity = serializedObject.FindProperty("rearLightIntensity");
            windowIsBroken = serializedObject.FindProperty("windowIsBroken");
            frontLeftWheelStateIndex = serializedObject.FindProperty("frontLeftWheelStateIndex");
            frontRightWheelStateIndex = serializedObject.FindProperty("frontRightWheelStateIndex");
            rearLeftWheelStateIndex = serializedObject.FindProperty("rearLeftWheelStateIndex");
            rearRightWheelStateIndex = serializedObject.FindProperty("rearRightWheelStateIndex");
            rotationFrontWheel = serializedObject.FindProperty("rotationFrontWheel");
            randomizeExterior = serializedObject.FindProperty("randomizeExterior");
            randomizeMaterialIndex = serializedObject.FindProperty("randomizeMaterialIndex");
            randomizeEachWindow = serializedObject.FindProperty("randomizeEachWindow");
            randomizeWearLevel = serializedObject.FindProperty("randomizeWearLevel");
            randomizeDoors = serializedObject.FindProperty("randomizeDoors");
            randomizeSlidingDoor = serializedObject.FindProperty("randomizeSlidingDoor");
            randomizeRearTrunkDoors = serializedObject.FindProperty("randomizeRearTrunkDoors");
            randomizeTrunk = serializedObject.FindProperty("randomizeTrunk");
            trunkOpenAngleMin = serializedObject.FindProperty("trunkOpenAngleMin");
            trunkOpenAngleMax = serializedObject.FindProperty("trunkOpenAngleMax");
            trunkOpenTiltMin = serializedObject.FindProperty("trunkOpenTiltMin");
            trunkOpenTiltMax = serializedObject.FindProperty("trunkOpenTiltMax");
            randomizeHood = serializedObject.FindProperty("randomizeHood");
            hoodOpenAngleMax = serializedObject.FindProperty("hoodOpenAngleMax");
            randomizeLightIntensity = serializedObject.FindProperty("randomizeLightIntensity");
            randomizeWindows = serializedObject.FindProperty("randomizeWindows");
            randomizeWheelDeflation = serializedObject.FindProperty("randomizeWheelDeflation");
            burntBodyMaterial = serializedObject.FindProperty("burntBodyMaterial");
            burntWheelMaterial = serializedObject.FindProperty("burntWheelMaterial");
            burntEngineMaterial = serializedObject.FindProperty("burntEngineMaterial");
            burntInteriorMaterial = serializedObject.FindProperty("burntInteriorMaterial");
            burntFrontLightMaterial = serializedObject.FindProperty("burntFrontLightMaterial");
            burntRearLightMaterial = serializedObject.FindProperty("burntRearLightMaterial");
            burntRailingsMaterial = serializedObject.FindProperty("burntRailingsMaterial");
            glassMaterial = serializedObject.FindProperty("glassMaterial");
            isBurnt = serializedObject.FindProperty("isBurnt");
            turnOnLight = serializedObject.FindProperty("turnOnLight");
            showMossOnCar = serializedObject.FindProperty("showMossOnCar");
            showGrassUnderWheels = serializedObject.FindProperty("showGrassUnderWheels");
            grassUnderWheels = serializedObject.FindProperty("grassUnderWheels");
            mossOnCar = serializedObject.FindProperty("mossOnCar");
            randomizeMoss = serializedObject.FindProperty("randomizeMoss");
            randomizeOnLocation = serializedObject.FindProperty("randomizeOnLocation");
            randomizeRemoveHood = serializedObject.FindProperty("randomizeRemoveHood");
            randomizeRemoveTrunk = serializedObject.FindProperty("randomizeRemoveTrunk");
            randomizeRemoveDoors = serializedObject.FindProperty("randomizeRemoveDoors");
            isUniqueInterior = serializedObject.FindProperty("isUniqueInterior");
            interiorMaterials = serializedObject.FindProperty("interiorMaterials");
            hasLiftRearDoor = serializedObject.FindProperty("hasLiftRearDoor");
            has3DBodyKit = serializedObject.FindProperty("has3DBodyKit");
            frontLightBar = serializedObject.FindProperty("frontLightBar");
            frontProtection = serializedObject.FindProperty("frontProtection");
            decalMaterials = serializedObject.FindProperty("decalMaterials");
            selectedDecalIndex = serializedObject.FindProperty("selectedDecalIndex");
            showLightBar = serializedObject.FindProperty("showLightBar");
            showFrontProtection = serializedObject.FindProperty("showFrontProtection");
            turnOnSiren = serializedObject.FindProperty("turnOnSiren");
        }

        public override void OnInspectorGUI()
        {
            CarViewAdvanced carView = (CarViewAdvanced)target;
            serializedObject.Update();
            showModels = EditorGUILayout.Foldout(showModels, "Models", true);
            if (showModels)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(carBody);
                EditorGUILayout.PropertyField(leftDoor);
                EditorGUILayout.PropertyField(rightDoor);
                EditorGUILayout.PropertyField(hasRearDoors);
                if (carView.hasRearDoors)
                {
                    EditorGUILayout.PropertyField(rearRightDoor);
                    EditorGUILayout.PropertyField(rearLeftDoor);
                }
                EditorGUILayout.PropertyField(hasSlidingDoor);
                if (carView.hasSlidingDoor)
                { 
                    carView.slidingDoorObject = (Door)EditorGUILayout.ObjectField("Extra Door Object", carView.slidingDoorObject, typeof(Door), true);
                }
                carView.hasRearTrunkDoors = EditorGUILayout.Toggle("Has Rear Trunk Doors", carView.hasRearTrunkDoors); 
                if (carView.hasRearTrunkDoors)
                {
                    EditorGUILayout.PropertyField(rearLeftTrunkDoor, new GUIContent("Rear Left Door Object"));
                    EditorGUILayout.PropertyField(rearRightTrunkDoor, new GUIContent("Rear Right Door Object"));
                    EditorGUILayout.PropertyField(hasLiftRearDoor, new GUIContent("Has Lift Rear Door"));
                }
                else
                {
                    carView.trunkDoor =
                        (Door)EditorGUILayout.ObjectField("Trunk Door", carView.trunkDoor, typeof(Door),
                            true);
                }
                EditorGUILayout.PropertyField(hood);
                EditorGUILayout.PropertyField(holder);
                EditorGUILayout.PropertyField(frontLeftWheel);
                EditorGUILayout.PropertyField(frontRightWheel);
                EditorGUILayout.PropertyField(rearLeftWheel);
                EditorGUILayout.PropertyField(rearRightWheel);
                EditorGUILayout.PropertyField(light);
                EditorGUILayout.PropertyField(glassBody);
                EditorGUILayout.PropertyField(windows);
                EditorGUILayout.PropertyField(grassUnderWheels);
                EditorGUILayout.PropertyField(mossOnCar);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(10);
            }
        
            showMeshes = EditorGUILayout.Foldout(showMeshes, "Meshes", true);
            if (showMeshes)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(originalWheelLODs);
                EditorGUILayout.PropertyField(deflatedWheelLODs);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(10);
            }
        
            showMaterials = EditorGUILayout.Foldout(showMaterials, "Materials", true);
            if (showMaterials)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(bodyMaterial);
                EditorGUILayout.PropertyField(interiorMaterial);
                EditorGUILayout.PropertyField(brokenWindowMaterial);
                EditorGUILayout.PropertyField(normalWindowMaterial);
                EditorGUILayout.PropertyField(wheelMaterial);
                EditorGUILayout.PropertyField(engineMaterial);
                EditorGUILayout.PropertyField(railingsMaterial);
                EditorGUILayout.PropertyField(invisibleMaterial);
                EditorGUILayout.PropertyField(originalBodyMaterial);
                EditorGUILayout.PropertyField(frontLightMaterial);
                EditorGUILayout.PropertyField(rearLightMaterial);
                EditorGUILayout.PropertyField(glassMaterial);
                EditorGUILayout.PropertyField(setOfMaterials);
                EditorGUILayout.PropertyField(burntBodyMaterial);
                EditorGUILayout.PropertyField(burntWheelMaterial);
                EditorGUILayout.PropertyField(burntEngineMaterial);
                EditorGUILayout.PropertyField(burntInteriorMaterial);
                EditorGUILayout.PropertyField(burntRailingsMaterial);
                EditorGUILayout.PropertyField(burntFrontLightMaterial);
                EditorGUILayout.PropertyField(burntRearLightMaterial);
                EditorGUILayout.PropertyField(isUniqueInterior);
                
                if (carView.isUniqueInterior)
                {
                    EditorGUILayout.PropertyField(interiorMaterials);
                }
                EditorGUILayout.PropertyField(has3DBodyKit, new GUIContent("Has 3D Body Kit"));
                if (carView.has3DBodyKit)
                {
                    EditorGUILayout.PropertyField(frontLightBar, new GUIContent("Front Lightbar "));
                    EditorGUILayout.PropertyField(frontProtection, new GUIContent("Front Protection "));
                    EditorGUILayout.PropertyField(decalMaterials, new GUIContent("Decal Materials"), true);
                }
           
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(10);
            
            }
            
            showSettings = EditorGUILayout.Foldout(showSettings, "Settings", true);
            if (showSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(changeSettings);

                if (!carView.isBurnt)
                {
                    setOfMaterialIndex.intValue = EditorGUILayout.IntSlider("Set of Material Index",
                        carView.setOfMaterialIndex, 0, carView.setOfMaterials.Length);
                    EditorGUILayout.PropertyField(synchronizeWearLevel);
                    EditorGUILayout.PropertyField(wearLevel);
                    EditorGUILayout.PropertyField(wearLevelDust);
                    EditorGUILayout.PropertyField(wearLevelRust);
                    EditorGUILayout.PropertyField(levelPeelPaint);
                    EditorGUILayout.PropertyField(wearLevelGlass);
                    EditorGUILayout.PropertyField(levelMaskMoss);
                }

                EditorGUILayout.PropertyField(isBurnt);
                EditorGUILayout.PropertyField(showMossOnCar); 
                EditorGUILayout.PropertyField(showGrassUnderWheels);  
                EditorGUILayout.Space(10);

                if (carView.has3DBodyKit)
                {
                    int decalCount = carView.decalMaterials != null ? carView.decalMaterials.Length : 0;
                    selectedDecalIndex.intValue = EditorGUILayout.IntSlider("Selected Decal Index", selectedDecalIndex.intValue , 0, decalCount - 1);
                    EditorGUILayout.PropertyField(showLightBar, new GUIContent("Show Lightbar"));
                    EditorGUILayout.PropertyField(showFrontProtection, new GUIContent("Show Front Protection"));
                    EditorGUILayout.PropertyField(turnOnSiren, new GUIContent("Turn On Siren"));
                }
               
                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("Angles and Tilts", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(leftDoorOpenAngle);
                EditorGUILayout.PropertyField(rightDoorOpenAngle);
                if (carView.hasRearDoors)
                {
                    EditorGUILayout.PropertyField(rightRearDoorOpenAngle);
                    EditorGUILayout.PropertyField(leftRearDoorOpenAngle);
                    EditorGUILayout.PropertyField(rightRearDoorTilt);
                    EditorGUILayout.PropertyField(leftRearDoorTilt);
                }

                if (carView.hasSlidingDoor)
                    EditorGUILayout.PropertyField(slidingDoorOpen);
                EditorGUILayout.PropertyField(leftDoorTilt);
                EditorGUILayout.PropertyField(rightDoorTilt);
                if (carView.hasRearTrunkDoors)
                {
                    EditorGUILayout.PropertyField(rearRightTrunkOpenAngle);
                    EditorGUILayout.PropertyField(rearLeftTrunkOpenAngle);
                }
                else
                {
                    EditorGUILayout.PropertyField(trunkOpenAngle);
                    EditorGUILayout.PropertyField(trunkOpenTilt);
                    EditorGUILayout.PropertyField(openTrunkReverse);

                }

                EditorGUILayout.PropertyField(hoodOpenAngle);
                EditorGUILayout.PropertyField(hoodOpenTilt);
                EditorGUILayout.Space(10);

                // Doors and Removal
                EditorGUILayout.LabelField("Doors and Removal", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(holderOpen);
                EditorGUILayout.PropertyField(removeLeftDoor);
                EditorGUILayout.PropertyField(removeRightDoor);
                EditorGUILayout.PropertyField(removeTrunk);
                EditorGUILayout.PropertyField(removeHood);           
                EditorGUILayout.Space(10);
            
                // Lights
                EditorGUILayout.LabelField("Lights", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(frontLightIntensity);
                EditorGUILayout.PropertyField(rearLightIntensity);
                EditorGUILayout.PropertyField(turnOnLight);           
                EditorGUILayout.Space(10);

                // Windows
                EditorGUILayout.LabelField("Windows", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(windowIsBroken);
                EditorGUILayout.Space(10);

                // Wheels
                EditorGUILayout.LabelField("Wheels", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(frontLeftWheelStateIndex);
                EditorGUILayout.PropertyField(frontRightWheelStateIndex);
                EditorGUILayout.PropertyField(rearLeftWheelStateIndex);
                EditorGUILayout.PropertyField(rearRightWheelStateIndex);
                EditorGUILayout.PropertyField(rotationFrontWheel);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(10);
            }

            showRandomization = EditorGUILayout.Foldout(showRandomization, "Randomization", true);
            if (showRandomization)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(randomizeOnLocation);
                EditorGUILayout.PropertyField(randomizeExterior);
                EditorGUILayout.PropertyField(randomizeMaterialIndex, new GUIContent("Randomize Material Index"));
                EditorGUILayout.PropertyField(randomizeWearLevel, new GUIContent("Randomize Wear Level"));
                EditorGUILayout.PropertyField(randomizeDoors, new GUIContent("Randomize Doors"));
                EditorGUILayout.PropertyField(doorOpenAngleMax, new GUIContent("Max Door Open Angle"));
                if (carView.hasSlidingDoor)
                    EditorGUILayout.PropertyField(randomizeSlidingDoor, new GUIContent("Randomize Sliding Door"));
                if (carView.hasRearTrunkDoors)
                    EditorGUILayout.PropertyField(randomizeRearTrunkDoors, new GUIContent("Randomize Rear Trunk Doors"));
                else
                    EditorGUILayout.PropertyField(randomizeTrunk, new GUIContent("Randomize Trunk"));
                EditorGUILayout.PropertyField(trunkOpenAngleMax, new GUIContent("Max Trunk Open Angle"));
            
                EditorGUILayout.PropertyField(randomizeHood, new GUIContent("Randomize Hood"));
                EditorGUILayout.PropertyField(hoodOpenAngleMax, new GUIContent("Max Hood Open Angle"));
                EditorGUILayout.PropertyField(randomizeLightIntensity, new GUIContent("Randomize Light Intensity"));
                EditorGUILayout.PropertyField(randomizeEachWindow, new GUIContent("Randomize Each Windows"));
                EditorGUILayout.PropertyField(randomizeWindows, new GUIContent("Randomize Windows"));
                EditorGUILayout.PropertyField(randomizeWheelDeflation, new GUIContent("Randomize Wheel Deflation"));
                EditorGUILayout.PropertyField(randomizeRemoveHood, new GUIContent("Randomize Remove Hood"));
                EditorGUILayout.PropertyField(randomizeRemoveTrunk, new GUIContent("Randomize Remove Trunk"));
                EditorGUILayout.PropertyField(randomizeRemoveDoors, new GUIContent("Randomize Remove Doors"));
                EditorGUILayout.PropertyField(randomizeMoss, new GUIContent("Randomize Moss"));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(10);
            }
        
            if (GUILayout.Button("Save Values"))
            {
                carView.SaveCurrentValues();
                Debug.Log("Current values have been saved.");
            }
        
            if (GUILayout.Button("Reset to Save Values"))
            {
                carView.ApplySavedValues();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            CarViewAdvanced car = (CarViewAdvanced)target;

            if (Event.current.type == EventType.MouseDrag)
            {
                if (Time.realtimeSinceStartup - _lastUpdateTime >= UpdateInterval)
                {
                    if (car.randomizeOnLocation && _lastPosition != car.transform.localPosition)
                    {
                        car.ApplyRandomization();
                        _lastPosition = car.transform.localPosition;
                        _lastUpdateTime = Time.realtimeSinceStartup;
                    }
                }
            }
            if (Event.current.type == EventType.MouseUp)
            {
                if (car.randomizeOnLocation && _lastPosition != car.transform.localPosition)
                {
                    car.ApplyRandomization();
                    _lastPosition = car.transform.localPosition;
                }
            }
        }
    }
}
