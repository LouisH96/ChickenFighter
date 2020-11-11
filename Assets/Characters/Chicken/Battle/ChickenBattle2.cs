using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenBattle2 : MonoBehaviour
{
    public event EventHandler<Chicken> ChickenLeft;

    [SerializeField] protected ChickenPen _pen = null;

    private bool _isFightActive = false;

    void Awake()
    {
        _pen.ChickenAdded += _pen_ChickenAdded;
        _pen.ChickenRemoved += _pen_ChickenRemoved;
    }

    private void _pen_ChickenRemoved(object sender, Chicken e)
    {
        if (_isFightActive)
        {
            e.ChickenFight.LeaveBattle(this);

            if (!CanFightBeActive())
                EnableFight(false);
        }
        ChickenLeft?.Invoke(this, e);
    }

    private void _pen_ChickenAdded(object sender, Chicken e)
    {
        if (!_isFightActive && CanFightBeActive())
            EnableFight(true);
    }

    protected virtual void EnableFight(bool enabled)
    {
        Assert.AreNotEqual(_isFightActive, enabled, "fight already in this state");

        if (_isFightActive == enabled)
            return;

        foreach (var fighter in _pen.Chickens)
        {
            if (enabled)
                fighter.ChickenFight.JoinBattle(this);
            else
                fighter.ChickenFight.LeaveBattle(this);
        }

        _isFightActive = enabled;
    }

    protected virtual bool CanFightBeActive()
    {
        return _pen.Chickens.Count > 1;
    }

    public virtual Chicken GetEnemy(Chicken chicken)
    {
        return GetClosestEnemy(chicken, _pen.Chickens);
    }

    public virtual bool IsEnemy(Chicken me, Chicken possibleEnemy)
    {
        Assert.IsTrue(_pen.Chickens.Contains(me), "'me' is not in chickenpen");
        Assert.IsTrue(_pen.Chickens.Contains(possibleEnemy), "possibleEnemy is not in chickenpen");

        return me != possibleEnemy;
    }

    protected static Chicken GetClosestEnemy(Chicken chicken, ICollection<Chicken> enemies)
    {
        Chicken closestChicken = null;
        float closestSqrDistance = 0.0f;

        foreach (Chicken enemy in enemies)
        {
            if (enemy == chicken)
                continue;

            float enemySqrDistance = (enemy.transform.position - chicken.transform.position).sqrMagnitude;
            if (closestChicken == null || enemySqrDistance < closestSqrDistance)
            {
                closestChicken = enemy;
                closestSqrDistance = enemySqrDistance;
            }
        }
        return closestChicken;
    }
}