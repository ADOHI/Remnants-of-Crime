using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    public Image characterPortrait;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public CanvasGroup dialogueCanvas;
    public Image backgroundPanel;   // ✅ 대화창 배경 패널 추가

    [Header("Typing Settings")]
    public float typeSpeed = 0.05f;

    public bool IsPlaying { get; private set; }

    private bool isTyping = false;
    private bool isNextRequested = false;
    private string currentSentence = "";
    private Coroutine typingCoroutine;

    // 이벤트
    public event Action OnDialogueLineEnd;
    public event Action OnDialogueAllEnd;

    private void Awake()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.alpha = 0f;
            dialogueCanvas.blocksRaycasts = false;
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(false); // 시작 시 숨김
        }
    }

    private void Update()
    {
        // 👉 입력은 여기서만 받는다
        if (IsPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            SkipOrNext();
        }
    }

    public IEnumerator ShowDialogue(CharacterData charData, DialogueLine[] lines, bool autoNext = false)
    {
        StopAllCoroutines();
        IsPlaying = true;

        if (dialogueCanvas != null)
        {
            dialogueCanvas.alpha = 1f;
            dialogueCanvas.blocksRaycasts = true;
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(true); // ✅ 대화 시작 시 보이기
        }

        nameText.text = charData.characterName;

        foreach (var line in lines)
        {
            // 표정 적용
            if (line.expression >= 0 && line.expression < charData.expressions.Length)
                characterPortrait.sprite = charData.expressions[line.expression];
            else
                characterPortrait.sprite = charData.portrait;

            currentSentence = line.text;

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeSentence(line.text));
            yield return typingCoroutine;

            if (!autoNext)
            {
                isNextRequested = false;
                while (!isNextRequested) // 스페이스 입력 기다림
                    yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            // ✅ 줄 끝났을 때 이벤트 호출
            OnDialogueLineEnd?.Invoke();
        }

        if (dialogueCanvas != null)
        {
            dialogueCanvas.alpha = 0f;
            dialogueCanvas.blocksRaycasts = false;
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(false); // ✅ 대화 끝나면 숨기기
        }

        IsPlaying = false;
        OnDialogueAllEnd?.Invoke();
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        // 무조건 끝까지 출력
        dialogueText.text = currentSentence;
        isTyping = false;
    }


    public void SkipOrNext()
    {
        if (!IsPlaying) return;

        if (isTyping)
        {

        }
        else
        {
            isNextRequested = true; // 다음 줄로
        }
    }
}
