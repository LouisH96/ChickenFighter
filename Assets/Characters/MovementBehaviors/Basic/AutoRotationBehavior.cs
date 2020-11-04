using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotationBehavior : FaceBehavior2
{
    //---Variables
    private Vector3? _direction = null;

    //---Public---
    public Vector3? Direction
    {
        set { _direction = value; }
        get { return _direction; }
    }

    public override Vector3 Target
    {
        get { return _target; }
        set
        {
            _target = value;
            _direction = null;
        }
    }

    public override RotationOutput HandleRotation(MovementAgent agent)
    {
        if (_direction != null)
            _target = agent.transform.position + (Vector3)_direction;

        return base.HandleRotation(agent);
    }
}
