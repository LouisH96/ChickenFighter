using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    //--- Components ---
    [SerializeField] private Text _text = null;
    private string _textContent = "";
    [SerializeField] private BossBattleManager _bossBattleManager = null;

    //--- Variables ---
    private Farmer _farmerAtDoor = null;
    private bool _activated = false;

    void Start()
    {
        _textContent = _text.text;
        _text.text = "";
    }

    void Update()
    {
        if(_farmerAtDoor && ! _activated &&
            Input.GetAxis("Action1") > 0.0f)
        {
            _farmerAtDoor.ChickenGrab.ThrowAll();
            _bossBattleManager.StartBossBattle();
            _activated = true;
            Debug.Log("ToCity");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _farmerAtDoor = Farmer.GetFromCollider(other);
        _text.text = _textContent;
    }

    private void OnTriggerExit(Collider other)
    {
        _farmerAtDoor = null;
        _text.text = "";
    }
}
