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
            _chickenA.EndBattle();
            _chickenA = null;
        }

        if(_chickenB)
        {
            _chickenB.EndBattle();
            _chickenB = null;
        }
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
