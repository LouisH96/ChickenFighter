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
    [SerializeField] private UIManager _uiManager = null;

    private Farmer _farmer = null;
    private bool _fightActivated = false;

    public void Start()
    {
        _bossBattle.Boss = _boss;
        _bossBattle.FightEnded += _bossBattle_FightEnded;

        _uiManager.ShowChatTime = 15.0f;
        _uiManager.ChatText = "Duke: Hi buddy, I heard there's alot of money to earn if you can KILL Dave's GOLDEN CHICKEN.\n You should breed the ultimate fighter chicken.";
        _uiManager.ShowChatTime = 4.0f;
    }

    private void _bossBattle_FightEnded(object sender, bool e)
    {
        if (!_fightActivated)
            return;



        if (e)
        {
            _uiManager.ChatText = "Dave: Wow you killed him, congratulations here's a million dollar.\n (You won the game, press 'alt' + 'F4' to close the game)";
            Time.timeScale = 0.0f;
        }
        else
        {
            _boss.Fighter.ResetHealth();
            _uiManager.ChatText = "Dave: Try again buddy";
        }

        _fightActivated = false;
    }

    public void StartBossBattle()
    {
        int amntGrabbed = _farmer.ChickenGrab.AmntGrabbed;

        if (amntGrabbed == 0)
        {
            _uiManager.ChatText = "Dave: GRAB your fighter chicken to start the fight.";
            return;
        }
        if (amntGrabbed != 1)
        {
            _uiManager.ChatText = "Dave: You can only fight with one chicken.";
            return;
        }
        _uiManager.ChatText = "Dave: Here we go.";

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

        if (!_fightActivated)
            _text.text = _startFightText;
    }

    private void OnTriggerExit(Collider other)
    {
        _farmer = null;
        _text.text = "";
    }
}