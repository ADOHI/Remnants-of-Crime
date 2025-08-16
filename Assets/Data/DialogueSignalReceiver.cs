using UnityEngine;
using UnityEngine.Playables;

public class DialogueSignalReceiver : MonoBehaviour, INotificationReceiver
{
    public DialogueManager dialogueManager;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is DialogueSignalAsset signal)
        {
            if (dialogueManager != null)
            {
                dialogueManager.PlayById(signal.DialogueID);
            }
            else
            {
                Debug.LogWarning("DialogueManager not assigned on DialogueSignalReceiver!");
            }
        }
    }
}
