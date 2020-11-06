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
    private CharacterController _characterController = null;

    void Awake()
    {
        _bodyParts = GetComponentsInChildren<FightBodyPart>().ToList();
        Assert.IsTrue(_bodyParts.TrueForAll(b => b.gameObject.layer == LayerMask.NameToLayer("FightBodyPart")));

        _beak = GetComponentInChildren<Beak2>();

        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        
    }
}
