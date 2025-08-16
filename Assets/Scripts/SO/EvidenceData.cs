using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Evidence")]
public class EvidenceData : ScriptableObject
{
    public string evidenceID;   // 고유 ID
    public string evidenceName; // 증거품 이름
    [TextArea(3, 5)]
    public string description;  // 설명
    public Sprite image;        // 증거품 이미지
}
