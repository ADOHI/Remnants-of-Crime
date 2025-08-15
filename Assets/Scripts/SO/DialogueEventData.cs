using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Dialogue Event")]
public class DialogueEvent : ScriptableObject
{
    public string characterID;
    [TextArea(2, 5)]
    public string[] sentences; // ���� �� ��� ����
}
