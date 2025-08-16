using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class HolderConfig : MonoBehaviour
    {
        [SerializeField] private Quaternion startRotation;
        [SerializeField] private Quaternion endRotation;

        public Quaternion StartRotationEuler => startRotation;
        public Quaternion EndRotationEuler => endRotation;
    }
}

