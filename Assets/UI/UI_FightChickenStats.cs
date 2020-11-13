using Assets.Characters.Chicken.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UI_FightChickenStats : MonoBehaviour
{
    [SerializeField] private Image _greenHealthBar = null;
    [SerializeField] private Text _healthBartext = null;

    [SerializeField] private RectTransform _statsTransform = null;
    [SerializeField] private Image _statsBackground = null;

    [SerializeField] private Text _hpRegenText = null;
    [SerializeField] private Text _hpRegenValue = null;
    [SerializeField] private Text _damageText = null;
    [SerializeField] private Text _damageValue = null;
    [SerializeField] private Text _speedText = null;
    [SerializeField] private Text _speedValue = null;
    [SerializeField] private Text _accelerationText = null;
    [SerializeField] private Text _accelerationValue = null;

    private Chicken _chicken = null;

    public Chicken Chicken
    {
        get { return _chicken; }
        set { SetChicken(value); }
    }

    private Color _defaultStatColor = new Color32(50, 50, 50, 255);
    private Color _defaultHealthTextColor = Color.white;
    [SerializeField] private Color _highlightStatColor = new Color32(255, 255, 0, 255);

    private Color _defaultBgColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color _highlightedBgColor = new Color32(205, 205, 0, 255);

    [SerializeField] private float _highlightDuration = 2.0f;

    void Start()
    {
        _defaultHealthTextColor = _healthBartext.color;
        _defaultStatColor = _damageText.color;
        _defaultBgColor = _statsBackground.color;

        SetGreenHealthBarRatio(1.0f);
    }

    void Update()
    {
        if (!_chicken)
            return;

        UpdateHealthBar();
    }

    private void OnDestroy()
    {
        if (_chicken)
            UnsetChicken();
    }



    private void UpdateHealthBar()
    {
        float maxHealth = _chicken.Stats.Health;
        float currentHealth = _chicken.Fighter.CurrentHealth;

        _healthBartext.text = _chicken.name + ": " + String.Format("{0:0.#}", currentHealth) + "/" + String.Format("{0:0.#}", maxHealth);
        SetGreenHealthBarRatio(currentHealth / maxHealth);
    }

    public void SetCompactmode(bool enable)
    {
        _statsTransform.gameObject.SetActive(!enable);
    }

    public float GetCurrentHeight()
    {
        if (_statsTransform.gameObject.activeSelf)
            return GetFullHeight();
        else
            return GetCompactHeight();
    }

    public float GetFullHeight()
    {
        return GetComponent<RectTransform>().rect.height;
    }

    public float GetCompactHeight()
    {
        return _greenHealthBar.rectTransform.rect.height;
    }

    private void SetGreenHealthBarRatio(float ratio)
    {
        _greenHealthBar.transform.localScale = new Vector3(ratio, 1.0f, 1.0f);
    }

    private void SetChicken(Chicken chicken)
    {
        if (_chicken)
            UnsetChicken();

        _chicken = chicken;
        _chicken.Stats.StatUpgraded += Stats_StatUpgraded;
        _chicken.Physical.Highlighted += Physical_Highlighted; ;
        _chicken.Physical.UnHighlighted += Physical_UnHighlighted;

        _damageValue.text = String.Format("{0:0.#}", _chicken.Stats.Damage);
        _hpRegenValue.text = String.Format("{0:0.#}", _chicken.Stats.HealthRegen);
        _speedValue.text = String.Format("{0:0.#}", _chicken.Stats.Speed);
        _accelerationValue.text = String.Format("{0:0.#}", _chicken.Stats.Acceleration);
        UpdateHealthBar();

        if (_chicken.Physical.IsHighLighted)
            _statsBackground.color = _highlightedBgColor;
    }

    private void Physical_Highlighted(object sender, Farmer e)
    {
        _statsBackground.color = _highlightedBgColor;
    }

    private void Physical_UnHighlighted(object sender, Farmer e)
    {
        _statsBackground.color = _defaultBgColor;
    }

    public void UnsetChicken()
    {
        if (!_chicken)
            return;

        _chicken.Stats.StatUpgraded -= Stats_StatUpgraded;
        _chicken.Physical.Highlighted -= Physical_Highlighted;
        _chicken.Physical.UnHighlighted -= Physical_UnHighlighted;

        CancelInvoke(nameof(ResetHealthHighlight));
        CancelInvoke(nameof(ResetHPRegenHighlight));
        CancelInvoke(nameof(ResetDamageHighlight));
        CancelInvoke(nameof(ResetSpeedHighlight));
        CancelInvoke(nameof(ResetAccelerationHighlight));
        ResetHealthHighlight();
        ResetHPRegenHighlight();
        ResetDamageHighlight();
        ResetSpeedHighlight();
        ResetAccelerationHighlight();
    }

    private void Stats_StatUpgraded(object sender, ChickenStats.ChickenStatEventArgs e)
    {
        Text text = GetStatText(e.Stat);
        Text value = GetStatValue(e.Stat);
        string resetFunctionName = GetResetStatTextFunctionName(e.Stat);

        text.color = _highlightStatColor;
        text.fontStyle = FontStyle.Bold;
        value.color = _highlightStatColor;
        value.fontStyle = FontStyle.Bold;
        value.text = String.Format("{0:0.#}", e.NewValue);

        CancelInvoke(resetFunctionName);
        Invoke(resetFunctionName, _highlightDuration);
    }

    private void ResetHealthHighlight()
    {
        _healthBartext.color = _defaultHealthTextColor;
        _healthBartext.fontStyle = FontStyle.Normal;
    }

    private void ResetHPRegenHighlight()
    {
        _hpRegenText.color = _defaultStatColor;
        _hpRegenText.fontStyle = FontStyle.Normal;
        _hpRegenValue.color = _defaultStatColor;
        _hpRegenValue.fontStyle = FontStyle.Normal;
    }

    private void ResetDamageHighlight()
    {
        _damageText.color = _defaultStatColor;
        _damageText.fontStyle = FontStyle.Normal;
        _damageValue.color = _defaultStatColor;
        _damageValue.fontStyle = FontStyle.Normal;
    }

    private void ResetSpeedHighlight()
    {
        _speedText.color = _defaultStatColor;
        _speedText.fontStyle = FontStyle.Normal;
        _speedValue.color = _defaultStatColor;
        _speedValue.fontStyle = FontStyle.Normal;
    }

    private void ResetAccelerationHighlight()
    {
        _accelerationText.color = _defaultStatColor;
        _accelerationText.fontStyle = FontStyle.Normal;
        _accelerationValue.color = _defaultStatColor;
        _accelerationValue.fontStyle = FontStyle.Normal;
    }

    private Text GetStatValue(ChickenStatDefinition definition)
    {
        ChickenStatsManager manager = ChickenStatsManager.Instance;

        if (definition == manager.Health)
            return _healthBartext;
        else
           if (definition == manager.HealthRegen)
            return _hpRegenValue;
        else
            if (definition == manager.Damage)
            return _damageValue;
        else
            if (definition == manager.Speed)
            return _speedValue;
        else
            if (definition == manager.Acceleration)
            return _accelerationValue;

        return null;
    }
    private Text GetStatText(ChickenStatDefinition definition)
    {
        ChickenStatsManager manager = ChickenStatsManager.Instance;

        if (definition == manager.Health)
            return _healthBartext;
        else
           if (definition == manager.HealthRegen)
            return _hpRegenText;
        else
            if (definition == manager.Damage)
            return _damageText;
        else
            if (definition == manager.Speed)
            return _speedText;
        else
            if (definition == manager.Acceleration)
            return _accelerationText;

        return null;
    }

    private string GetResetStatTextFunctionName(ChickenStatDefinition definition)
    {
        ChickenStatsManager manager = ChickenStatsManager.Instance;

        if (definition == manager.Health)
            return nameof(ResetHealthHighlight);
        else
           if (definition == manager.HealthRegen)
            return nameof(ResetHPRegenHighlight);
        else
            if (definition == manager.Damage)
            return nameof(ResetDamageHighlight);
        else
            if (definition == manager.Speed)
            return nameof(ResetSpeedHighlight);
        else
            if (definition == manager.Acceleration)
            return nameof(ResetAccelerationHighlight);

        return null;
    }
}