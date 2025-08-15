using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Evidence")]
public class EvidenceData : ScriptableObject
{
    public string evidenceID;
    public string title;
    public Sprite image;
    public string description;
}