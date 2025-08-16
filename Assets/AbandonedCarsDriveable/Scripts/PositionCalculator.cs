using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class PositionCalculator
    {
        private readonly GameObject _car;

        public PositionCalculator(GameObject car)
        {
            _car = car;
        }

        public void UpdateCarPosition(int frontLeftWheelStateIndex, int rearLeftWheelStateIndex, int frontRightWheelStateIndex, int rearRightWheelStateIndex)
        {
            float normalHeight = 0f; 
            float deflatedHeight = -0.01f; 
            float hiddenHeight = -0.2f; 
            float frontLeftOffset = GetWheelHeightOffset((WheelState)frontLeftWheelStateIndex, normalHeight,
                deflatedHeight, hiddenHeight);
            float frontRightOffset = GetWheelHeightOffset((WheelState)rearLeftWheelStateIndex, normalHeight,
                deflatedHeight, hiddenHeight); 
            float rearLeftOffset = GetWheelHeightOffset((WheelState)frontRightWheelStateIndex, normalHeight,
                deflatedHeight, hiddenHeight); 
            float rearRightOffset = GetWheelHeightOffset((WheelState)rearRightWheelStateIndex, normalHeight,
                deflatedHeight, hiddenHeight);

            float averageHeight = (frontLeftOffset + frontRightOffset + rearLeftOffset + rearRightOffset) / 4;

            if (averageHeight < hiddenHeight) averageHeight = hiddenHeight;

            float frontAverage = (frontLeftOffset + frontRightOffset) / 2;
            float rearAverage = (rearLeftOffset + rearRightOffset) / 2;
            float frontTilt = (rearAverage - frontAverage) * 30;

            bool isDiagonalFL_RR = frontLeftWheelStateIndex != (int)WheelState.Hidden &&
                                   rearRightWheelStateIndex != (int)WheelState.Hidden
                                   && frontRightWheelStateIndex == (int)WheelState.Hidden &&
                                   rearLeftWheelStateIndex == (int)WheelState.Hidden;
            bool isDiagonalFR_RL = frontRightWheelStateIndex != (int)WheelState.Hidden &&
                                   rearLeftWheelStateIndex != (int)WheelState.Hidden
                                   && frontLeftWheelStateIndex == (int)WheelState.Hidden &&
                                   rearRightWheelStateIndex == (int)WheelState.Hidden;

            if (isDiagonalFL_RR || isDiagonalFR_RL)
            {
                frontTilt -= 1.0f; 
            }

            float leftAverage = (frontLeftOffset + rearLeftOffset) / 2;
            float rightAverage = (frontRightOffset + rearRightOffset) / 2;
            float sideTilt = (leftAverage - rightAverage) * 20;

            Quaternion targetRotation = Quaternion.Euler(frontTilt, _car.transform.rotation.eulerAngles.y, sideTilt);

            _car.transform.localPosition = new Vector3(_car.transform.localPosition.x, averageHeight, _car.transform.localPosition.z);

            _car.transform.rotation = targetRotation;
        }


        private float GetWheelHeightOffset(WheelState state, float normal, float deflated, float hidden)
        {
            return state switch
            {
                WheelState.Normal => normal,
                WheelState.Deflated => deflated,
                WheelState.Hidden => hidden,
                _ => 0f
            };
        }
    }

    public enum WheelState
    {
        Normal = 0,
        Deflated = 1,
        Hidden = 2
    }
}