using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueEvent dialogueEvent;
    public UIManager uiManager;
    public bool autoNext = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.ShowDialogue(dialogueEvent.characterID, dialogueEvent.sentences, autoNext);
        }
    }
}
