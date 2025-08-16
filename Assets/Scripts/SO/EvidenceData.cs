using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Evidence")]
public class EvidenceData : ScriptableObject
{
    public string evidenceID;   // ���� ID
    public string evidenceName; // ����ǰ �̸�
    [TextArea(3, 5)]
    public string description;  // ����
    public Sprite image;        // ����ǰ �̹���
}
