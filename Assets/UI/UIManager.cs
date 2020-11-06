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


    void Start()
    {
       //_stats = gameObject.GetComponents<UI_FightChickenStats>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        List<Chicken> chickens = _battle.GetAllChickens();
        for (int i= 0; i < _stats.Count; i++)
        {
            if(i < chickens.Count && !_battle.IsPaused)
            {
                _stats[i].gameObject.SetActive(true);
                _stats[i].Chicken = chickens[i];
            }
            else
                _stats[i].gameObject.SetActive(false);

        }
    }
}
