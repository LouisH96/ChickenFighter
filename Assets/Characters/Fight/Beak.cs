using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Beak : MonoBehaviour
{
    private static List<string> _teamTags = new List<string> { "TeamA", "TeamB" };
    private string _enemyTag;

    private FightChicken _fightChicken = null;

    void Start()
    {
        _enemyTag = _teamTags.FirstOrDefault(t => !CompareTag(t));
        _fightChicken = GetComponentInParent<FightChicken>();
    }

    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_enemyTag))
        {
            if (_fightChicken
                && _fightChicken.Enemy)
                _fightChicken.Enemy.TakeDamage(1);
        }
    }
}
