using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSeekBehavior : SeekBehavior
{
    void Update()
    {
        Target = MySessionUtils.Instance.MouseInWorld;
    }
}
