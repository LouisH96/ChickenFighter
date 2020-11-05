using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    protected AttackZone _attackZone = null;

    void Awake()
    {
        if (_attackZone) _attackZone = GetComponent<AttackZone>();
        if (_attackZone) _attackZone = GetComponentInChildren<AttackZone>();
    }

    void Update()
    {
        if (_attackZone.Enemy != null)
            Attack();
        
    }

    public virtual void Attack()
    {

    }
}
