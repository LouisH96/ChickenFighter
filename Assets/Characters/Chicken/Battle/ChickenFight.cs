using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenFight : MonoBehaviour
{
    //--- Components ---
    [SerializeField] private Chicken _chicken;
    [SerializeField] private ChickenPhysical _physical;

    //--- Fight Components ---
    private List<FightBodyPart> _bodyParts = new List<FightBodyPart>();
    private Beak2 _beak = null;

    //--- Variables ---
    private ChickenBattle _battle = null;

    //--- Public Variable Access ---
    public bool IsInBattleArea { get { return _battle != null; } }
    public bool CanFight { get { return _physical.State == ChickenPhysical.PhysicalState.Character; } }

    void Start()
    {
        _bodyParts = GetComponentsInChildren<FightBodyPart>().ToList();
        _beak = GetComponentInChildren<Beak2>();

        Assert.IsTrue(_bodyParts.TrueForAll(b => b.gameObject.layer == LayerMask.NameToLayer("FightBodyParts")), "Not all FightBodyPart were of that layer");
    }

    void Update()
    {
        
    }
}
