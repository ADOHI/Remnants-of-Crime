using System.Collections;
using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class DoorService
    {
        public enum DoorSide { Left, Right }
        public enum ExtraDoor { Trunk, Hood, None}
        public enum ProccessType { Smooth = 0, Instantly = 1}

        private GameObject _leftDoor;
        private GameObject _rightDoor;
        private GameObject _trunkDoor;
        private GameObject _trunkDoorTop;
        private GameObject _hood;
        private GameObject _holder;

        private Vector3 _slidingDoorStartPosition = new (0.885988832f, 1.49805665f, -1.10209751f);
        private Vector3 _slidingDoorFirstPosition = new (0.885988832f, 1.49805665f, -1.176f);
        private Vector3 _slidingDoorEndPosition = new (-0.639999986f, 1.516f, -1.176f);

        public DoorService( GameObject rightDoor,GameObject leftDoor, GameObject trunkDoor,GameObject trunkDoorTop, GameObject hood, GameObject holder)
        {
            _leftDoor = leftDoor;
            _rightDoor = rightDoor;
        
            _hood = hood;
            if (holder != null)
                _holder = holder;
            if (trunkDoor != null) 
                _trunkDoor = trunkDoor;
            if (trunkDoorTop != null)
                _trunkDoorTop = trunkDoorTop;

        }

        public void OpenDoor(DoorSide side, float angle, float tilt)
        {
            GameObject door = side == DoorSide.Left ? _leftDoor : _rightDoor;
            if (door != null)
            {
                door.transform.localRotation = Quaternion.Euler(tilt, angle, door.transform.rotation.z);
            }
        } 
        public void OpenDoor(GameObject door, float angle, float tilt, ProccessType proccessType = ProccessType.Instantly)
        {
            if (door != null)
            {
                Vector3 localRotation = door.transform.localEulerAngles; 
                Quaternion targetRotation = Quaternion.Euler(tilt, angle, localRotation.z);

                if (proccessType == ProccessType.Instantly)
                {
                    door.transform.localRotation = targetRotation;
                }
                else
                {
                    CoroutineRunner.Instance.StartCoroutine(SmoothProccesRotation(door, targetRotation, 1));
                }
            }
        }
        public void OpenExtraDoor(ExtraDoor doorType, float angle, float tilt, ProccessType proccessType = ProccessType.Instantly)
        {
            if (doorType == ExtraDoor.Trunk)
            {
               

                if (_trunkDoor != null)
                {
                    Quaternion targetRotation = Quaternion.Euler(angle, tilt, 0);
                    if (proccessType == ProccessType.Instantly)
                        _trunkDoor.transform.localRotation = targetRotation;
                    else
                        CoroutineRunner.Instance.StartCoroutine(SmoothProccesRotation(_trunkDoor, targetRotation, 1));
                }

                if (_trunkDoorTop != null)
                {
                    Quaternion targetRotation = Quaternion.Euler(-angle, tilt, 0);
                    if (proccessType == ProccessType.Instantly)
                        _trunkDoorTop.transform.localRotation = targetRotation;
                    else
                        CoroutineRunner.Instance.StartCoroutine(SmoothProccesRotation(_trunkDoorTop,  targetRotation, 1));
                }

                return;
            }

            if (_hood == null) return;
            Quaternion hoodTarget = Quaternion.Euler(angle, tilt, 0);

            if (proccessType == ProccessType.Instantly)
            {
                _hood.transform.localRotation = hoodTarget;
            }
            else
            {
                CoroutineRunner.Instance.StartCoroutine(SmoothProccesRotation(_hood, hoodTarget, 1));
            }
        }


        public void RemoveDoor(DoorSide side, bool remove)
        {
            GameObject door = side == DoorSide.Left ? _leftDoor : _rightDoor;
            if (door != null)
            {
                door.SetActive(!remove);
            }
        }

        public void RemoveExtraDoor(ExtraDoor doorType, bool remove)
        {
            switch (doorType)
            {
                case ExtraDoor.Trunk:
                {
                    if (_trunkDoor != null) _trunkDoor.SetActive(!remove);
                    if (_trunkDoorTop != null) _trunkDoorTop.SetActive(!remove);
                    break;
                }
                case ExtraDoor.Hood:
                {
                    if (_hood != null) _hood.SetActive(!remove);
                    break;
                }
            }
        }

        public void OpenHolder(float sliderValue , Quaternion startRotation,Quaternion endRotation)
        {
            if (_holder == null) return;
            switch (sliderValue)
            {
                case 1f:
                    CoroutineRunner.Instance.StartCoroutine(SmoothProccesRotation(_holder, startRotation, endRotation, 1.5f));
                    break;
                case 0:
                    _holder.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, sliderValue);
                    break;
            }

        } 
        public void OpenHolderCar(float sliderValue , Quaternion startRotation,Quaternion endRotation)
        {
            if (_holder == null) return;
            _holder.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, sliderValue);
        }
        private IEnumerator SmoothProccesRotation(GameObject targetObject, Quaternion startRotation, Quaternion targetRotation, float duration)
        {
            if (targetObject == null) 
                yield break;

            Quaternion initialRotation = targetObject.transform.localRotation;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                targetObject.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }
            targetObject.transform.localRotation = targetRotation;
        }


        public void SlideDoor(GameObject slidingDoor, float sliderValue, ProccessType handleDoorType = ProccessType.Instantly)
        {
            if (slidingDoor != null)
            {
                Vector3 targetPosition;

                if (sliderValue < 0.1f)
                {
                    targetPosition = Vector3.Lerp(_slidingDoorStartPosition, _slidingDoorFirstPosition, sliderValue / 0.1f);
                }
                else
                {
                    targetPosition = Vector3.Lerp(_slidingDoorFirstPosition, _slidingDoorEndPosition, (sliderValue - 0.1f) / 0.9f);
                }

                if (handleDoorType == ProccessType.Instantly)
                {
                    slidingDoor.transform.localPosition = targetPosition;
                }
                else
                {
                    CoroutineRunner.Instance.StartCoroutine(SmoothProccesPosition(slidingDoor.transform, 
                        slidingDoor.transform.localPosition, targetPosition, 1));
                }
            }
        }


        public void OpenRearDoors(GameObject rearLeftDoor,GameObject rearRightDoor, float leftDoorAngle, float rightDoorAngle , float leftTilt , float rightTilt, bool hasLiftRearDoor )
        {
            if (rearLeftDoor != null && hasLiftRearDoor)
            {
                rearLeftDoor.transform.localRotation = Quaternion.Euler( leftDoorAngle + 180f, leftTilt, 0);
            }
            else if (rearLeftDoor != null)
            {
                rearLeftDoor.transform.localRotation = Quaternion.Euler(leftTilt, leftDoorAngle, 0);
            }

            if (rearRightDoor != null && hasLiftRearDoor)
            {
                rearRightDoor.transform.localRotation = Quaternion.Euler( rightDoorAngle , rightTilt,0);
            }
            else if (rearRightDoor != null)
            {
                rearRightDoor.transform.localRotation = Quaternion.Euler(rightTilt, rightDoorAngle, 0);
            }
        }


        private IEnumerator SmoothProccesRotation(GameObject door, Quaternion targetRotation, float duration)
        {
            Quaternion startRotation = door.transform.localRotation;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                door.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            door.transform.localRotation = targetRotation;
        }


        private IEnumerator SmoothProccesPosition(Transform slidingDoor, Vector3 startPosition, Vector3 endPosition, float duration)
        {
            if (slidingDoor == null) 
                yield break;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                slidingDoor.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                yield return null; 
            }

            slidingDoor.localPosition = endPosition;
        }
    }
}

