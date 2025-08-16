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
                Debug.Log($"���ο� ����: {skill.value}");
                // ����: EnemyManager.Instance.SetSlow(skill.value);
                break;

            case PassiveSkillData.SkillType.Barrior:
                Debug.Log($"�踮�� ����: {skill.value}");
                // ����: Player.Instance.SetBarrier(skill.value);
                break;

            case PassiveSkillData.SkillType.Teleport:
                Debug.Log($"�ڷ���Ʈ ���� (��Ÿ�� {skill.value})");
                // ����: Player.Instance.EnableTeleport(skill.value);
                break;
        }
    }

    public PassiveSkillData GetEquippedSkill() => equippedSkill;
}
