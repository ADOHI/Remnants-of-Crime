using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public RectTransform background; // ��׶���(�θ�)
    public CharacterData[] characters;

    private GameObject characterGrid;
    private GameObject detailsPage;

    private int currentCharacterIndex = 0;
    private int currentInfoTabIndex = 0;

    private readonly string[] infoTabs = { "�⺻����", "ĳ���� ����", "��� ����", "��ȭ ���" };

    void Start()
    {
        CreateTabs();
    }

    // ====== �� ���� ======
    private void CreateTabs()
    {
        string[] tabNames = { "�ι�", "����ǰ", "����" };
        for (int i = 0; i < tabNames.Length; i++)
        {
            // [��ġ�� ����] �� ��ư ��ġ
            GameObject btnObj = CreateUIButton(tabNames[i], background, new Vector2(100 + (i * 150), -50));
            int index = i;
            btnObj.GetComponent<Button>().onClick.AddListener(() => OnTabClick(index));
        }
    }

    private void OnTabClick(int index)
    {
        if (index == 0) ShowCharacterList();
        // index == 1 �� ����ǰ UI
        // index == 2 �� ���� UI
    }

    // ====== �ι� ��� ǥ�� ======
    private void ShowCharacterList()
    {
        if (characterGrid != null) Destroy(characterGrid);
        if (detailsPage != null) Destroy(detailsPage);

        characterGrid = new GameObject("CharacterGrid", typeof(RectTransform));
        characterGrid.transform.SetParent(background, false);

        var gridLayout = characterGrid.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(100, 120); // [��ġ�� ����] �� ũ��
        gridLayout.spacing = new Vector2(10, 10);    // [��ġ�� ����] �� ����
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 6; // ���� 6��
        characterGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150); // [��ġ�� ����] �׸��� ��ġ

        for (int i = 0; i < characters.Length && i < 12; i++) // �ִ� 6x2 = 12��
        {
            if (characters[i] == null) continue;
            GameObject slot = CreateCharacterSlot(characters[i]);
            slot.transform.SetParent(characterGrid.transform, false);
        }
    }

    private GameObject CreateCharacterSlot(CharacterData data)
    {
        GameObject slot = new GameObject(data.characterName, typeof(RectTransform));
        var vLayout = slot.AddComponent<VerticalLayoutGroup>();
        vLayout.spacing = 5;
        vLayout.childAlignment = TextAnchor.MiddleCenter;

        // �ʻ�ȭ
        GameObject imgObj = new GameObject("Portrait", typeof(RectTransform));
        imgObj.transform.SetParent(slot.transform, false);
        var img = imgObj.AddComponent<Image>();
        img.sprite = data.portrait;
        img.preserveAspect = true;
        imgObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100); // [��ġ�� ����] �ʻ�ȭ ũ��

        // �̸�
        CreateUIText(data.characterName, slot.transform, Vector2.zero, 18);

        // Ŭ�� �� �� ������
        var btn = slot.AddComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            currentCharacterIndex = System.Array.IndexOf(characters, data);
            ShowCharacterDetails();
        });

        return slot;
    }

    // ====== �� ������ ======
    private void ShowCharacterDetails()
    {
        if (detailsPage != null) Destroy(detailsPage);
        if (characterGrid != null) Destroy(characterGrid);

        detailsPage = new GameObject("DetailsPage", typeof(RectTransform));
        detailsPage.transform.SetParent(background, false);

        // ���� ������
        GameObject leftPage = new GameObject("LeftPage", typeof(RectTransform));
        leftPage.transform.SetParent(detailsPage.transform, false);
        leftPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 0); // [��ġ�� ����] ���� ������ ��ġ

        var charImg = leftPage.AddComponent<Image>();
        charImg.sprite = characters[currentCharacterIndex].portrait;
        charImg.preserveAspect = true;

        // �� ȭ��ǥ
        GameObject upBtn = CreateUIButton("��", leftPage.transform, new Vector2(0, 120)); // [��ġ�� ����]
        upBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            currentCharacterIndex = (currentCharacterIndex - 1 + characters.Length) % characters.Length;
            ShowCharacterDetails();
        });

        // �Ʒ� ȭ��ǥ
        GameObject downBtn = CreateUIButton("��", leftPage.transform, new Vector2(0, -120)); // [��ġ�� ����]
        downBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Length;
            ShowCharacterDetails();
        });

        // ������ ������
        GameObject rightPage = new GameObject("RightPage", typeof(RectTransform));
        rightPage.transform.SetParent(detailsPage.transform, false);
        rightPage.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 0); // [��ġ�� ����]

        UpdateRightPage(rightPage);
    }

    private void UpdateRightPage(GameObject rightPage)
    {
        foreach (Transform child in rightPage.transform)
            Destroy(child.gameObject);

        // �� �̸�
        CreateUIText(infoTabs[currentInfoTabIndex], rightPage.transform, new Vector2(0, 100), 24);

        // ����
        string content = "";
        var data = characters[currentCharacterIndex];
        switch (currentInfoTabIndex)
        {
            case 0: // �⺻ ����
                content = data.Discription;
                break;
            case 1: // ĳ���� ����
                foreach (var rel in data.relationships)
                    content += $"- {rel.targetCharacterID}: {rel.relationDescription}\n";
                break;
            case 2: // ��� ����
                foreach (var rel in data.caserelationships)
                    content += $"- {rel.targetCase}: {rel.relationDescription}\n";
                break;
            case 3: // ��ȭ ���
                foreach (var t in data.transcripts)
                    content += $"Q: {t.Question}\nA: {t.Answer}\n\n";
                break;
        }

        CreateUIText(content, rightPage.transform, new Vector2(0, 0), 18);

        // ��/�� ȭ��ǥ ����
        if (currentInfoTabIndex > 0) // ù �������� �ƴ� ���� ���� ȭ��ǥ
        {
            GameObject leftArrow = CreateUIButton("��", rightPage.transform, new Vector2(-150, -150)); // [��ġ�� ����]
            leftArrow.GetComponent<Button>().onClick.AddListener(() =>
            {
                currentInfoTabIndex = Mathf.Max(currentInfoTabIndex - 1, 0);
                UpdateRightPage(rightPage);
            });
        }

        if (currentInfoTabIndex < infoTabs.Length - 1) // ������ �������� �ƴ� ���� ���� ȭ��ǥ
        {
            GameObject rightArrow = CreateUIButton("��", rightPage.transform, new Vector2(150, -150)); // [��ġ�� ����]
            rightArrow.GetComponent<Button>().onClick.AddListener(() =>
            {
                currentInfoTabIndex = Mathf.Min(currentInfoTabIndex + 1, infoTabs.Length - 1);
                UpdateRightPage(rightPage);
            });
        }
    }

    // ====== ���� UI ���� ======
    private GameObject CreateUIButton(string text, Transform parent, Vector2 anchoredPos)
    {
        GameObject obj = new GameObject(text + "_Button", typeof(RectTransform), typeof(Image), typeof(Button));
        obj.transform.SetParent(parent, false);
        var txt = CreateUIText(text, obj.transform, Vector2.zero, 20);
        obj.GetComponent<RectTransform>().anchoredPosition = anchoredPos; // [��ġ�� ����] ��ư ��ġ
        return obj;
    }

    private GameObject CreateUIText(string text, Transform parent, Vector2 anchoredPos, int fontSize)
    {
        GameObject obj = new GameObject("Text", typeof(RectTransform));
        obj.transform.SetParent(parent, false);
        var tmp = obj.AddComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.fontSize = fontSize; // [��Ʈ ���� ����] ���⼭ ��Ʈ ũ�� ���� ����
        // tmp.font = (TMP_FontAsset)Resources.Load("��Ʈ���"); // [��Ʈ ���� ����] TMP ��Ʈ ���� ����
        tmp.alignment = TextAlignmentOptions.Center;

        obj.GetComponent<RectTransform>().anchoredPosition = anchoredPos; // [��ġ�� ����]
        return obj;
    }
}
