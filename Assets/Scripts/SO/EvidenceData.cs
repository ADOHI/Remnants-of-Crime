using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Evidence")]
public class EvidenceData : ScriptableObject
{
    public string evidenceID;
    public string evidenceName;
    public Sprite evidenceSprite;
    [TextArea] public string description;
    public PassiveSkillData passiveSkill; // ���� �� �ߵ� �нú�
}
