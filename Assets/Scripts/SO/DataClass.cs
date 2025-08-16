using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterID;
    public int expression;
    public string text;
}

[System.Serializable]
public class CutsceneLine
{
    public CharacterData character;   // 어떤 캐릭터가 말하는지
    public DialogueLine[] dialogues;  // 그 캐릭터의 대사들
}

[CreateAssetMenu(menuName = "GameData/Cutscene")]
public class CutsceneData : ScriptableObject
{
    public string cutsceneID;         // 컷씬 고유 ID
    public CutsceneLine[] lines;      // 컷씬 시퀀스 (캐릭터 + 대사들)
}

[System.Serializable]
public class DialogueData
{
    public string id;
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueDatabase
{
    public DialogueData[] dialogues;
}
