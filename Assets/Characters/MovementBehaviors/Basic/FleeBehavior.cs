using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehavior : SeekBehavior
{
    public override MovementOutput HandleMovement(MovementAgent agent)
    {
        Target = agent.transform.position + agent.transform.position - Target;
        return base.HandleMovement(agent);
    }
}
