using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueSceneChanger : MonoBehaviour
{
    public DialogueUI dialogueUI;
    public string nextSceneName;

    void Start()
    {
        if (dialogueUI != null)
        {
            dialogueUI.OnDialogueAllEnd += HandleDialogueEnd;
        }
    }

    private void HandleDialogueEnd()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
