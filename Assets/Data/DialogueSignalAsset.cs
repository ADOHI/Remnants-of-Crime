using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline; // Áß¿ä!

[CreateAssetMenu(menuName = "Signals/Dialogue Signal")]
public class DialogueSignalAsset : SignalAsset   // ¡ç PlayableAsset °¡ ¾Æ´Ô!
{
    [SerializeField] private string dialogueID;
    public string DialogueID => dialogueID;
}
