using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleManager : MonoBehaviour
{
    [SerializeField] private Chicken _bossTemplate = null;
    private Chicken _boss = null;

    [SerializeField] private ChickenPen _allyWaitingPen = null;
    [SerializeField] private BossBattle _bossBattle = null;

    [SerializeField] private Transform _bossSpawn = null;
    [SerializeField] private Transform[] _allySpawns = null;

    private Farmer _farmer = null;
    [SerializeField] private Text _text = null;
    [SerializeField] private string _startFightText = "'LMB' to start the fight";
    [SerializeField] private Transform _previewBossSpawn = null;
    private Chicken _previewBoss = null;

    private bool _activated = false;

    public void Start()
    {
        _previewBoss = Instantiate(_bossTemplate, _previewBossSpawn);
        _previewBoss.Location.ExitPen(_previewBoss.Location.Pen);
        _previewBoss.gameObject.SetActive(false);
    }

    public void StartBossBattle()
    {
        _previewBoss.gameObject.SetActive(true);

        _boss = Instantiate(_bossTemplate, _bossSpawn.transform.position, _bossSpawn.transform.rotation);
        _bossBattle.Boss = _boss;

        int allyCount = _allyWaitingPen.Chickens.Count;
        for (int i = 0; i < allyCount; i++)
        {
            Chicken ally = _allyWaitingPen.Chickens[0];
            _bossBattle.Allies.Add(ally);

            ally.MoveTo(_allySpawns[i].position, _allySpawns[i].rotation);
        }
    }

    private void Update()
    {
        if (!_activated && Input.GetAxis("Action1") > 0.0f && _farmer)
        {
            _activated = true;
            StartBossBattle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _farmer = Farmer.GetFromCollider(other);
        _text.text = _startFightText;
    }

    private void OnTriggerExit(Collider other)
    {
        _farmer = null;
        _text.text = "";
    }
}