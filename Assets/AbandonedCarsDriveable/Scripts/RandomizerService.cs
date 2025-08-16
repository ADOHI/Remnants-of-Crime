using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class RandomizerService
    {
        private readonly WearService _wearService;
        private readonly DoorService _doorService;
        private readonly WheelService _wheelService;
        private readonly LightService _lightService;
        private readonly WindowService _windowService;

        public RandomizerService(
            WearService wearService, DoorService doorService,
            WheelService wheelService, LightService lightService, WindowService windowService)
        {
            _wearService = wearService;
            _doorService = doorService;
            _wheelService = wheelService;
            _lightService = lightService;
            _windowService = windowService;
        }

        public RandomValues GetRandomValues(CarViewAdvanced carView)
        {
            var randomValues = new RandomValues
            {
                WearLevelDust = carView.randomizeWearLevel ? (float?)Random.Range(0f, 1f) : null,
                WearLevelRust = carView.randomizeWearLevel ? (float?)Random.Range(0f, 1f) : null,
                SetOfMaterialIndex = carView.randomizeMaterialIndex ? (int?)Random.Range(0, 5) : null,
                LeftDoorOpenAngle = carView.randomizeDoors ? (float?)Random.Range(90f, 180 - carView.doorOpenAngleMax) : null,
                RightDoorOpenAngle = carView.randomizeDoors ? (float?)Random.Range(90f, 0 + carView.doorOpenAngleMax) : null,
                LeftRearDoorOpenAngle = carView.hasRearDoors && carView.randomizeDoors ? (float?)Random.Range(90f, 180 - carView.doorOpenAngleMax) : null,
                RightRearDoorOpenAngle = carView.hasRearDoors && carView.randomizeDoors ? (float?)Random.Range(90f, 0 + carView.doorOpenAngleMax) : null,
                LeftDoorTilt = carView.randomizeDoors ? (float?)Random.Range(-5f, 0f) : null,
                RightDoorTilt = carView.randomizeDoors ? (float?)Random.Range(-5f, 0f) : null,
                LeftRearDoorTilt = carView.hasRearDoors && carView.randomizeDoors ? (float?)Random.Range(-5f, 0f) : null,
                RightRearDoorTilt = carView.hasRearDoors && carView.randomizeDoors ? (float?)Random.Range(-5f, 0f) : null,
                SlidingDoorOpen = carView.hasSlidingDoor && carView.randomizeSlidingDoor ? (float?)Random.Range(0f, 1f) : null,
                RearRightTrunkOpenAngle = carView.hasRearTrunkDoors && carView.randomizeRearTrunkDoors ? (float?)Random.Range(90f, 90 - carView.trunkOpenAngleMax) : null,
                RearLeftTrunkOpenAngle = carView.hasRearTrunkDoors && carView.randomizeRearTrunkDoors ? (float?)Random.Range(90f, 90 + carView.trunkOpenAngleMax) : null,
                TrunkOpenAngle = !carView.hasRearTrunkDoors && carView.randomizeTrunk ? (float?)Random.Range(0, carView.trunkOpenAngleMax) : null,
                TrunkOpenTilt = carView.randomizeTrunk ? (float?)Random.Range(80, 100) : null,
                HoodOpenAngle = carView.randomizeHood ? (float?)Random.Range(0, carView.hoodOpenAngleMax) : null,
                HoodOpenTilt = carView.randomizeHood ? (float?)Random.Range(80, 100) : null,
                FrontLightIntensity = carView.randomizeLightIntensity ? (float?)Random.Range(0f, 10f) : null,
                RearLightIntensity = carView.randomizeLightIntensity ? (float?)Random.Range(0f, 10f) : null,
                LevelMaskMoss = carView.randomizeMoss? Random.Range(0f, 1f) : null,
                RemoveLeftDoor = carView.randomizeRemoveDoors ? (bool?)(Random.value > 0.5f) : null,
                RemoveRightDoor = carView.randomizeRemoveDoors ? (bool?)(Random.value > 0.5f) : null,
                RemoveHood = carView.randomizeRemoveHood ? (bool?)(Random.value > 0.5f) : null,
                RemoveTrunk = carView.randomizeRemoveTrunk ? (bool?)(Random.value > 0.5f) : null,
            
            };

            if (!carView.randomizeWindows || carView.windows == null || carView.windows.Length <= 0) return randomValues;
            for (int i = 0; i < carView.windows.Length; i++)
            {
                randomValues.WindowIsBroken.Add(Random.value > 0.5f);
            }

            return randomValues;
        }

    }
}
