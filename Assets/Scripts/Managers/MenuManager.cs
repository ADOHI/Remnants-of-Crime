using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panelSettings;
    public GameObject panelCharacterInfo;
    public GameObject panelEvidenceList;

    public void ShowPanel(string panelName)
    {
        // 모든 패널 비활성화
        panelSettings.SetActive(false);
        panelCharacterInfo.SetActive(false);
        panelEvidenceList.SetActive(false);

        // 해당하는 패널만 활성화
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
