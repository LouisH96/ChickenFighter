using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFleeBehavior : FleeBehavior2
{
    void Update()
    {
        _target = MySessionUtils.Instance.GetWorldMousePosition();
    }
}
