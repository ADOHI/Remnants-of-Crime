using UnityEngine;

[CreateAssetMenu(menuName = "GameData/PlayerStats")]
public class PlayerStatsData : ScriptableObject
{
    public int hp;
    public int stamina;
    public float cashGainRate; // Çö±Ý È¹µæÀ²
    public int attack;

    public void Accept(IStatVisitor visitor)
    {
        visitor.Visit(this);
    }
}
