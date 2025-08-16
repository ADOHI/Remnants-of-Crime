using System.Collections.Generic;

namespace AbandonedCarsDriveable.Scripts
{
    public class RandomValues
    {
        public int? SetOfMaterialIndex;
        public float? WearLevelRust;
        public float? WearLevelDust;
        public float? LeftDoorOpenAngle;
        public float? RightDoorOpenAngle;
        public float? LeftRearDoorOpenAngle;
        public float? RightRearDoorOpenAngle;
        public float? LeftDoorTilt;
        public float? RightDoorTilt;
        public float? LeftRearDoorTilt;
        public float? RightRearDoorTilt;
        public float? SlidingDoorOpen;
        public float? RearRightTrunkOpenAngle;
        public float? RearLeftTrunkOpenAngle;
        public float? TrunkOpenAngle;
        public float? TrunkOpenTilt;
        public float? HoodOpenAngle;
        public float? HoodOpenTilt;
        public float? LevelMaskMoss;
        public float? FrontLightIntensity;
        public float? RearLightIntensity; 
        public bool? RemoveLeftDoor;
        public bool? RemoveRightDoor;
        public bool? RemoveHood;
        public bool? RemoveTrunk;
        public List<bool?> WindowIsBroken { get; set; } = new List<bool?>();
    }
}
