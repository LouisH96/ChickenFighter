using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehavior2 : SeekBehavior2
{
    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        _target = agent.transform.position + agent.transform.position - _target;
        return base.HandleMovement(agent);
    }
}
