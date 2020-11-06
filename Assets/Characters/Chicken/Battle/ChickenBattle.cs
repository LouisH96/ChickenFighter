using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenBattle : MonoBehaviour
{
    [SerializeField] private List<List<Chicken>> _teams = new List<List<Chicken>>();

    [SerializeField] private bool _isFarmBattle = true;
    private bool _fightPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        StartBattle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartBattle()
    {
        _teams.SelectMany(t => t)
             .ToList()
             .ForEach(c => c.StartBattle(this));
    }

    public void EndBattle()
    {
        //foreach(List<Chicken> team in _teams)
        //{

        //}
        //if(_chickenA)
        //{
        //    Chicken a = _chickenA;
        //    _chickenA = null;
        //    a.EndBattle();
        //}

        //if(_chickenB)
        //{
        //    Chicken b = _chickenB;
        //    _chickenB = null;
        //    b.EndBattle();
        //}
    }

    public void PauzeBattle()
    {
        _fightPaused = true;

        foreach (var chicken in _teams.SelectMany(t => t))
            chicken.ChangeState(Chicken.ChickenState.Farm);
    }

    public List<Chicken> AddChickenToNewTeam(Chicken chicken)
    {
        List<Chicken> newTeam = new List<Chicken>() { chicken };
        _teams.Add(newTeam);
        chicken.StartBattle(this);

        if (_fightPaused)
        {
            _fightPaused = false;
            StartBattle();
        }

        return newTeam;
    }

    public void AddChickenToTeam(Chicken chicken, List<Chicken> team)
    {
        if (_teams.Contains(team))
        {
            team.Add(chicken);
            chicken.StartBattle(this);
        }

        if (_fightPaused)
        {
            _fightPaused = false;
            StartBattle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isFarmBattle
            && other.GetType() == typeof(CharacterController)
            && other.CompareTag("Chicken"))
        {
            Debug.Log("add chicken to battle");
            AddChickenToNewTeam(other.GetComponentInParent<Chicken>());
        }
    }
    public void RemoveChickenOutOfBattle(Chicken chicken)
    {
        List<Chicken> allyTeam = GetAllyTeam(chicken);
        if (allyTeam == null)
            return;

        allyTeam.Remove(chicken);
        if (allyTeam.Count == 0)
            _teams.Remove(allyTeam);

        chicken.EndBattle();

        if (_teams.Count == 1)
            PauzeBattle();
    }

    public bool IsEnemy(Chicken ally, Chicken possibleEnemy)
    {
        List<Chicken> allyTeam = GetAllyTeam(ally);
        if (allyTeam == null)
        {
            Debug.Log("ally not found in a team");
            return false;
        }

        if (allyTeam.Any(teammate => teammate == possibleEnemy))
            return false;
        else
            return true;
    }

    public Chicken GetClosestEnemy(Chicken chicken)
    {
        List<Chicken> allyTeam = GetAllyTeam(chicken);

        float closestSqrDistance = 0.0f;
        Chicken closestChicken = null;
        foreach (var enemy in _teams.Where(t => t != allyTeam).SelectMany(t => t))
        {
            float dist = (enemy.transform.position - chicken.transform.position).sqrMagnitude;
            if (closestChicken == null
                || dist < closestSqrDistance)
            {
                closestChicken = enemy;
                closestSqrDistance = dist;
            }
        }

        return closestChicken;
    }

    public List<Chicken> GetAllyTeam(Chicken chicken)
    {
        return _teams.FirstOrDefault(team => team.Any(c => c == chicken));
    }
}
