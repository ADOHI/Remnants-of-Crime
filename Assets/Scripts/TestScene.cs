using UnityEngine;

public class TestScene : MonoBehaviour
{
    public UIManager ui;
    public DialogueLoader loader;

    void Start()
    {
        DialogueData data = loader.GetDialogueById("intro01");
        if (data != null)
        {
            ui.ShowDialogueSequence(data, false); // false = 스페이스바 넘김
        }
    }
}