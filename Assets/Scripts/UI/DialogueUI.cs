using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public Image characterPortrait;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public float typeSpeed = 0.05f;

    public bool IsPlaying { get; private set; }

    public CanvasGroup dialogueCanvas;

    private bool isTyping = false;
    private bool isNextRequested = false;
    private string currentSentence = "";
    private Coroutine typingCoroutine;

    private void Awake()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.alpha = 0f;
            dialogueCanvas.blocksRaycasts = false;
        }
    }

    public IEnumerator ShowDialogue(CharacterData charData, string[] sentences, bool autoNext = false)
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.alpha = 1f;
            dialogueCanvas.blocksRaycasts = true;
        }

        characterPortrait.sprite = charData.portrait;
        nameText.text = charData.characterName;

        foreach (var sentence in sentences)
        {
            currentSentence = sentence;
            typingCoroutine = StartCoroutine(TypeSentence(sentence));
            yield return typingCoroutine;

            if (!autoNext)
            {
                isNextRequested = false;
                while (!isNextRequested)
                    yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (dialogueCanvas != null)
        {
            dialogueCanvas.alpha = 0f;
            dialogueCanvas.blocksRaycasts = false;
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        IsPlaying = true;
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);

            if (!isTyping) // 스킵된 경우
                break;
        }

        // 즉시완성 시에도 전체 문장 출력 보장
        dialogueText.text = currentSentence;
        isTyping = false;
        IsPlaying = false;
    }

    public void SkipOrNext()
    {
        if (isTyping)
        {
            // 타이핑 중이면 즉시완성
            isTyping = false;
        }
        else
        {
            // 이미 완성된 상태면 다음 문장으로 이동 요청
            isNextRequested = true;
        }
    }
}
