using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ChickenGrab _chickenGrab= null;
    [SerializeField] private ChickenPen _battlePen = null;

    [SerializeField] private StatsCollection _fightingList = null;
    [SerializeField] private RectTransform _fightingListDecorations = null;

    [SerializeField] private UI_FightChickenStats _highlightingChicken = null;

    private void Start()
    {
        _battlePen.ChickenAdded += _battlePen_ChickenAdded;
        _battlePen.ChickenRemoved += _battlePen_ChickenRemoved;

        _chickenGrab.ChickenHighlighted += _chickenGrab_ChickenHighlighted;
        _chickenGrab.ChickenUnHighlighted += _chickenGrab_ChickenUnHighlighted;
        _highlightingChicken.gameObject.SetActive(false);
    }

    private void _chickenGrab_ChickenUnHighlighted(object sender, Chicken e)
    {
        _highlightingChicken.gameObject.SetActive(false);
        _highlightingChicken.UnsetChicken();
    }

    private void _chickenGrab_ChickenHighlighted(object sender, Chicken e)
    {
        _highlightingChicken.Chicken = e;
        _highlightingChicken.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _battlePen.ChickenAdded -= _battlePen_ChickenAdded;
        _battlePen.ChickenRemoved -= _battlePen_ChickenRemoved;
        _chickenGrab.ChickenHighlighted -= _chickenGrab_ChickenHighlighted;
        _chickenGrab.ChickenUnHighlighted -= _chickenGrab_ChickenUnHighlighted;
    }

    private void _battlePen_ChickenRemoved(object sender, Chicken e)
    {
        _fightingList.Remove(e);
    }

    private void _battlePen_ChickenAdded(object sender, Chicken e)
    {
        _fightingList.Add(e);
    }

    private void Update()
    {
        _fightingListDecorations.gameObject.SetActive(_fightingList.HasItems());
    }
}
