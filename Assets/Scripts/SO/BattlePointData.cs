using UnityEngine;

[CreateAssetMenu(menuName = "GameData/BattlePoint")]
public class BattlePointData : ScriptableObject
{
    [Header("기본 정보")]
    public string pointID;
    public string pointName;
    public Sprite pointIcon;
    [TextArea] public string description;

    [Header("전투 관련")]
    [Range(0, 100)] public float clearRate;       // 클리어율 %
    public EnemyData[] enemyPool;                 // 출현 적 데이터
    public Vector2Int baseEnemyCount;             // 최소~최대 적 수
    public float clearRateEnemyBonus = 20f;       // 클리어율 %당 적 증가 규칙
    public float clearRateIncreaseMin = 15f;      // 전투 후 상승 최소값
    public float clearRateIncreaseMax = 20f;      // 전투 후 상승 최대값

    [Header("보상 관련")]
    public EvidenceData rewardEvidence;           // 100% 보상 증거품
    public int baseCashReward;                     // 기본 현금 보상
}
