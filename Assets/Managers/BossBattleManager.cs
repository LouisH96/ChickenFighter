using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//dirty because lack of time
public class BossBattleManager : MonoBehaviour
{
    [SerializeField] private Chicken _boss = null;
    [SerializeField] private BossBattle _bossBattle = null;
    [SerializeField] private Transform[] _allySpawns = null;
    [SerializeField] private Text _text = null;
    [SerializeField] private string _startFightText = "'LMB' to start the fight";

    private Farmer _farmer = null;
    private bool _fightActivated = false;

    public void Start()
    {
        _bossBattle.Boss = _boss;
        _bossBattle.FightEnded += _bossBattle_FightEnded;
    }

    private void _bossBattle_FightEnded(object sender, bool e)
    {
        if (!_fightActivated)
            return;

        if (e)
        {
            Debug.Log("won");
        }
        else
        {
            _boss.Fighter.ResetHealth();
            Debug.Log("Lost");
        }

        _fightActivated = false;
    }

    public void StartBossBattle()
    {
        int amntGrabbed = _farmer.ChickenGrab.AmntGrabbed;

        if(amntGrabbed != 1)
        {
            Debug.Log("only 1");
            return;
        }

        _bossBattle.Allies.Clear();
        Chicken grabbed = _farmer.ChickenGrab.GetPickedUpChickens()[0];
        _farmer.ChickenGrab.ThrowAll();
        _bossBattle.Allies.Add(grabbed);
        grabbed.Physical.CanPickup = false;
        grabbed.MoveTo(_allySpawns[0].position, _allySpawns[0].rotation);

        _fightActivated = true;
    }

    private void Update()
    {
        if (!_fightActivated && Input.GetAxis("Action1") > 0.0f && _farmer)
        {
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