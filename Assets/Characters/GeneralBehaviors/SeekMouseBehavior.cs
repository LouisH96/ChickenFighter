using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SeekBehavior))]
public class SeekMouseBehavior : MonoBehaviour
{
    //---Components---
    private SeekBehavior _seekBehavior = null;

    void Awake()
    {
        _seekBehavior = GetComponent<SeekBehavior>();
    }

    void Update()
    {
        Vector3 mousePos = MySessionUtils.Instance.GetWorldMousePosition();
        _seekBehavior.Target = mousePos;
    }
}