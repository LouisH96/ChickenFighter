using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    //---Components---
    [SerializeField] private Chicken _chicken = null;

    private List<FightBodyPart> _targets = new List<FightBodyPart>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _targets.RemoveAll(p => p == null);
        if (_targets.Count == 0)
            _chicken.IsAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        FightBodyPart bodyPart = other.gameObject.GetComponent<FightBodyPart>();

        if (!bodyPart)
            return;

        if (_chicken.IsEnemy(bodyPart.Chicken))
        {
            _targets.Add(bodyPart);
            _chicken.IsAttacking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FightBodyPart bodyPart = other.gameObject.GetComponent<FightBodyPart>();

        if (!bodyPart)
            return;

        if (_targets.Contains(bodyPart))
        {
            _targets.Remove(bodyPart);

            if (_targets.Count == 0)
                _chicken.IsAttacking = false;
        }
    }
}
