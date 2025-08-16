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

    // 🔥 string[] → DialogueLine[] 변환해서 넘기도록 수정
    public void ShowDialogue(string charID, string[] sentences, bool autoNext = false)
    {
        var charData = dataManager.GetCharacterData(charID);
        if (charData == null)
        {
            Debug.LogError($"CharacterData not found for ID: {charID}");
            return;
        }

        // string[] -> DialogueLine[] 변환 (표정은 -1로 기본 처리)
        DialogueLine[] lines = new DialogueLine[sentences.Length];
        for (int i = 0; i < sentences.Length; i++)
        {
            lines[i] = new DialogueLine
            {
                text = sentences[i],
                expression = -1 // 표정 값 없음 → 기본 초상화 사용
            };
        }

        StartCoroutine(dialogueUI.ShowDialogue(charData, lines, autoNext));
    }

    public void ShowDialogueSequence(DialogueData data, bool autoNext = false)
    {
        var firstChar = dataManager.GetCharacterData(data.lines[0].characterID);
        if (firstChar == null)
        {
            Debug.LogError("CharacterData not found");
            return;
        }

        StartCoroutine(dialogueUI.ShowDialogue(firstChar, data.lines, autoNext));
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

            // ✅ DialogueLine 배열 하나로 감싸서 전달
            yield return StartCoroutine(dialogueUI.ShowDialogue(charData, new DialogueLine[] { line }, autoNext));
        }
    }
}
