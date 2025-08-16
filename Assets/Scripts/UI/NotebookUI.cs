using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public RectTransform background; // 백그라운드(부모)
    public PlayerStatsData playerStats; // 스탯 데이터 SO
    public EvidenceData[] evidences; // 증거품 데이터
    public SpriteData spriteDatas;   // 책갈피용 스프라이트
    public TMP_FontAsset defaultFont; // 기본 폰트 (인스펙터에 넣기)

    private GameObject slotGrid;
    private GameObject detailsPage;

    private void Start()
    {
        CreateTabs();
    }

    // ====== 탭 생성 ======
    private void CreateTabs()
    {
        string[] tabNames = { "스탯 강화", "증거품", "설정" };
        for (int i = 0; i < tabNames.Length; i++)
        {
            // 책갈피 버튼만 스프라이트 적용
            GameObject btnObj = CreateUIButton(tabNames[i], background, new Vector2(-730, 200 - (i * 200)), true, spriteDatas.sprites[i]);
            int index = i;
            btnObj.GetComponent<Button>().onClick.AddListener(() => OnTabClick(index));
            btnObj.GetComponent<RectTransform>().localScale = new Vector2(-1, 1);
        }
    }

    private void OnTabClick(int index)
    {
        if (index == 0) ShowStatsTab();
        else if (index == 1) ShowEvidenceList();
        // index == 2 → 설정
    }

    // ====== 스탯 강화 탭 ======
    private void ShowStatsTab()
    {
        if (slotGrid != null) Destroy(slotGrid);
        if (detailsPage != null) Destroy(detailsPage);

        slotGrid = new GameObject("StatsGrid", typeof(RectTransform));
        slotGrid.transform.SetParent(background, false);

        var grid = slotGrid.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(200, 200);
        grid.spacing = new Vector2(20, 20);
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = 2;

        slotGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 100);

        CreateStatSlot("체력 (HP)", playerStats.hp,
            () => { playerStats.hp = Mathf.Max(0, playerStats.hp - 10); RefreshStats(); },
            () => { playerStats.hp += 10; RefreshStats(); });

        CreateStatSlot("스태미나", playerStats.stamina,
            () => { playerStats.stamina = Mathf.Max(0, playerStats.stamina - 5); RefreshStats(); },
            () => { playerStats.stamina += 5; RefreshStats(); });

        CreateStatSlot("현금 획득률", (playerStats.cashGainRate * 100f).ToString("F1") + "%",
            () => { playerStats.cashGainRate = Mathf.Max(0, playerStats.cashGainRate - 0.1f); RefreshStats(); },
            () => { playerStats.cashGainRate += 0.1f; RefreshStats(); });

        CreateStatSlot("공격력", playerStats.attack,
            () => { playerStats.attack = Mathf.Max(0, playerStats.attack - 1); RefreshStats(); },
            () => { playerStats.attack += 1; RefreshStats(); });
    }

    private void CreateStatSlot(string label, object value, UnityEngine.Events.UnityAction onMinus, UnityEngine.Events.UnityAction onPlus)
    {
        GameObject slot = new GameObject("StatSlot", typeof(RectTransform), typeof(Image));
        slot.transform.SetParent(slotGrid.transform, false);

        var slotImg = slot.GetComponent<Image>();
        slotImg.sprite = null; // 기본 단색
        slotImg.color = new Color(1, 1, 1, 0.2f); // 살짝 투명한 흰색 배경

        var vLayout = slot.AddComponent<VerticalLayoutGroup>();
        vLayout.spacing = 10;
        vLayout.childAlignment = TextAnchor.MiddleCenter;

        // 스탯 텍스트
        var text = CreateUIText($"{label}: {value}", slot.transform, Vector2.zero, new Vector2(180, 40));
        text.GetComponent<RectTransform>().localScale = new Vector2(0.8f, 0.8f);

        // 버튼 그룹
        GameObject btnGroup = new GameObject("BtnGroup", typeof(RectTransform));
        btnGroup.transform.SetParent(slot.transform, false);

        var hLayout = btnGroup.AddComponent<HorizontalLayoutGroup>();
        hLayout.spacing = 40;
        hLayout.childAlignment = TextAnchor.MiddleCenter;

        var btnGroupLayout = btnGroup.AddComponent<LayoutElement>();
        btnGroupLayout.preferredHeight = 50;

        // - 버튼
        GameObject minusBtn = CreateUIButton("-", btnGroup.transform, Vector2.zero);
        minusBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 40);
        minusBtn.GetComponent<Button>().onClick.AddListener(onMinus);

        // + 버튼
        GameObject plusBtn = CreateUIButton("+", btnGroup.transform, Vector2.zero);
        plusBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 40);
        plusBtn.GetComponent<Button>().onClick.AddListener(onPlus);
    }

    private void RefreshStats()
    {
        ShowStatsTab();
    }

    // ====== 증거품 목록 ======
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
        img.sprite = data.evidenceSprite; // 증거품은 원래 스프라이트
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

        // 좌측 이미지
        GameObject leftPage = new GameObject("LeftPage", typeof(RectTransform));
        leftPage.transform.SetParent(detailsPage.transform, false);
        leftPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 0);

        var img = leftPage.AddComponent<Image>();
        img.sprite = data.evidenceSprite;
        img.preserveAspect = true;

        // 우측 텍스트
        GameObject rightPage = new GameObject("RightPage", typeof(RectTransform));
        rightPage.transform.SetParent(detailsPage.transform, false);
        rightPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 0);

        CreateUIText(data.evidenceName, rightPage.transform, new Vector2(150, 120), new Vector2(100, 50));
        CreateUIText(data.description, rightPage.transform, new Vector2(150, 40), new Vector2(200, 50));

        if (data.passiveSkill != null)
        {
            CreateUIText("패시브: " + data.passiveSkill.skillName, rightPage.transform, new Vector2(150, -40), new Vector2(250, 50));
            CreateUIText(data.passiveSkill.description, rightPage.transform, new Vector2(150, -80), new Vector2(250, 50));
        }

        GameObject equipBtn = CreateUIButton("장착하기", rightPage.transform, new Vector2(150, -150));
        equipBtn.GetComponent<Button>().onClick.AddListener(() => EquipEvidence(data));
    }

    private void EquipEvidence(EvidenceData data)
    {
        if (data.passiveSkill != null)
        {
            PassiveSkillManager.Instance.EquipSkill(data.passiveSkill);
            Debug.Log($"[{data.evidenceName}] 장착 → 패시브 [{data.passiveSkill.skillName}] 발동!");
        }
    }

    // ====== 공용 UI 생성 ======
    private GameObject CreateUIButton(string text, Transform parent, Vector2 anchoredPos, bool useSprite = false, Sprite sprite = null)
    {
        GameObject obj = new GameObject(text + "_Button", typeof(RectTransform), typeof(Image), typeof(Button));
        obj.transform.SetParent(parent, false);

        var img = obj.GetComponent<Image>();
        if (useSprite && sprite != null) img.sprite = sprite;
        else
        {
            img.sprite = null; // 기본 단색 버튼
            img.color = new Color(1, 1, 1, 0.2f);
        }

        // 버튼 텍스트
        var textObj = CreateUIText(text, obj.transform, Vector2.zero, new Vector2(50, 50));

        // 책갈피 버튼은 뒤집힌 상태 → 텍스트만 다시 정방향 보정
        if (useSprite)
        {
            textObj.GetComponent<RectTransform>().localScale = new Vector2(-1, 1);
        }

        obj.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        return obj;
    }

    private GameObject CreateUIText(string text, Transform parent, Vector2 anchoredPos, Vector2 size)
    {
        GameObject obj = new GameObject("Text", typeof(RectTransform));
        obj.transform.SetParent(parent, false);

        var tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.font = defaultFont; // 기본 폰트
        tmp.color = Color.black; // ✅ 기본색 검정
        tmp.enableAutoSizing = true;
        tmp.alignment = TextAlignmentOptions.Center;

        obj.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        obj.GetComponent<RectTransform>().sizeDelta = size;
        return obj;
    }

}
