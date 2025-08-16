using UnityEngine;

[CreateAssetMenu(menuName = "GameData/BattlePoint")]
public class BattlePointData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string pointID;
    public string pointName;
    public Sprite pointIcon;
    [TextArea] public string description;

    [Header("���� ����")]
    [Range(0, 100)] public float clearRate;       // Ŭ������ %
    public EnemyData[] enemyPool;                 // ���� �� ������
    public Vector2Int baseEnemyCount;             // �ּ�~�ִ� �� ��
    public float clearRateEnemyBonus = 20f;       // Ŭ������ %�� �� ���� ��Ģ
    public float clearRateIncreaseMin = 15f;      // ���� �� ��� �ּҰ�
    public float clearRateIncreaseMax = 20f;      // ���� �� ��� �ִ밪

    [Header("���� ����")]
    public EvidenceData rewardEvidence;           // 100% ���� ����ǰ
    public int baseCashReward;                     // �⺻ ���� ����
}
