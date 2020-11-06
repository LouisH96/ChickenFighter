using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenBattle : MonoBehaviour
{
    [SerializeField] private List<List<Chicken>> _teams = new List<List<Chicken>>();

    [SerializeField] private bool _isFarmBattle = true;
    private bool _fightPaused = true;

    public bool IsPaused { get { return _fightPaused; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TryStartBattle()
    {
        _teams.SelectMany(t => t)
             .ToList()
             .Where(c => c.CanFight()).ToList()
             .ForEach(c => c.WakeupFromBattlePause());

        if(!ShouldBePaused())
           _fightPaused = false; 
    }

    public void EndBattle()
    {
        foreach (Chicken chicken in _teams.SelectMany(t => t))
        {
            Assert.IsNotNull(chicken, "chicken should not be null");
            chicken.RemoveFromBattle();
        }

        _fightPaused = true;

        Assert.IsTrue(_fightPaused, "fight should be paused cause less then 2 teams with active chickens");
    }

    public void PauzeBattle()
    {
        foreach (var chicken in _teams.SelectMany(t => t))
            chicken.PauzeBattle();

        _fightPaused = true;
    }

    public List<Chicken> AddChickenToNewTeam(Chicken chicken)
    {
        if (_teams.Any(team => team.Contains(chicken)))
        {
            return null;
        }

        List<Chicken> newTeam = new List<Chicken>() { chicken };
        _teams.Add(newTeam);
        chicken.AddToBattle(this);

        if (ShouldBePaused())
            PauzeBattle();
        else
            TryStartBattle();

        return newTeam;
    }

    public void AddChickenToTeam(Chicken chicken, List<Chicken> team)
    {
        if (_teams.Any(t => t.Contains(chicken)))
        {
            return;
        }

        if (_teams.Contains(team))
        {
            team.Add(chicken);
            chicken.AddToBattle(this);
        }

        if (ShouldBePaused())
            PauzeBattle();
        else
            TryStartBattle();
    }

    public List<Chicken> GetAllChickens()
    {
        return _teams.SelectMany(t => t).ToList();
    }

    public void OnChickenWakeUp(Chicken chicken)
    {
        if (!_teams.Any(t=> t.Contains(chicken)))
            return;

        if (!ShouldBePaused())
            TryStartBattle();
    }

    public bool ShouldBePaused()
    {
        return _teams.Count(t => t.Any(c => c.CanFight())) < 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        List<Chicken> enteredChickens = GetChickensFromCollider(other);

        if (enteredChickens != null)
        {
            foreach (var chicken in enteredChickens)
            {
                //Debug.Log("add " + chicken.name + " to battle");
                AddChickenToNewTeam(chicken);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        List<Chicken> enteredChickens = GetChickensFromCollider(other);

        if (enteredChickens != null)
        {
            foreach (var chicken in enteredChickens)
            {
                //Debug.Log("remove " + chicken.name + " from battle");
                RemoveChickenOutOfBattle(chicken);
            }
        }
    }

    private List<Chicken> GetChickensFromCollider(Collider other)
    {
        Chicken thrownChicken = IsThrownChicken(other);

        if (thrownChicken != null)
            return new List<Chicken> { thrownChicken };

        Farmer farmer = IsFarmer(other);

        if (farmer != null)
                 return farmer.GrabbedChickens;

        else return null;
    }

    public Farmer IsFarmer(Collider other)
    {
        if (other.GetType() == typeof(CharacterController))
        {
            if (other.CompareTag("Player"))
            {
                return other.GetComponent<Farmer>();
            }
        }

        return null;
    }

    public Chicken IsThrownChicken(Collider other)
    {
        if (other.CompareTag("Chicken"))
        {
            if (other.GetType() == typeof(BoxCollider))
            {
                return other.transform.parent.GetComponent<Chicken>();
            }
        }
        return null;
    }

    public void RemoveChickenOutOfBattle(Chicken chicken)
    {
        List<Chicken> allyTeam = GetAllyTeam(chicken);
        if (allyTeam == null)
            return;

        allyTeam.Remove(chicken);
        if (allyTeam.Count == 0)
            _teams.Remove(allyTeam);

        chicken.RemoveFromBattle();

        if (ShouldBePaused())
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
