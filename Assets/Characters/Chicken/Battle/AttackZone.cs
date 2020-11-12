using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    //---Components---
    [SerializeField] private Chicken _chicken = null;
    private Collider _collider = null;

    private List<FightBodyPart> _targets = new List<FightBodyPart>();

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    void Start()
    {
        _chicken.Fighter.BattleLeft += ChickenFight_BattleLeft;
    }

    void Update()
    {
        _targets.RemoveAll(p => p == null);
        if (_targets.Count == 0)
            _chicken.IsAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        FightBodyPart bodyPart = other.gameObject.GetComponent<FightBodyPart>();

        if (!bodyPart)
            return;

        Battle battle = _chicken.Fighter.Battle;

        if(battle)
        {
            if(battle.IsEnemy(_chicken, bodyPart.Chicken))
            {
                _targets.Add(bodyPart);
                _chicken.IsAttacking = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FightBodyPart bodyPart = other.gameObject.GetComponent<FightBodyPart>();

        if (!bodyPart)
            return;

        if (_targets.Contains(bodyPart))
        {
            _targets.Remove(bodyPart);

            if (_targets.Count == 0)
                _chicken.IsAttacking = true;
        }
    }

    private void ChickenFight_BattleLeft(object sender, Battle e)
    {
        _targets.Clear();
        _chicken.IsAttacking = false;
    }

    public void RemoveFromTargets(Chicken chicken)
    {
        _targets.RemoveAll(bodyPart => bodyPart.Chicken == chicken);
        if (_targets.Count == 0)
            _chicken.IsAttacking = false;
    }
}