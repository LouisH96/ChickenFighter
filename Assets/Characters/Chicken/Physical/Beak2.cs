using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beak2 : MonoBehaviour
{
    //---Components---
    [SerializeField] private Chicken _chicken = null;

    void OnCollisionEnter(Collision collision)
    {
        FightBodyPart bodyPart = collision.gameObject.GetComponent<FightBodyPart>();

        if (!bodyPart)
            return;

        ChickenBattle2 myBattle = _chicken.ChickenFight.Battle;
        ChickenBattle2 hitChickensBattle = bodyPart.Chicken.ChickenFight.Battle;

        if (myBattle != hitChickensBattle
            || myBattle == null || hitChickensBattle == null)
            return;

        if (myBattle.IsEnemy(_chicken, bodyPart.Chicken))
        {
            //if(bodyPart.Chicken.CurrentHealth <= _chicken.Stats.Damage)
            //{
            //    _chicken.Stats.UpgradeRandomStat();
            //}

            bodyPart.HitBodyPart(_chicken.Stats.Damage, _chicken);
        }
    }



    //private Chicken _attackingChicken = null;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //void OnCollisionEnter(Collision collision)
    //{
    //    FightBodyPart bodyPart = collision.gameObject.GetComponent<FightBodyPart>();

    //    if (!bodyPart)
    //        return;

    //    if(_chicken.IsEnemy(bodyPart.Chicken))
    //    {
    //        _attackingChicken = bodyPart.Chicken;
    //        _chicken.IsAttacking = true;
    //    }


    //    //if (collision.gameObject.CompareTag(_enemyTag))
    //    //{
    //    //    if (_fightChicken
    //    //        && _fightChicken.Enemy)
    //    //        _fightChicken.Enemy.TakeDamage(1);
    //    //}
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    FightBodyPart bodyPart = collision.gameObject.GetComponent<FightBodyPart>();

    //    if(_attackingChicken == bodyPart.Chicken)
    //    {
    //        _chicken.IsAttacking = false;
    //        _attackingChicken = null;
    //    }
    //}
}
