using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    //---Components---
    [SerializeField] private Chicken _chicken = null;

    private Chicken _attackingChicken = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        FightBodyPart bodyPart = other.gameObject.GetComponent<FightBodyPart>();

        if (!bodyPart)
            return;

        if (_chicken.IsEnemy(bodyPart.Chicken))
        {
            _attackingChicken = bodyPart.Chicken;
            _chicken.IsAttacking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FightBodyPart bodyPart = other.gameObject.GetComponent<FightBodyPart>();

        if (!bodyPart)
            return;

        if (_attackingChicken == bodyPart.Chicken)
        {
            _chicken.IsAttacking = false;
            _attackingChicken = null;
        }
    }
}
