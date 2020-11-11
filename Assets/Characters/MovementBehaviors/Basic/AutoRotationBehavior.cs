using Assets.Characters.MovementBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotationBehavior : FaceBehavior
{
    //---Variables
    private Vector2? _direction = null;

    //---Public---
    public Vector2? Direction2D
    {
        set { _direction = value; }
        get { return _direction; }
    }

    public override Vector2 Target2D
    {
        set
        {
            base.Target2D = value;
            _direction = null;
        }
    }

    public override Vector3 Target3D
    {
        set
        {
            base.Target3D = value;
            _direction = null;
        }
    }

    public override RotationOutput HandleRotation(MovementAgent agent)
    {
        if (_direction != null)
            Target2D = agent.Pos2D + (Vector2)_direction;

        return base.HandleRotation(agent);
    }
}
