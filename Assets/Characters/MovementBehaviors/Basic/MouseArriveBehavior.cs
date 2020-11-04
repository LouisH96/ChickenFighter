using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseArriveBehavior : ArriveBehavior
{
    void Update()
    {
        _target = MySessionUtils.Instance.MouseInWorld;
    }
}
