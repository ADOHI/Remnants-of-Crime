using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Evidence")]
public class EvidenceData : ScriptableObject
{
    public string evidenceID;
    public string evidenceName;
    public Sprite evidenceSprite;
    [TextArea] public string description;
    public PassiveSkillData passiveSkill; // 장착 시 발동 패시브
}
