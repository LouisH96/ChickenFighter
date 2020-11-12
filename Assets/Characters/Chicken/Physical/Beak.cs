using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FightBodyPart;

public class Beak : MonoBehaviour
{
    public event EventHandler<HitEventArgs> LandedHit;

    //---Components---
    [SerializeField] private Chicken _chicken = null;

    void OnCollisionEnter(Collision collision)
    {
        FightBodyPart bodyPart = collision.gameObject.GetComponent<FightBodyPart>();

        if (!bodyPart)
            return;

        Battle myBattle = _chicken.ChickenFight.Battle;
        Battle hitChickensBattle = bodyPart.Chicken.ChickenFight.Battle;

        if (myBattle != hitChickensBattle
            || myBattle == null || hitChickensBattle == null)
            return;

        if (myBattle.IsEnemy(_chicken, bodyPart.Chicken))
        {
            var hitArgs = bodyPart.HitBodyPart(_chicken.Stats.Damage, _chicken);
            LandedHit?.Invoke(this, hitArgs);
        }
    }
}
