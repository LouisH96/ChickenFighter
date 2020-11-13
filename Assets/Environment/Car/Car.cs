using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Dirty because lack of time
public class Car : MonoBehaviour
{
    //--- Components ---
    [SerializeField] private ChickenPen _trunk = null;
    [SerializeField] private Text _text = null;
    [SerializeField] private string _moveText = "";
    [SerializeField] private SwitchLocationManager _switchLocationManager = null;

    //--- Stats ---
    [SerializeField] private int _locationIndex = 0;

    //--- Variables ---
    private Farmer _farmerAtDoor = null;
    private bool _actived = false;

    public ChickenPen Trunk { get { return _trunk; } }

    void Start()
    {
        _text.text = "";
    }


    void Update()
    {
        if (Input.GetAxis("Action1") > 0.0f)
        {
            if (_farmerAtDoor != null && !_actived)
            {

                _farmerAtDoor.ChickenGrab.ThrowAll();
                Car to = _switchLocationManager.MoveFrom(_locationIndex);
                to._actived = true;
                _farmerAtDoor = null;
                _actived = true;
            }
        }
        else
        {
            _actived = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _farmerAtDoor = Farmer.GetFromCollider(other);
        _text.text = _moveText;
    }

    private void OnTriggerExit(Collider other)
    {
        _farmerAtDoor = null;
        _text.text = "";
    }
}
