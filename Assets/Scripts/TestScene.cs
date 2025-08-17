using UnityEngine;

public class TestScene : MonoBehaviour
{
    public UIManager ui;
    public DialogueLoader loader;
    public string DID;

    void Start()
    {
        DialogueData data = loader.GetDialogueByID(DID);
        if (data != null)
        {
            ui.ShowDialogueSequence(data, false); // false = 스페이스바 넘김
        }
    }
}