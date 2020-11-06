using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FightChickenStats : MonoBehaviour
{
    [SerializeField] private Image _hp = null;
    [SerializeField] private Text _hptext = null;

    [SerializeField] private Text _damage = null;
    [SerializeField] private Text _hpRegen = null;
    [SerializeField] private Text _speed = null;
    [SerializeField] private Text _acceleration = null;

    private Chicken _chicken = null;
    private float _oldMaxHealth;
    private float _oldDamage;
    private float _oldHpRegen;
    private float _oldSpeed;
    private float _oldAcceleration;

    [SerializeField] private Image _statsBg = null;

    public Chicken Chicken
    {
        get
        {
            return _chicken;
        }
        set
        {
            if(value != _chicken
                && value != null)
            {
                _chicken = value;
                _oldMaxHealth = _chicken.Stats.Health;
                _oldDamage = _chicken.Stats.Damage;
                _oldHpRegen = _chicken.Stats.HealthRegen;
                _oldSpeed = _chicken.Stats.MaxSpeed;
                _oldAcceleration = _chicken.Stats.Acceleration;

                _hptext.color = _defaultHPTextColor;
                _damage.color = _defaultTextColor;
                _hpRegen.color = _defaultTextColor;
                _speed.color = _defaultTextColor;
                _acceleration.color = _defaultTextColor;

                _hptext.fontStyle = FontStyle.Normal;
                _damage.fontStyle = FontStyle.Normal;
                _hpRegen.fontStyle = FontStyle.Normal;
                _speed.fontStyle = FontStyle.Normal;
                _acceleration.fontStyle = FontStyle.Normal;
            }
        }
    }

    private static Color _defaultTextColor = new Color32(50,50,50, 255);
    private static Color _defaultHPTextColor = new Color32(0,0,0, 255);

    [SerializeField] private Color _defaultBgColor = new Color32(255,255,255, 255);
    [SerializeField] private Color _highlightedBgColor = new Color32(205,205,0, 255);

    [SerializeField] private Color _upgradedStatColor = new Color32(255, 255, 0, 255);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Chicken)
        {
            if (Chicken.Stats.Health != _oldMaxHealth)
            {
                _hptext.color = _upgradedStatColor;
                _hptext.fontStyle = FontStyle.Bold;
                _oldMaxHealth = Chicken.Stats.Health;
            }

            if (Chicken.Stats.Damage != _oldDamage)
            {
                _damage.color = _upgradedStatColor;
                _damage.fontStyle = FontStyle.Bold;
                _oldDamage = Chicken.Stats.Damage;
            }

            if (Chicken.Stats.HealthRegen != _oldHpRegen)
            {
                _hpRegen.color = _upgradedStatColor;
                _hpRegen.fontStyle = FontStyle.Bold;
                _oldHpRegen = Chicken.Stats.HealthRegen;
            }

            if (Chicken.Stats.MaxSpeed != _oldSpeed)
            {
                _speed.color = _upgradedStatColor;
                _speed.fontStyle = FontStyle.Bold;
                _oldSpeed = Chicken.Stats.MaxSpeed;
            }

            if (Chicken.Stats.Acceleration != _oldAcceleration)
            {
                _acceleration.color = _upgradedStatColor;
                _acceleration.fontStyle = FontStyle.Bold;
                _oldAcceleration = Chicken.Stats.Acceleration;
            }

            _damage.text = "Damage:" + _oldDamage;
            _hpRegen.text = "HP Regen:" + _oldHpRegen;
            _speed.text = "Speed:" + _oldSpeed;
            _acceleration.text = "Acceleration:" + _oldAcceleration;

            _hptext.text = Chicken.name + ": " + String.Format("{0:0.0}", Chicken.CurrentHealth) + "/" + Chicken.MaxHealth;
            _hp.transform.localScale = new Vector3(Chicken.CurrentHealthRatio, 1, 1);

            if (Chicken.isHighLighted)
                _statsBg.color = _highlightedBgColor;
            else
                _statsBg.color = _defaultBgColor;

        }
        else
        {
            _hptext.text = "empty";
            _hp.transform.localScale = new Vector3(0.0f, 1, 1);
        }
    }
}
