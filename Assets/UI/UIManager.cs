using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private FightChicken _chickenA = null;
    [SerializeField] private FightChicken _chickenB = null;

    [SerializeField] private Image _hpA = null;
    [SerializeField] private Image _hpB = null;

    [SerializeField] private Text _hpTextA = null;
    [SerializeField] private Text _hpTextB = null;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_chickenA)
            _hpA.transform.localScale = new Vector3(_chickenA.HealthRatio, 1, 1);
        else
            _hpA.transform.localScale = new Vector3(0.0f, 1, 1);

        if (_chickenB)
            _hpB.transform.localScale = new Vector3(_chickenB.HealthRatio, 1, 1);
        else
            _hpB.transform.localScale = new Vector3(0.0f, 1, 1);

        if (_hpTextA)
            _hpTextA.text = _chickenA.CurrentHealth + "/" + _chickenA.MaxHealth;
        else
            _hpTextA.text = "0";

        if (_hpTextB)
            _hpTextB.text = _chickenB.CurrentHealth + "/" + _chickenB.MaxHealth;
        else
            _hpTextB.text = "0";
    }
}
