using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public RectTransform background; // ��׶���(�θ�)
    public PlayerStatsData playerStats; // ���� ������ SO
    public EvidenceData[] evidences; // ����ǰ ������

    private GameObject slotGrid;
    private GameObject detailsPage;

    private void Start()
    {
        CreateTabs();
    }

    // ====== �� ���� ======
    private void CreateTabs()
    {
        string[] tabNames = { "���� ��ȭ", "����ǰ", "����" };
        for (int i = 0; i < tabNames.Length; i++)
        {
            GameObject btnObj = CreateUIButton(tabNames[i], background, new Vector2(-800, 200 - (i * 200)));
            int index = i;
            btnObj.GetComponent<Button>().onClick.AddListener(() => OnTabClick(index));
        }
    }

    private void OnTabClick(int index)
    {
        if (index == 0) ShowStatsTab();
        else if (index == 1) ShowEvidenceList();
        // index == 2 �� ����
    }

    // ====== ���� ��ȭ �� ======
    private void ShowStatsTab()
    {
        if (slotGrid != null) Destroy(slotGrid);
        if (detailsPage != null) Destroy(detailsPage);

        slotGrid = new GameObject("StatsGrid", typeof(RectTransform));
        slotGrid.transform.SetParent(background, false);

        var grid = slotGrid.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(200, 120);  // ���� ũ�� �˳���
        grid.spacing = new Vector2(20, 20);
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = 1; // ���η� ��ġ

        slotGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 100);

        CreateStatSlot("ü�� (HP)", playerStats.hp,
            () => { playerStats.hp = Mathf.Max(0, playerStats.hp - 10); RefreshStats(); },
            () => { playerStats.hp += 10; RefreshStats(); });

        CreateStatSlot("���¹̳�", playerStats.stamina,
            () => { playerStats.stamina = Mathf.Max(0, playerStats.stamina - 5); RefreshStats(); },
            () => { playerStats.stamina += 5; RefreshStats(); });

        CreateStatSlot("���� ȹ���", (playerStats.cashGainRate * 100f).ToString("F1") + "%",
            () => { playerStats.cashGainRate = Mathf.Max(0, playerStats.cashGainRate - 0.1f); RefreshStats(); },
            () => { playerStats.cashGainRate += 0.1f; RefreshStats(); });

        CreateStatSlot("���ݷ�", playerStats.attack,
            () => { playerStats.attack = Mathf.Max(0, playerStats.attack - 1); RefreshStats(); },
            () => { playerStats.attack += 1; RefreshStats(); });
    }

    private void CreateStatSlot(string label, object value, UnityEngine.Events.UnityAction onMinus, UnityEngine.Events.UnityAction onPlus)
    {
        // ���� ���
        GameObject slot = new GameObject("StatSlot", typeof(RectTransform), typeof(Image));
        slot.transform.SetParent(slotGrid.transform, false);

        var slotImg = slot.GetComponent<Image>();
        slotImg.color = new Color(0.2f, 0.2f, 0.2f, 0.6f); // ������ ȸ�� ���

        var vLayout = slot.AddComponent<VerticalLayoutGroup>();
        vLayout.spacing = 10;
        vLayout.childAlignment = TextAnchor.MiddleCenter;

        // ���� �ؽ�Ʈ
        CreateUIText($"{label}: {value}", slot.transform, Vector2.zero, new Vector2(180, 40));

        // ��ư �׷�
        GameObject btnGroup = new GameObject("BtnGroup", typeof(RectTransform));
        btnGroup.transform.SetParent(slot.transform, false);

        var btnGroupRect = btnGroup.GetComponent<RectTransform>();
        var hLayout = btnGroup.AddComponent<HorizontalLayoutGroup>();
        hLayout.spacing = 20;
        hLayout.childAlignment = TextAnchor.MiddleCenter;

        // ��ư �׷� ũ�� ����
        var btnGroupLayout = btnGroup.AddComponent<LayoutElement>();
        btnGroupLayout.preferredHeight = 50;
        btnGroupLayout.minHeight = 50;

        // - ��ư
        GameObject minusBtn = CreateUIButton("-", btnGroup.transform, Vector2.zero);
        minusBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 40);
        var minusLayout = minusBtn.AddComponent<LayoutElement>();
        minusLayout.preferredWidth = 60;
        minusLayout.preferredHeight = 40;
        minusBtn.GetComponent<Button>().onClick.AddListener(onMinus);

        // + ��ư
        GameObject plusBtn = CreateUIButton("+", btnGroup.transform, Vector2.zero);
        plusBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 40);
        var plusLayout = plusBtn.AddComponent<LayoutElement>();
        plusLayout.preferredWidth = 60;
        plusLayout.preferredHeight = 40;
        plusBtn.GetComponent<Button>().onClick.AddListener(onPlus);
    }



    private void RefreshStats()
    {
        ShowStatsTab();
    }

    // ====== ����ǰ ��� ======
    private void ShowEvidenceList()
    {
        if (slotGrid != null) Destroy(slotGrid);
        if (detailsPage != null) Destroy(detailsPage);

        slotGrid = new GameObject("EvidenceGrid", typeof(RectTransform));
        slotGrid.transform.SetParent(background, false);

        var gridLayout = slotGrid.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(120, 140);
        gridLayout.spacing = new Vector2(10, 10);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 6;
        slotGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150);

        for (int i = 0; i < evidences.Length && i < 12; i++)
        {
            if (evidences[i] == null) continue;
            GameObject slot = CreateEvidenceSlot(evidences[i]);
            slot.transform.SetParent(slotGrid.transform, false);
        }
    }

    private GameObject CreateEvidenceSlot(EvidenceData data)
    {
        GameObject slot = new GameObject(data.evidenceName, typeof(RectTransform));
        var vLayout = slot.AddComponent<VerticalLayoutGroup>();
        vLayout.spacing = 5;
        vLayout.childAlignment = TextAnchor.MiddleCenter;

        GameObject imgObj = new GameObject("Image", typeof(RectTransform));
        imgObj.transform.SetParent(slot.transform, false);
        var img = imgObj.AddComponent<Image>();
        img.preserveAspect = true;
        imgObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        CreateUIText(data.evidenceName, slot.transform, Vector2.zero, new Vector2(20, 10));

        var btn = slot.AddComponent<Button>();
        btn.onClick.AddListener(() => ShowEvidenceDetails(data));

        return slot;
    }

    private void ShowEvidenceDetails(EvidenceData data)
    {
        if (detailsPage != null) Destroy(detailsPage);
        if (slotGrid != null) Destroy(slotGrid);

        detailsPage = new GameObject("EvidenceDetails", typeof(RectTransform));
        detailsPage.transform.SetParent(background, false);

        GameObject leftPage = new GameObject("LeftPage", typeof(RectTransform));
        leftPage.transform.SetParent(detailsPage.transform, false);
        leftPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 0);

        var img = leftPage.AddComponent<Image>();
        img.preserveAspect = true;

        GameObject rightPage = new GameObject("RightPage", typeof(RectTransform));
        rightPage.transform.SetParent(detailsPage.transform, false);
        rightPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 0);

        CreateUIText(data.evidenceName, rightPage.transform, new Vector2(0, 100), new Vector2(20, 10));
        CreateUIText(data.description, rightPage.transform, Vector2.zero, new Vector2(20, 10));
    }

    // ====== ���� UI ���� ======
    private GameObject CreateUIButton(string text, Transform parent, Vector2 anchoredPos)
    {
        GameObject obj = new GameObject(text + "_Button", typeof(RectTransform), typeof(Image), typeof(Button));
        obj.transform.SetParent(parent, false);
        CreateUIText(text, obj.transform, Vector2.zero, new Vector2(20, 10));
        obj.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        return obj;
    }

    private GameObject CreateUIText(string text, Transform parent, Vector2 anchoredPos, Vector2 size)
    {
        GameObject obj = new GameObject("Text", typeof(RectTransform));
        obj.transform.SetParent(parent, false);
        var tmp = obj.AddComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.enableAutoSizing = true;
        tmp.alignment = TextAlignmentOptions.Center;

        obj.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        obj.GetComponent<RectTransform>().sizeDelta = size;
        return obj;
    }
}
