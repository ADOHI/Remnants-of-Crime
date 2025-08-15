using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panelSettings;
    public GameObject panelCharacterInfo;
    public GameObject panelEvidenceList;

    public void ShowPanel(string panelName)
    {
        // ��� �г� ��Ȱ��ȭ
        panelSettings.SetActive(false);
        panelCharacterInfo.SetActive(false);
        panelEvidenceList.SetActive(false);

        // �ش��ϴ� �гθ� Ȱ��ȭ
        switch (panelName)
        {
            case "Settings":
                panelSettings.SetActive(true);
                break;
            case "CharacterInfo":
                panelCharacterInfo.SetActive(true);
                break;
            case "EvidenceList":
                panelEvidenceList.SetActive(true);
                break;
        }
    }
}
