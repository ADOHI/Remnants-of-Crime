using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public DialogueLoader loader;
    public DataManager dataManager;
    public DialogueUI ui;
    public PlayableDirector director;   // 타임라인 디렉터

    Coroutine playing;

    private void Awake()
    {
        // ✅ 줄 끝났을 때 타임라인 Resume
        ui.OnDialogueLineEnd += () =>
        {
            if (director != null)
            {
                director.Resume();
            }
        };
    }

    public void PlayById(string dialogueId)
    {
        var data = loader.GetDialogueByID(dialogueId);
        if (data == null) return;

        if (playing != null) StopCoroutine(playing);
        playing = StartCoroutine(PlayRoutine(data.lines));
    }

    private IEnumerator PlayRoutine(DialogueLine[] lines)
    {
        foreach (var line in lines)
        {
            var charData = dataManager.GetCharacterData(line.characterID);
            if (charData == null) continue;

            // 한 줄 출력 (UI에서 스페이스 입력 관리)
            yield return ui.ShowDialogue(charData, new DialogueLine[] { line }, autoNext: false);

            // 👉 Resume은 UI 이벤트(OnDialogueLineEnd)에서 처리되므로 여기서는 안 해도 됨
        }

        playing = null;
    }
}
