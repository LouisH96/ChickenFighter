using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BossChickenBattle : ChickenBattle2
{
    [SerializeField] private List<Chicken> _enemies = new List<Chicken>();
    private List<Chicken> _allies = new List<Chicken>();

    private void Awake()
    {
        _pen.ChickenAdded += _pen_ChickenAdded;
        _pen.ChickenRemoved += _pen_ChickenRemoved;
    }

    private void _pen_ChickenRemoved(object sender, Chicken e)
    {
        if (_enemies.Contains(e))
            _enemies.Remove(e);
        else
        if (_allies.Contains(e))
            _allies.Remove(e);
        else
            Assert.IsTrue(true, "chicken is not an enemy or ally");
    }

    private void _pen_ChickenAdded(object sender, Chicken e)
    {
        if (!_enemies.Contains(e))
        {
            _allies.Add(e);
        }
    }

    public override Chicken GetEnemy(Chicken chicken)
    {
        if (_enemies.Contains(chicken))
            return GetClosestEnemy(chicken, _allies);
        else
        if (_allies.Contains(chicken))
            return GetClosestEnemy(chicken, _enemies);

        Assert.IsTrue(true, "chicken is not an enemy or ally");
        return null;
    }

    public override bool IsEnemy(Chicken me, Chicken possibleEnemy)
    {
        ICollection<Chicken> meTeam;
        ICollection<Chicken> otherTeam;

        if (_allies.Contains(me))
        {
            meTeam = _allies;
            otherTeam = _enemies;
        }
        else
        {
            Assert.IsTrue(_enemies.Contains(me), "'me' is in neither team");
            meTeam = _enemies;
            otherTeam = _allies;
        }

        if(otherTeam.Contains(possibleEnemy))
        {
            return true;
        }
        else
        {
            Assert.IsTrue(meTeam.Contains(possibleEnemy), "possibleEnemy is in neither team");
            return false;
        }
    }
}