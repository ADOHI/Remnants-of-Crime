using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public DialogueLoader loader;
    public DataManager dataManager;
    public DialogueUI ui;
    public PlayableDirector director;   // 타임라인 디렉터

    private Coroutine playing;
    private bool waitingForResume = false;

    private void Awake()
    {
        // ✅ 대사 한 줄 끝났을 때 호출
        ui.OnDialogueLineEnd += OnDialogueLineFinished;
    }

    private void OnDestroy()
    {
        if (ui != null)
            ui.OnDialogueLineEnd -= OnDialogueLineFinished;
    }

    public void PlayByID(string dialogueId)
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

            // UI에서 대사 출력 및 입력 관리
            yield return ui.ShowDialogue(charData, new DialogueLine[] { line }, autoNext: false);

            // 👉 Resume은 OnDialogueLineFinished 에서만 처리
            // PauseMarker에 걸려 있으면, 별도의 입력 대기 코루틴에서 Resume 시도
        }

        playing = null;
    }

    private void OnDialogueLineFinished()
    {
        if (director == null) return;

        if (!waitingForResume)
        {
            waitingForResume = true;
            StartCoroutine(WaitForResumeInput());
        }
    }

    private IEnumerator WaitForResumeInput()
    {
        // PauseMarker 도달 시까지 기다림
        // PauseMarker가 없으면 그냥 이 코루틴은 끝나지 않고 영향 없음
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        // 👉 Resume 시도 (PauseMarker에 걸려있다면 정상 동작)
        director.Resume();
        waitingForResume = false;
    }

}
