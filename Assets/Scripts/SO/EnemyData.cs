using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyID;
    public string enemyName;
    public Sprite enemySprite;
    public int hp;
    public int attack;
    public int rewardCash; // óġ �� ����
    public bool isElite;
}
