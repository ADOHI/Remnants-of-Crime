using UnityEngine;

[CreateAssetMenu(menuName = "GameData/PassiveSkill")]
public class PassiveSkillData : ScriptableObject
{
    public string skillID;
    public string skillName;
    [TextArea] public string description;

    public enum SkillType
    {
        Slow,
        Barrior,
        Teleport
    }
    public SkillType skillType;
    public float value;
}
