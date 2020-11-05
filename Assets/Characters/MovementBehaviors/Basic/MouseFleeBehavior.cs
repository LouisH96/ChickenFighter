using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFleeBehavior : FleeBehavior
{
    void Update()
    {
        _target = MySessionUtils.Instance.GetWorldMousePosition();
    }
}
