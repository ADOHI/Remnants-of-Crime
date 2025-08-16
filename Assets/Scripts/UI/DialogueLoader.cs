using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    public TextAsset dialogueJson;
    private DialogueDatabase database;

    void Awake()
    {
        // JSON Array ¡æ {"dialogues": [...] } ·¡ÇÎ
        string wrappedJson = "{\"dialogues\":" + dialogueJson.text + "}";
        database = JsonUtility.FromJson<DialogueDatabase>(wrappedJson);
    }

    public DialogueData GetDialogueByID(string id)
    {
        foreach (var dialogue in database.dialogues)
        {
            if (dialogue.id == id)
                return dialogue;
        }
        Debug.LogError($"Dialogue with ID '{id}' not found.");
        return null;
    }
}
