using UnityEngine;

public class PassiveSkillManager : MonoBehaviour
{
    public static PassiveSkillManager Instance;
    private PassiveSkillData equippedSkill;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EquipSkill(PassiveSkillData skill)
    {
        equippedSkill = skill;
        ApplySkillEffect(skill);
    }

    private void ApplySkillEffect(PassiveSkillData skill)
    {
        switch (skill.skillType)
        {
            case PassiveSkillData.SkillType.Slow:
                Debug.Log($"슬로우 적용: {skill.value}");
                // 예시: EnemyManager.Instance.SetSlow(skill.value);
                break;

            case PassiveSkillData.SkillType.Barrior:
                Debug.Log($"배리어 적용: {skill.value}");
                // 예시: Player.Instance.SetBarrier(skill.value);
                break;

            case PassiveSkillData.SkillType.Teleport:
                Debug.Log($"텔레포트 적용 (쿨타임 {skill.value})");
                // 예시: Player.Instance.EnableTeleport(skill.value);
                break;
        }
    }

    public PassiveSkillData GetEquippedSkill() => equippedSkill;
}
