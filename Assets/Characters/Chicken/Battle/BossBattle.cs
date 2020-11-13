using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : Battle
{
    //--- Components ---

    //--- Variables ---
    private List<Chicken> _allies = new List<Chicken>();
    private Chicken _boss = null;

    //--- Public Member Access ---
    public List<Chicken> Allies { get { return _allies; } set { _allies = value; } }
    public Chicken Boss { get { return _boss; } set { _boss = value; } }

    void Start()
    {
    }

    protected override void _pen_ChickenRemoved(object sender, Chicken e)
    {
        if (e == _boss)
            _boss = null;
        else
            _allies.Remove(e);
        base._pen_ChickenRemoved(sender, e);
    }

    void Update()
    {
        
    }

    protected override bool CanFightBeActive()
    {
        return _boss != null && _allies.Count > 0;
    }

    public override Chicken GetEnemy(Chicken chicken)
    {
        if(chicken == _boss)
            return GetClosestEnemy(chicken, _allies);
        else
            return _boss;
    }

    public override bool IsEnemy(Chicken me, Chicken possibleEnemy)
    {
        if (me == possibleEnemy)
            return false;
        else if (me == _boss)
            return true;
        else if (possibleEnemy == _boss)
            return true;
        else
            return false;
    }
}