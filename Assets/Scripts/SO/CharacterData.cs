using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Character")]
public class CharacterData : ScriptableObject
{
    public string characterID;
    public string characterName;
    public Sprite portrait;
    public string description;
}