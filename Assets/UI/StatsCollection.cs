using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StatsCollection : MonoBehaviour
{
    private RectTransform _rectTransform = null;

    [SerializeField] private GameObject _template = null;
    [SerializeField] RectTransform _decoration = null;

    private float _fullStatHeight = 20.0f;
    private float _collapsedStatsHeight = 10.0f;
    private bool _areStatsCollapsed = false;
    [SerializeField] private float _margin = 7.5f;

    void Awake()
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        _rectTransform = GetComponent<RectTransform>();

        UI_FightChickenStats templateStats = _template.GetComponent<UI_FightChickenStats>();
        _fullStatHeight = templateStats.GetFullHeight();
        _collapsedStatsHeight = templateStats.GetCompactHeight();

        ListChanged();
    }

    void Start()
    {
        ListChanged();
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        foreach(var chicken in GetAllChickens())
            chicken.Died -= Chicken_Died;
    }

    public void Add(Chicken chicken)
    {
        GameObject statsGameObject = Instantiate(_template, _rectTransform);
        RectTransform rectTransform = statsGameObject.GetComponent<RectTransform>();
        UI_FightChickenStats stats = statsGameObject.GetComponent<UI_FightChickenStats>();

        stats.SetCompactmode(_areStatsCollapsed);
        rectTransform.anchoredPosition = new Vector2(0.0f, GetNextY());

        stats.Chicken = chicken;
        chicken.Died += Chicken_Died;
        ListChanged();
    }

    public void Remove(Chicken chicken)
    {
        int childIndex = GetChildIndex(chicken);
        // Assert.IsTrue(childIndex != -1, "chicken not in list");

        if (childIndex == -1)
            return;

        RectTransform statsRect = (RectTransform)_rectTransform.GetChild(childIndex);
        statsRect.SetParent(null);
        Destroy(statsRect.gameObject);
        chicken.Died -= Chicken_Died;

        ListChanged();
    }

    public List<Chicken> GetAllChickens()
    {
        List<Chicken> chickens = new List<Chicken>();

        foreach(RectTransform child in _rectTransform)
        {
            UI_FightChickenStats uiStats = child.gameObject.GetComponent<UI_FightChickenStats>();
            if (!uiStats)
                Debug.LogWarning("all childs should be UI_FightChickenStats");
            else
                chickens.Add(uiStats.Chicken);
        }

        return chickens;
    }

    private void ListChanged()
    {
        if (_areStatsCollapsed)
        {
            if (_rectTransform.childCount <= GetMaxAmntOfExpanded())
                SetCollapseStats(false);
        }
        else
        {
            if (_rectTransform.childCount > GetMaxAmntOfExpanded())
                SetCollapseStats(true);
        }

        RecalculatePositions();

        if (_decoration)
            _decoration.gameObject.SetActive(_rectTransform.childCount > 0);
    }

    private void Chicken_Died(object sender, Chicken.DiedEventArgs e)
    {
        Remove(e.Chicken);
    }

    private int GetChildIndex(Chicken chicken)
    {
        for (int i = 0; i < _rectTransform.childCount; i++)
        {
            UI_FightChickenStats stats = _rectTransform.GetChild(i).GetComponent<UI_FightChickenStats>();
            if (!stats)
                continue;

            if (stats.Chicken == chicken)
                return i;
        }
        return -1;
    }

    private void SetCollapseStats(bool collapse)
    {
        foreach (RectTransform child in _rectTransform)
        {
            UI_FightChickenStats stats = child.GetComponent<UI_FightChickenStats>();
            if (!stats)
                continue;
            stats.SetCompactmode(collapse);
        }
        _areStatsCollapsed = collapse;
    }

    private void RecalculatePositions()
    {
        float height = _areStatsCollapsed
            ? _collapsedStatsHeight
            : _fullStatHeight;
        float y = 0.0f;

        foreach (RectTransform child in _rectTransform)
        {
            child.anchoredPosition = new Vector2(0.0f, y);
            y -= height + _margin;
        }
    }

    private float GetNextY()
    {
        int currentFightingChickens = transform.childCount;
        float currentHeight = _areStatsCollapsed
            ? _collapsedStatsHeight
            : _fullStatHeight;

        float nextY = currentFightingChickens * -(currentHeight + _margin);
        return nextY;
    }

    public float GetHeightOfItems()
    {
        int amnt = transform.childCount;

        float currentHeight = _areStatsCollapsed
            ? _collapsedStatsHeight
            : _fullStatHeight;

        float totalheight = currentHeight * amnt;

        if (amnt > 1)
            totalheight += _margin * (amnt - 1);

        return totalheight;
    }

    public bool HasItems()
    {
        return transform.childCount > 0;
    }

    private int GetMaxAmntOfExpanded()
    {
        return GetMaxAmntOfStats(_rectTransform.rect.height, _fullStatHeight, _margin);
    }

    private int GetMaxAmntOfCollapsed()
    {
        return GetMaxAmntOfStats(_rectTransform.rect.height, _collapsedStatsHeight, _margin);
    }

    private static int GetMaxAmntOfStats(float screenHeight, float statsHeight, float margin)
    {
        return Mathf.FloorToInt(screenHeight / (statsHeight + margin));
    }
}