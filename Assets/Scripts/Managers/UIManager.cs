using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public DialogueUI dialogueUI;
    public ToggleMenuUI toggleMenuUI;
    public DataManager dataManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggleMenuUI.ToggleMenu();
        }

        // 스페이스 입력은 DialogueUI로 전달
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueUI.SkipOrNext();
        }
    }

    public void ShowDialogue(string charID, string[] sentences, bool autoNext = false)
    {
        var charData = dataManager.GetCharacterData(charID);
        if (charData == null)
        {
            Debug.LogError($"CharacterData not found for ID: {charID}");
            return;
        }
        StartCoroutine(dialogueUI.ShowDialogue(charData, sentences, autoNext));
    }

    public void ShowDialogueSequence(DialogueData data, bool autoNext = false)
    {
        StartCoroutine(PlayDialogue(data, autoNext));
    }

    private IEnumerator PlayDialogue(DialogueData data, bool autoNext)
    {
        foreach (var line in data.lines)
        {
            var charData = dataManager.GetCharacterData(line.characterID);
            if (charData == null)
            {
                Debug.LogError($"CharacterData not found for ID: {line.characterID}");
                continue;
            }

            yield return StartCoroutine(dialogueUI.ShowDialogue(charData, new string[] { line.text }, autoNext));
        }
    }
}
