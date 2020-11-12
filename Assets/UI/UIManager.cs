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

    [SerializeField] private RectTransform _highlightedChickenDecoration = null;
    [SerializeField] private UI_FightChickenStats _highlightedChicken = null;

    private string _grabbedTextFormat;
    [SerializeField] private Text _grabbedText = null;
    [SerializeField] private StatsCollection _grabbedList = null;

    private void Start()
    {
        _battlePen.ChickenAdded += _battlePen_ChickenAdded;
        _battlePen.ChickenRemoved += _battlePen_ChickenRemoved;

        _chickenGrab.ChickenHighlighted += _chickenGrab_ChickenHighlighted;
        _chickenGrab.ChickenUnHighlighted += _chickenGrab_ChickenUnHighlighted;
        _highlightedChicken.gameObject.SetActive(false);
        _highlightedChickenDecoration.gameObject.SetActive(false);

        _chickenGrab.ChickenGrabbed += _chickenGrab_ChickenGrabbed;
        _chickenGrab.ChickenThrown += _chickenGrab_ChickenThrown;

        _grabbedTextFormat = _grabbedText.text;
        UpdateGrabbedText();
    }

    private void _chickenGrab_ChickenThrown(object sender, Chicken e)
    {
        _grabbedList.Remove(e);
        UpdateGrabbedText();
    }

    private void _chickenGrab_ChickenGrabbed(object sender, Chicken e)
    {
        _grabbedList.Add(e);
        UpdateGrabbedText();
    }

    private void UpdateGrabbedText()
    {
        int max = _chickenGrab.MaxGrabbed;
        int current = _chickenGrab.AmntGrabbed;
        _grabbedText.text = string.Format(_grabbedTextFormat, current, max);
    }

    private void _chickenGrab_ChickenUnHighlighted(object sender, Chicken e)
    {
        _highlightedChickenDecoration.gameObject.SetActive(false);
        _highlightedChicken.gameObject.SetActive(false);
        _highlightedChicken.UnsetChicken();
    }

    private void _chickenGrab_ChickenHighlighted(object sender, Chicken e)
    {
        _highlightedChicken.Chicken = e;
        _highlightedChicken.gameObject.SetActive(true);
        _highlightedChickenDecoration.gameObject.SetActive(true);
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
    }
}
