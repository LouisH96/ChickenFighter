using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Battle : MonoBehaviour
{
    public event EventHandler<Chicken> ChickenLeft;

    [SerializeField] protected ChickenPen _pen = null;

    private bool _isFightActive = false;

    void Awake()
    {
        _pen.ChickenAdded += _pen_ChickenAdded;
        _pen.ChickenRemoved += _pen_ChickenRemoved;
    }

    private void Update()
    {
        DoDebugChecks();
    }

    private void DoDebugChecks()
    {
        foreach(var chicken in _pen.Chickens)
        {
            if(_isFightActive)
                Assert.AreEqual(chicken.Fighter.Battle, this, "every chicken in pen should be in the battle");
            else
                Assert.IsNull(chicken.Fighter.Battle, "shouldnt be in a battle");
        }
    }

    protected virtual void _pen_ChickenRemoved(object sender, Chicken e)
    {
        if (_isFightActive)
        {
            e.Fighter.LeaveBattle(this);

            if (!CanFightBeActive())
                EnableFight(false);
        }
        ChickenLeft?.Invoke(this, e);
    }

    private void _pen_ChickenAdded(object sender, Chicken e)
    {
        if(!_isFightActive)
        {
            if(CanFightBeActive())
                EnableFight(true);
        }
        else
        {
            e.Fighter.JoinBattle(this);
        }
    }

    protected virtual void EnableFight(bool enabled)
    {
        Assert.AreNotEqual(_isFightActive, enabled, "fight already in this state");

        if (_isFightActive == enabled)
            return;

        foreach (var fighter in _pen.Chickens)
        {
            if (enabled)
                fighter.Fighter.JoinBattle(this);
            else
                fighter.Fighter.LeaveBattle(this);
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