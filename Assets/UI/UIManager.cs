using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ChickenPen _battlePen = null;

    [SerializeField] private StatsCollection _fightingList = null;
    [SerializeField] private RectTransform _fightingListDecorations = null;

    private void Start()
    {
        _battlePen.ChickenAdded += _battlePen_ChickenAdded;
        _battlePen.ChickenRemoved += _battlePen_ChickenRemoved;
    }

    private void OnDestroy()
    {
        _battlePen.ChickenAdded -= _battlePen_ChickenAdded;
        _battlePen.ChickenRemoved -= _battlePen_ChickenRemoved;
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
