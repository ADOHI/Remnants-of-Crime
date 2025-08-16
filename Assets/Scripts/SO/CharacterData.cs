using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Character")]
public class CharacterData : ScriptableObject
{
    [Header("General Info")]
    public string characterID;      // ���� ID
    public string characterName;    // ĳ���� �̸�
    public Sprite portrait;         // �⺻ �ʻ�ȭ

    [Header("Expressions")]
    public Sprite[] expressions;    // ǥ�� �迭 (0=�⺻, 1=���¾�, 2=ȭ���� ��)

    [Header("Profile Tab")]
    [TextArea(5, 5)]
    public string Discription;
}
