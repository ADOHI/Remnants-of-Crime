[System.Serializable]
public class DialogueLine
{
    public string characterID;
    public string text;
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
