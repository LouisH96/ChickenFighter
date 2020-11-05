using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFaceBehavior : FaceBehavior
{ 
    void Update()
    {
        _target = MySessionUtils.Instance.GetWorldMousePosition();
    }
}
