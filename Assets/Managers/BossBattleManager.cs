using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    [SerializeField] private Chicken _bossTemplate = null;
    private Chicken _boss = null;

    [SerializeField] private ChickenPen _trunk = null;
    [SerializeField] private BossBattle _bossBattle = null;

    [SerializeField] private Transform _bossSpawn = null;
    [SerializeField] private Transform[] _allySpawns = null;

    public void StartBossBattle()
    {
        _boss = Instantiate(_bossTemplate, _bossSpawn.transform.position, _bossSpawn.transform.rotation);
        _bossBattle.Boss = _boss;


        int allyCount = _trunk.Chickens.Count;
        for(int i = 0; i < allyCount; i ++)
        {
            Chicken ally = _trunk.Chickens[0];
            ally.Location.ExitPen(_trunk);
            _bossBattle.Allies.Add(ally);
            ally.Physical.ChangeState(ChickenPhysical.PhysicalState.Physics);
            ally.transform.position = _allySpawns[i].position;
            ally.transform.rotation = _allySpawns[i].rotation;
            //ally.Movement.Agent.enabled = true;

        }
    }
}
