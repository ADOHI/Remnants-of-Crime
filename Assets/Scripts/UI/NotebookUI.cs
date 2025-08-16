using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public RectTransform background; // 백그라운드(부모)
    public CharacterData[] characters;
    public EvidenceData[] evidences; // 인스펙터에서 등록

    private GameObject characterGrid;
    private GameObject detailsPage;

    private int currentCharacterIndex = 0;
    private int currentInfoTabIndex = 0;

    private readonly string[] infoTabs = { "기본정보", "캐릭터 관계", "사건 관계", "대화 기록" };

    void Start()
    {
        CreateTabs();
    }

    // ====== 탭 생성 ======
    private void CreateTabs()
    {
        string[] tabNames = { "인물", "증거품", "설정" };
        for (int i = 0; i < tabNames.Length; i++)
        {
            // [위치값 설정] 탭 버튼 위치
            GameObject btnObj = CreateUIButton(tabNames[i], background, new Vector2(-800 , 200 - (i * 200)));
            int index = i;
            btnObj.GetComponent<Button>().onClick.AddListener(() => OnTabClick(index));
        }
    }

    private void OnTabClick(int index)
    {
        if (index == 0) ShowCharacterList();
        else if (index == 1) ShowEvidenceList();
        // index == 2 → 설정 UI
    }

    // ====== 인물 목록 표시 ======
    private void ShowCharacterList()
    {
        if (characterGrid != null) Destroy(characterGrid);
        if (detailsPage != null) Destroy(detailsPage);

        characterGrid = new GameObject("CharacterGrid", typeof(RectTransform));
        characterGrid.transform.SetParent(background, false);

        var gridLayout = characterGrid.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(200, 240); // [위치값 설정] 셀 크기
        gridLayout.spacing = new Vector2(20, 20);    // [위치값 설정] 셀 간격
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 6; // 가로 6개
        characterGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 250); // [위치값 설정] 그리드 위치

        for (int i = 0; i < characters.Length && i < 12; i++) // 최대 6x2 = 12개
        {
            if (characters[i] == null) continue;
            GameObject slot = CreateCharacterSlot(characters[i]);
            slot.transform.SetParent(characterGrid.transform, false);
        }
    }

    private void ShowEvidenceList()
    {
        if (characterGrid != null) Destroy(characterGrid);
        if (detailsPage != null) Destroy(detailsPage);

        characterGrid = new GameObject("EvidenceGrid", typeof(RectTransform));
        characterGrid.transform.SetParent(background, false);

        var gridLayout = characterGrid.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(120, 140);
        gridLayout.spacing = new Vector2(10, 10);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 6;
        characterGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150);

        for (int i = 0; i < evidences.Length && i < 12; i++)
        {
            if (evidences[i] == null) continue;
            GameObject slot = CreateEvidenceSlot(evidences[i]);
            slot.transform.SetParent(characterGrid.transform, false);
        }
    }


    private GameObject CreateCharacterSlot(CharacterData data)
    {
        GameObject slot = new GameObject(data.characterName, typeof(RectTransform));
        var vLayout = slot.AddComponent<VerticalLayoutGroup>();
        vLayout.spacing = 5;
        vLayout.childAlignment = TextAnchor.MiddleCenter;

        // 초상화
        GameObject imgObj = new GameObject("Portrait", typeof(RectTransform));
        imgObj.transform.SetParent(slot.transform, false);
        var img = imgObj.AddComponent<Image>();
        img.sprite = data.portrait;
        img.preserveAspect = true;
        imgObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100); // [위치값 설정] 초상화 크기

        // 이름
        CreateUIText(data.characterName, slot.transform, Vector2.zero, new Vector2(20, 10));

        // 클릭 시 상세 페이지
        var btn = slot.AddComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            currentCharacterIndex = System.Array.IndexOf(characters, data);
            ShowCharacterDetails();
        });

        return slot;
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
        img.sprite = data.image;
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
        if (characterGrid != null) Destroy(characterGrid);

        detailsPage = new GameObject("EvidenceDetails", typeof(RectTransform));
        detailsPage.transform.SetParent(background, false);

        GameObject leftPage = new GameObject("LeftPage", typeof(RectTransform));
        leftPage.transform.SetParent(detailsPage.transform, false);
        leftPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 0);

        var img = leftPage.AddComponent<Image>();
        img.sprite = data.image;
        img.preserveAspect = true;

        GameObject rightPage = new GameObject("RightPage", typeof(RectTransform));
        rightPage.transform.SetParent(detailsPage.transform, false);
        rightPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 0);

        CreateUIText(data.evidenceName, rightPage.transform, new Vector2(0, 100), new Vector2(20, 10));
        CreateUIText(data.description, rightPage.transform, Vector2.zero, new Vector2(20, 10));
    }


    // ====== 상세 페이지 ======
    private void ShowCharacterDetails()
    {
        if (detailsPage != null) Destroy(detailsPage);
        if (characterGrid != null) Destroy(characterGrid);

        detailsPage = new GameObject("DetailsPage", typeof(RectTransform));
        detailsPage.transform.SetParent(background, false);

        // 왼쪽 페이지
        GameObject leftPage = new GameObject("LeftPage", typeof(RectTransform));
        leftPage.transform.SetParent(detailsPage.transform, false);
        leftPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 0); // [위치값 설정] 왼쪽 페이지 위치
        leftPage.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);

        var charImg = leftPage.AddComponent<Image>();
        charImg.sprite = characters[currentCharacterIndex].portrait;
        charImg.preserveAspect = true;

        // 위 화살표
        GameObject upBtn = CreateUIButton("▲", leftPage.transform, new Vector2(0, 300)); // [위치값 설정]
        upBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            currentCharacterIndex = (currentCharacterIndex - 1 + characters.Length) % characters.Length;
            ShowCharacterDetails();
        });

        // 아래 화살표
        GameObject downBtn = CreateUIButton("▼", leftPage.transform, new Vector2(0, -300)); // [위치값 설정]
        downBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Length;
            ShowCharacterDetails();
        });

        // 오른쪽 페이지
        GameObject rightPage = new GameObject("RightPage", typeof(RectTransform));
        rightPage.transform.SetParent(detailsPage.transform, false);
        rightPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 0); // [위치값 설정]

        UpdateRightPage(rightPage);
    }

    private void UpdateRightPage(GameObject rightPage)
    {
        foreach (Transform child in rightPage.transform)
            Destroy(child.gameObject);

        // 탭 이름
        CreateUIText(infoTabs[currentInfoTabIndex], rightPage.transform, new Vector2(0, 300), new Vector2(300, 50));

        // 내용
        string content = "";
        var data = characters[currentCharacterIndex];
        switch (currentInfoTabIndex)
        {
            case 0: // 기본 정보
                content = data.Discription;
                break;
            case 1: // 캐릭터 관계
                foreach (var rel in data.relationships)
                    content += $"- {rel.targetCharacterID}: {rel.relationDescription}\n";
                break;
            case 2: // 사건 관계
                foreach (var rel in data.caserelationships)
                    content += $"- {rel.targetCase}: {rel.relationDescription}\n";
                break;
            case 3: // 대화 기록
                foreach (var t in data.transcripts)
                    content += $"Q: {t.Question}\nA: {t.Answer}\n\n";
                break;
        }

        CreateUIText(content, rightPage.transform, new Vector2(0, 0), new Vector2(400,400));

        // 좌/우 화살표 생성
        if (currentInfoTabIndex > 0) // 첫 페이지가 아닐 때만 좌측 화살표
        {
            GameObject leftArrow = CreateUIButton("◀", rightPage.transform, new Vector2(-150, -300)); // [위치값 설정]
            leftArrow.GetComponent<Button>().onClick.AddListener(() =>
            {
                currentInfoTabIndex = Mathf.Max(currentInfoTabIndex - 1, 0);
                UpdateRightPage(rightPage);
            });
        }

        if (currentInfoTabIndex < infoTabs.Length - 1) // 마지막 페이지가 아닐 때만 우측 화살표
        {
            GameObject rightArrow = CreateUIButton("▶", rightPage.transform, new Vector2(150, -300)); // [위치값 설정]
            rightArrow.GetComponent<Button>().onClick.AddListener(() =>
            {
                currentInfoTabIndex = Mathf.Min(currentInfoTabIndex + 1, infoTabs.Length - 1);
                UpdateRightPage(rightPage);
            });
        }
    }

    // ====== 공용 UI 생성 ======
    private GameObject CreateUIButton(string text, Transform parent, Vector2 anchoredPos)
    {
        GameObject obj = new GameObject(text + "_Button", typeof(RectTransform), typeof(Image), typeof(Button));
        obj.transform.SetParent(parent, false);
        var txt = CreateUIText(text, obj.transform, Vector2.zero, new Vector2(20, 10));
        obj.GetComponent<RectTransform>().anchoredPosition = anchoredPos; // [위치값 설정] 버튼 위치
        return obj;
    }

    private GameObject CreateUIText(string text, Transform parent, Vector2 anchoredPos, Vector2 Size)
    {
        GameObject obj = new GameObject("Text", typeof(RectTransform));
        obj.transform.SetParent(parent, false);
        var tmp = obj.AddComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.enableAutoSizing = true; // [폰트 변경 가능] 여기서 폰트 크기 변경 가능
        // tmp.font = (TMP_FontAsset)Resources.Load("폰트경로"); // [폰트 변경 가능] TMP 폰트 지정 가능
        tmp.alignment = TextAlignmentOptions.Center;

        obj.GetComponent<RectTransform>().anchoredPosition = anchoredPos; // [위치값 설정]
        obj.GetComponent<RectTransform>().sizeDelta = Size; 
        return obj;
    }
}
