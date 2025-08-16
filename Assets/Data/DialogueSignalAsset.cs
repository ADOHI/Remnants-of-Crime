using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline; // �߿�!

[CreateAssetMenu(menuName = "Signals/Dialogue Signal")]
public class DialogueSignalAsset : SignalAsset   // �� PlayableAsset �� �ƴ�!
{
    [SerializeField] private string dialogueID;
    public string DialogueID => dialogueID;
}
