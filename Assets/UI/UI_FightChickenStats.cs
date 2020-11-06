using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FightChickenStats : MonoBehaviour
{
    [SerializeField] private Chicken _chicken = null;
    [SerializeField] private Image _hp = null;
    [SerializeField] private Text _hpText = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (_chickenA)
        //    _hpA.transform.localScale = new Vector3(_chickenA.CurrentHealthRatio, 1, 1);
        //else
        //    _hpA.transform.localScale = new Vector3(0.0f, 1, 1);

        //if (_chickenB)
        //    _hpB.transform.localScale = new Vector3(_chickenB.CurrentHealthRatio, 1, 1);
        //else
        //    _hpB.transform.localScale = new Vector3(0.0f, 1, 1);

        //if (_hpTextA)
        //    _hpTextA.text = _chickenA.CurrentHealth + "/" + _chickenA.MaxHealth;
        //else
        //    _hpTextA.text = "0";

        //if (_hpTextB)
        //    _hpTextB.text = _chickenB.CurrentHealth + "/" + _chickenB.MaxHealth;
        //else
        //    _hpTextB.text = "0";
    }
}
