using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace AbandonedCarsDriveable.Scripts
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private CarConfig _config;
        [SerializeField] private bool _isDriverDoor;
        [SerializeField] private CarInteractionManager _carInteractionManager;
        [SerializeField] private HolderConfig _holderConfig;
    
        [FormerlySerializedAs("_openAngleForTilt")] public float openAngleForTilt;

        public bool OpenedAngleMore => _config.OpenClosedValue.OpenValue > _config.OpenClosedValue.ClosedValue;
    
        [SerializeField] private OpenClosedValue _currentOpenClosedValue;
        public UnityEvent DoorOpened;
        public UnityEvent DoorClosed;

        private bool _isOpen = false;
        private float _currentTilt;
        private DoorService _doorService;
        private SimpleCarController _carController;
    
        public void Init(DoorService doorService, SimpleCarController carController)
        {
            try
            {
                _doorService = doorService;
                if (_currentOpenClosedValue.ClosedValue == 0 && _currentOpenClosedValue.OpenValue == 0)
                {
                    _currentOpenClosedValue = new OpenClosedValue(_config.OpenClosedValue.OpenValue,
                        _config.OpenClosedValue.ClosedValue);
                }

                if (carController != null)
                {
                    _carController = carController;
                }
            }
            catch (Exception )
            {
                // ignored
            }
        }

        public void UpdateDoorData(float angle, float tilt)
        {
            var halfAngle = (_config.OpenClosedValue.ClosedValue + _config.OpenClosedValue.OpenValue) / 2;
        
            if (OpenedAngleMore)
            {
                if (angle > halfAngle)
                {
                    _currentOpenClosedValue.OpenValue = angle;
                    _currentOpenClosedValue.ClosedValue = Math.Abs(tilt - _config.Tilt) > 0 ? openAngleForTilt : _config.OpenClosedValue.ClosedValue;
                }
                else
                {
                    _currentOpenClosedValue.ClosedValue = angle;
                    _currentOpenClosedValue.OpenValue = _config.OpenClosedValue.OpenValue;
                }
            }
            else
            {
                if (angle > halfAngle)
                {
                    _currentOpenClosedValue.ClosedValue = angle;
                    _currentOpenClosedValue.OpenValue = _config.OpenClosedValue.OpenValue;
                }
                else
                {
                    _currentOpenClosedValue.OpenValue = angle;
                    _currentOpenClosedValue.ClosedValue = Math.Abs(tilt - _config.Tilt) > 0 ? openAngleForTilt : _config.OpenClosedValue.ClosedValue;

                }
            }

            _currentTilt = tilt;
        }
        [ContextMenu("Handle")]
        public void Handle(DoorHandler player)
        {

            if (!_isOpen)
            {
                OpenDoor(_config.HandleDoorType, _config.ExtraDoor);
                _isOpen = true;
            }
            else
            {
                CloseDoor(_config.HandleDoorType, _config.ExtraDoor);
                _isOpen = false;
            }

            if (_isDriverDoor && _carInteractionManager != null)
            {
                if (!_carInteractionManager.InCar)
                {
                    _carInteractionManager.Init(player);
                    _carInteractionManager.EnterCar();
                }
            }
        }

        private void OpenDoor(HandleDoorType handleDoorType, DoorService.ExtraDoor extraDoor)
        {
            if (handleDoorType == HandleDoorType.Default)
            {
                if (extraDoor == DoorService.ExtraDoor.None)
                    _doorService.OpenDoor(gameObject, _currentOpenClosedValue.OpenValue, _config.Tilt, DoorService.ProccessType.Smooth);
                else if (extraDoor == DoorService.ExtraDoor.Hood && _holderConfig != null)
                {
                    if (_currentOpenClosedValue.OpenValue == _config.OpenClosedValue.OpenValue && _currentTilt == _config.Tilt)
                    {
                        _doorService.OpenHolder(1 ,_holderConfig.StartRotationEuler, _holderConfig.EndRotationEuler );
                    }
                    _doorService.OpenExtraDoor(extraDoor, _currentOpenClosedValue.OpenValue, _currentTilt,
                        DoorService.ProccessType.Smooth);
                }
                else
                    _doorService.OpenExtraDoor(extraDoor, _currentOpenClosedValue.OpenValue, _currentTilt,
                        DoorService.ProccessType.Smooth);
            }
            else
                _doorService.SlideDoor(gameObject, _currentOpenClosedValue.OpenValue, DoorService.ProccessType.Smooth);

            DoorOpened?.Invoke();
        }
        public void CloseAfterEntry()
        {
            if (!_isOpen) return;

            CloseDoor(_config.HandleDoorType, _config.ExtraDoor);
            _isOpen = false;
        }

        private void CloseDoor(HandleDoorType handleDoorType, DoorService.ExtraDoor extraDoor)
        {
            if (handleDoorType == HandleDoorType.Default)
            {
                if (extraDoor == DoorService.ExtraDoor.None)
                    _doorService.OpenDoor(gameObject, _currentOpenClosedValue.ClosedValue, _currentTilt,
                        DoorService.ProccessType.Smooth);
                else if (extraDoor == DoorService.ExtraDoor.Hood && _holderConfig != null)
                {
                    _doorService.OpenHolder(0 ,_holderConfig.StartRotationEuler, _holderConfig.EndRotationEuler );
                    _doorService.OpenExtraDoor(extraDoor, _currentOpenClosedValue.ClosedValue, _currentTilt,
                        DoorService.ProccessType.Smooth);
                }
                else
                    _doorService.OpenExtraDoor(extraDoor, _currentOpenClosedValue.ClosedValue, _currentTilt,
                        DoorService.ProccessType.Smooth);
            }
            else
                _doorService.SlideDoor(gameObject, _currentOpenClosedValue.ClosedValue, DoorService.ProccessType.Smooth);

            DoorClosed?.Invoke();
        }

    }

    [Serializable]
    public class CarConfig
    {
        public HandleDoorType HandleDoorType;
        public OpenClosedValue OpenClosedValue;
        public float Tilt;
        public DoorService.ExtraDoor ExtraDoor;
    }

    [Serializable]
    public class OpenClosedValue
    {
        public float OpenValue;
        public float ClosedValue;

        public OpenClosedValue(float openValue, float closedValue)
        {
            OpenValue = openValue;
            ClosedValue = closedValue;
        }
    }

    public enum HandleDoorType
    {
        Default = 0,
        Sliding = 1
    }
}