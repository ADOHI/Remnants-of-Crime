using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Character")]
public class CharacterData : ScriptableObject
{
    [Header("General Info")]
    public string characterID;      // 고유 ID
    public string characterName;    // 캐릭터 이름
    public Sprite portrait;         // 기본 초상화

    [Header("Expressions")]
    public Sprite[] expressions;    // 표정 배열 (0=기본, 1=웃는얼굴, 2=화난얼굴 등)

    [Header("Profile Tab")]
    [TextArea(5, 5)]
    public string Discription;
}
