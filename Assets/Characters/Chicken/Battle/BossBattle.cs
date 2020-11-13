using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//dirty because lack of time
public class BossBattle : Battle
{
    public event EventHandler<bool> FightEnded;

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
        bool isBossFightChicken = false;
        if (e == _boss)
        {
            _boss = null;
            isBossFightChicken = true;
        }
        else
        {
            _allies.Remove(e);
            isBossFightChicken = true;
        }

        if(isBossFightChicken && !CanFightBeActive())
        {
            FightEnded?.Invoke(this, _boss == null);
        }

        base._pen_ChickenRemoved(sender, e);
    }

    protected override void _pen_ChickenAdded(object sender, Chicken e)
    {
        base._pen_ChickenAdded(sender, e);
        if (e != _boss
            && !_allies.Contains(e))
        {
            e.Location.Pen.RemoveChicken(e);
            e.Location.ExitPen(e.Location.Pen);
            //e.MoveTo(new Vector3(0.0f, -50.0f, 0.0f), e.transform.rotation);
            Destroy(e);
            Destroy(e.gameObject);
            return;
        }
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