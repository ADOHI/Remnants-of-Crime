using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class CutSceneController : MonoBehaviour
{
    public DialogueUI dialogueUI;
    public CutsceneData cutsceneData; // ScriptableObject 컷씬 데이터
    public PlayableDirector playableDirector; // Timeline 제어용

    private void OnEnable()
    {
        if (dialogueUI != null)
            dialogueUI.OnDialogueAllEnd += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        if (dialogueUI != null)
            dialogueUI.OnDialogueAllEnd -= HandleDialogueEnd;
    }

    // 컷씬 실행
    public void PlayCutscene()
    {
        if (cutsceneData == null || dialogueUI == null || playableDirector == null)
        {
            Debug.LogError("CutsceneData, DialogueUI, or PlayableDirector is missing!");
            return;
        }

        playableDirector.Play(); // 타임라인 실행
        StartCoroutine(RunCutscene());
    }

    // 컷씬 시퀀스 실행 (대사 싱크용)
    private IEnumerator RunCutscene()
    {
        foreach (var sceneLine in cutsceneData.lines)
        {
            if (sceneLine.character == null) continue;

            // ✅ 대사 시작 전에 타임라인 멈춤
            PauseTimeline();

            yield return dialogueUI.ShowDialogue(sceneLine.character, sceneLine.dialogues, false);

            // ✅ 대사가 끝난 후 타임라인 재개
            ResumeTimeline();
        }
    }

    private void HandleDialogueEnd()
    {
        Debug.Log("컷씬 전체 종료!");
        // TODO: 카메라 전환, 다음 이벤트 호출 등
    }

    // 👉 Timeline 제어용 메서드
    public void PauseTimeline()
    {
        if (playableDirector != null)
            playableDirector.Pause();
    }

    public void ResumeTimeline()
    {
        if (playableDirector != null)
            playableDirector.Play();
    }
}
