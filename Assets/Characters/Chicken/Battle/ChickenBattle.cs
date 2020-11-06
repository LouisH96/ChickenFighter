using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBattle : MonoBehaviour
{
    [SerializeField] private Chicken _chickenA = null;
    [SerializeField] private Chicken _chickenB = null;


    // Start is called before the first frame update
    void Start()
    {
        StartBattle(_chickenA, _chickenB);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartBattle(Chicken chickenA, Chicken chickenB)
    {
        if (chickenA == chickenB)
            return;
        if (chickenA == null || chickenB == null)
            return;

        if (_chickenA != null || _chickenB != null)
            EndBattle();

        _chickenA = chickenA;
        _chickenB = chickenB;
        _chickenA.StartBattle(this);
        _chickenB.StartBattle(this);
    }

    public void EndBattle()
    {
        if(_chickenA)
        {
            Chicken a = _chickenA;
            _chickenA = null;
            a.EndBattle();
        }

        if(_chickenB)
        {
            Chicken b = _chickenB;
            _chickenB = null;
            b.EndBattle();
        }
    }

    public bool IsEnemy(Chicken ally, Chicken possibleEnemy)
    {
        if (_chickenA == ally && _chickenB == possibleEnemy)
            return true;
        else
        if (_chickenB == ally && _chickenA == possibleEnemy)
            return true;
        else return false;
    }

    public Chicken GetEnemy(Chicken chicken)
    {
        if (_chickenA == chicken)
            return _chickenB;
        else
        if (_chickenB == chicken)
            return _chickenA;
        else
            return null;
    }
}
