using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MouseSeekBehavior : SeekBehavior
{
    void Update()
    {
        _target = MySessionUtils.Instance.MouseInWorld;
    }
}
