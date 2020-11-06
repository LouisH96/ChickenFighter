using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ChickenPhysical : MonoBehaviour
{
    //---Components---
    private List<FightBodyPart> _bodyParts = new List<FightBodyPart>();
    private Beak2 _beak = null;
    [SerializeField] private CharacterController _characterController = null;

    //---Variables---
    private Chicken.ChickenState _state = Chicken.ChickenState.None;

    void Awake()
    {
        _bodyParts = GetComponentsInChildren<FightBodyPart>().ToList();

        Assert.IsTrue(_bodyParts.TrueForAll(b => b.gameObject.layer == LayerMask.NameToLayer("FightBodyParts")), "Not all FightBodyPart were of that layer");

        _beak = GetComponentInChildren<Beak2>();
    }

    private void Start()
    {
        ChangeState(_state);
    }

    void Update()
    {
        
    }

    public void ChangeState(Chicken.ChickenState newState)
    {
        switch (newState)
        {
            case Chicken.ChickenState.Farm:
                break;
            case Chicken.ChickenState.Fight:
                break;
            case Chicken.ChickenState.None:
                break;
            default:
                break;
        }
    }

    private void SetNoState()
    {

    }

    private void SetFarmWanderState()
    {

    }

    private void SetFarmPickedupState()
    {

    }

    private void SetFightState()
    {

    }
}
