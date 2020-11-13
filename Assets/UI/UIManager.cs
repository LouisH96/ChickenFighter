using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ChickenGrab _chickenGrab = null;

    private ChickenPen _displayedPen = null;
    [SerializeField] private StatsCollection _displayedPenList = null;
    [SerializeField] private Text _displayedPenText = null;

    [SerializeField] private RectTransform _highlightedChickenDecoration = null;
    [SerializeField] private UI_FightChickenStats _highlightedChicken = null;

    private string _grabbedTextFormat;
    [SerializeField] private Text _grabbedText = null;
    [SerializeField] private StatsCollection _grabbedList = null;

    [SerializeField] private Transform _chat = null;
    [SerializeField] private Text _chatText = null;
    [SerializeField] private float _showChatTime = 4.0f;

    [SerializeField] private Text _topText = null;
    [SerializeField] private bool _displayPenCount = false;

    //--- Public Member Access ---
    public ChickenPen DisplayedPen { get { return _displayedPen; } }

    public string TopText { get { return _topText.text; } set { _topText.text = value; } }

    public float ShowChatTime { get { return _showChatTime; } set { _showChatTime = value; } }

    public string ChatText
    {
        get { return _chatText.text; }
        set
        {
            _chatText.text = value;
            if (value == "")
            {
                _chat.gameObject.SetActive(false);
            }
            else
            {
                _chat.gameObject.SetActive(true);
                CancelInvoke(nameof(RemoveChatScreen));
                Invoke(nameof(RemoveChatScreen), _showChatTime);
            }
        }
    }

    private void Start()
    {
        _chickenGrab.ChickenHighlighted += _chickenGrab_ChickenHighlighted;
        _chickenGrab.ChickenUnHighlighted += _chickenGrab_ChickenUnHighlighted;
        _highlightedChicken.gameObject.SetActive(false);
        _highlightedChickenDecoration.gameObject.SetActive(false);

        _chickenGrab.ChickenGrabbed += _chickenGrab_ChickenGrabbed;
        _chickenGrab.ChickenThrown += _chickenGrab_ChickenThrown;

        _grabbedTextFormat = _grabbedText.text;
        UpdateGrabbedText();

        if (_chatText.text == "")
            _chat.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_displayedPen && _displayPenCount)
        {
            _displayedPenText.text = _displayedPen.name + " " + _displayedPen.Chickens.Count + "/" + _displayedPen.MaxAmntOfChickens;
        }
    }

    private void RemoveChatScreen()
    {
        ChatText = "";
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
        UndisplayPen();
        _chickenGrab.ChickenHighlighted -= _chickenGrab_ChickenHighlighted;
        _chickenGrab.ChickenUnHighlighted -= _chickenGrab_ChickenUnHighlighted;
    }

    public void DisplayPen(ChickenPen pen)
    {
        if (_displayedPen)
            UndisplayPen();

        pen.ChickenAdded += Pen_ChickenAdded;
        pen.ChickenRemoved += Pen_ChickenRemoved;

        _displayedPen = pen;
        _displayedPenText.text = pen.name;

        foreach (var chicken in pen.Chickens)
            _displayedPenList.Add(chicken);
    }

    public void UndisplayPen()
    {
        if (_displayedPen)
        {
            _displayedPenList.RemoveAll();
            _displayedPenText.text = "";
            _displayedPen.ChickenAdded -= Pen_ChickenAdded;
            _displayedPen.ChickenRemoved -= Pen_ChickenRemoved;
            _displayedPen = null;
        }
    }

    private void Pen_ChickenAdded(object sender, Chicken e)
    {
        _displayedPenList.Add(e);
    }

    private void Pen_ChickenRemoved(object sender, Chicken e)
    {
        _displayedPenList.Remove(e);
    }
}
