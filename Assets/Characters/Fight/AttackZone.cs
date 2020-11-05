using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private static List<string> _teamTags = new List<string>{ "TeamA", "TeamB" };

    private string _enemyTag;
    private GameObject _enemy = null;

    public GameObject Enemy { get { return _enemy; } }
    void Start()
    {
        _enemyTag = _teamTags.FirstOrDefault(t => !CompareTag(t));
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_enemyTag))
            _enemy = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _enemy)
            _enemy = null;
    }

   // void OnCollisionEnter(Collision collision)
   // {
   //     if (collision.gameObject.CompareTag(_enemyTag))
   //     _enemy = collision.gameObject;
   // }

   //void OnCollisionExit(Collision collision)
   // {
   //     if (collision.gameObject == _enemy)
   //         _enemy = null;
   // }
}
