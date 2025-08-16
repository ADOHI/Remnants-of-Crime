using UnityEngine;

[CreateAssetMenu(menuName = "GameData/SpriteData")]
public class SpriteData : ScriptableObject
{
    [SerializeField] public Sprite[] sprites;
}
