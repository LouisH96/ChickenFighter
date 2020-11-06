using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MovementAgent))]
public class FightChicken : MonoBehaviour
{
    //---Components---
    [SerializeField] private MovementBehavior _flee = null;
    [SerializeField] private SeekBehavior _chase = null;
    private MovementAgent _agent = null;
    private ChickenAttack _chickenAttack = null;
    private ChickenStats _chickenStats = null;

    //---Stats---
    [SerializeField] private float _fleeTime = 5.0f;
    [SerializeField] private float _chaseTime = 5.0f;
    [SerializeField] private float _behaviorTimeRandomRatio = 0.2f; //max 20% of time can be added or subtracted

    //---Variables--
    [SerializeField] private FightChicken _enemy = null;
    [SerializeField] private float _currentHealth = 1.0f;

    public FightChicken Enemy { get { return _enemy; } set { _enemy = value; } }

    public float HealthRatio { get { return _currentHealth / 50.0f; } }

    public int CurrentHealth { get { return (int)_currentHealth; } }
    public int MaxHealth { get { return (int)_chickenStats.Health; } }


    void Awake()
    {
        AddTagRecursively(transform, tag);
        _agent = GetComponent<MovementAgent>();
        _chickenAttack = GetComponent<ChickenAttack>();
        _chickenStats = GetComponent<ChickenStats>();
        _currentHealth = _chickenStats.Health;

        _agent.MaxVelocity = _chickenStats.MaxSpeed;
        _agent.Acceleration = _chickenStats.Acceleration;
    }

    private void Start()
    {
        if (!_agent || !_flee || !_chase || !_enemy)
            return;

        GetComponent<FleeBehavior>().LockedTarget = _enemy.gameObject.transform;
        _chase.LockedTarget = _enemy.gameObject.transform;

        if (Random.value < 0.5f)
            _agent.MovementBehavior = _flee;
        else
            _agent.MovementBehavior = _chase;
        SwapBehavior();
    }

    void Update()
    {

    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR

#endif
    }

    public void SwapBehavior()
    {
        float behaviorTime;

        if (_agent.MovementBehavior == _flee)
        {
            _agent.MovementBehavior = _chase;
            behaviorTime = _chaseTime;
            _chase.ShowDebugInfo = true;
            _flee.ShowDebugInfo = false;
        }
        else
        {
            _agent.MovementBehavior = _flee;
            behaviorTime = _fleeTime;
            _chase.ShowDebugInfo = false;
            _flee.ShowDebugInfo = true;
        }

        behaviorTime *= 1.0f
            + Random.Range(-_behaviorTimeRandomRatio, _behaviorTimeRandomRatio);
        Invoke(nameof(SwapBehavior), behaviorTime);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
            Destroy(this.gameObject);
    }

    //from the internet
    void AddTagRecursively(Transform trans, string tag)
    {
        trans.gameObject.tag = tag;
        if (trans.childCount > 0)
            foreach (Transform t in trans)
                AddTagRecursively(t, tag);
    }
}