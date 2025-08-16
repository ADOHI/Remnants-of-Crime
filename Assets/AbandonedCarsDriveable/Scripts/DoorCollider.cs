using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class DoorCollider : MonoBehaviour
    {
        [SerializeField] private Door _door;
    
        public void Handle(DoorHandler player)
        {
            _door.Handle(player);
        }
    }
}
