using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Character")]
public class CharacterData : ScriptableObject
{
    [Header("General Info")]
    public string characterID;      // ���� ID
    public string characterName;    // ĳ���� �̸�
    public Sprite portrait;         // �⺻ �ʻ�ȭ

    [Header("Profile Tab")]
    [TextArea(5, 5)]
    public string Discription;


    [Header("Relation Tab1")]
    public RelationshipData[] relationships; // ���� ������ �迭

    [Header("Relation Tab2")]
    public CaseRelationshipData[] caserelationships; // ���� ������ �迭

    [Header("Transcript Tab")]
    public TranscriptData[] transcripts;

}

[System.Serializable]
public class RelationshipData
{
    public string targetCharacterID;   // ���� ��� ĳ���� ID
    public string relationDescription; // ���� ����
    public Sprite relationIcon;        // ���� ������
}


[System.Serializable]
public class CaseRelationshipData
{
    public string targetCase;   // ��� �̸�
    public string relationDescription; // ���� ����
    public Sprite relationIcon;        // ���� ������
}

[System.Serializable]
public class TranscriptData
{
    public string Question;
    public string Answer;
}