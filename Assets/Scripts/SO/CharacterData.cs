using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Character")]
public class CharacterData : ScriptableObject
{
    [Header("General Info")]
    public string characterID;      // 고유 ID
    public string characterName;    // 캐릭터 이름
    public Sprite portrait;         // 기본 초상화

    [Header("Profile Tab")]
    [TextArea(5, 5)]
    public string Discription;


    [Header("Relation Tab1")]
    public RelationshipData[] relationships; // 관계 데이터 배열

    [Header("Relation Tab2")]
    public CaseRelationshipData[] caserelationships; // 관계 데이터 배열

    [Header("Transcript Tab")]
    public TranscriptData[] transcripts;

}

[System.Serializable]
public class RelationshipData
{
    public string targetCharacterID;   // 관계 대상 캐릭터 ID
    public string relationDescription; // 관계 설명
    public Sprite relationIcon;        // 관계 아이콘
}


[System.Serializable]
public class CaseRelationshipData
{
    public string targetCase;   // 사건 이름
    public string relationDescription; // 관계 설명
    public Sprite relationIcon;        // 관계 아이콘
}

[System.Serializable]
public class TranscriptData
{
    public string Question;
    public string Answer;
}