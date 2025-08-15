using UnityEngine;

public class ToggleMenuUI : MonoBehaviour
{
    public GameObject settingsTab;
    public GameObject characterInfoTab;
    public GameObject evidenceTab;
    public GameObject testimonyTab;

    bool isActive = false;

    public void ToggleMenu()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void OpenTab(GameObject tab)
    {
        settingsTab.SetActive(false);
        characterInfoTab.SetActive(false);
        evidenceTab.SetActive(false);
        testimonyTab.SetActive(false);

        tab.SetActive(true);
    }
}