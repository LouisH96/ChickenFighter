using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ChickenBattle _battle = null;
    [SerializeField] private List<UI_FightChickenStats> _stats;

    [SerializeField] private UI_FightChickenStats _highlightedStats;
    [SerializeField] private List<UI_FightChickenStats> _pickedUpStats;
    [SerializeField] private ChickenGrab _chickenGrab = null;

    void Start()
    {
        //_stats = gameObject.GetComponents<UI_FightChickenStats>().ToList();

    }

    // Update is called once per frame
    void Update()
    {
        List<Chicken> chickens = _battle.GetAllChickens();
        for (int i = 0; i < _stats.Count; i++)
        {
            if (i < chickens.Count && !_battle.IsPaused)
            {
                _stats[i].gameObject.SetActive(true);
                _stats[i].Chicken = chickens[i];
            }
            else
                _stats[i].gameObject.SetActive(false);
        }

        List<Chicken> pickedUpChicks = _chickenGrab.GetPickedUpChickens();
        for (int i = 0; i < _pickedUpStats.Count; i++)
        {
            if (i < pickedUpChicks.Count)
            {
                _pickedUpStats[i].gameObject.SetActive(true);
                _pickedUpStats[i].Chicken = pickedUpChicks[i];
            }
            else
            {
                _pickedUpStats[i].gameObject.SetActive(false);
            }
        }

        _highlightedStats.gameObject.SetActive(_chickenGrab.HighlightedChicken != null);
        _highlightedStats.Chicken = _chickenGrab.HighlightedChicken;
    }
}
