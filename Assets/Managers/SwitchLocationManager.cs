using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dirty because lack of time
public class SwitchLocationManager : MonoBehaviour
{
    [SerializeField] private Car[] _cars;
    [SerializeField] private GameObject _player = null;
    [SerializeField] private CharacterController _farmerCharacterController = null;
    [SerializeField] private Farmer _farmer = null;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Car MoveFrom(int fromIndex)
    {
        int next = fromIndex + 1;
        if (next >= _cars.Length)
            next = 0;

        Car from = _cars[fromIndex];
        Car to = _cars[next];

        Vector3 carDelta = to.transform.position - from.transform.position;

        int amntChickens = from.Trunk.Chickens.Count;
        for (int i = 0; i < amntChickens; i++)
        {
            Chicken chicken = from.Trunk.Chickens[0];
            chicken.MoveTo(chicken.transform.position + carDelta, chicken.transform.rotation);
        }

        _farmer.PenAreaExit(from.Trunk);
        _farmerCharacterController.enabled = false;
        _player.transform.position += carDelta;
        _farmerCharacterController.enabled = true;

        return to;
    }
}
