using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1 : MonoBehaviour
{
    [SerializeField] private List<Chicken> _lostChickens = new List<Chicken>();
    [SerializeField] private ChickenPen _recoverPen = null;
    [SerializeField] private UIManager _uIManager = null;
    [SerializeField] private GameObject _bill = null;
    private bool _taskDone = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var chicken in FindObjectsOfType<ChickenPhysical>())
        {
            if(!_lostChickens.Contains(chicken.Chicken))
            {
                chicken.CanPickup = false;
            }
        }

        _uIManager.ChatText = "Bill: Oi lil Billy, GRAB those lost chickens and put them back in this pen.";
        _uIManager.TopText = "GRAB and return the 5 lost chickens";
    }

    void KillBill()
    {
        float delay = 10.0f;
        _bill.transform.Rotate(Vector3.forward, -90.0f, Space.Self);
        _bill.transform.Rotate(Vector3.up, -90.0f, Space.Self);
        _uIManager.ShowChatTime = delay;
        _uIManager.ChatText = "Bill: Ooh no I just died from a heartattack.\n The ranch is yours now Billy.";
        Invoke(nameof(EndTutorial), delay);
    }

    void EndTutorial()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        var recovered = _recoverPen.Chickens;
        if (_lostChickens.All(c => recovered.Contains(c)) && !_taskDone)
        {
            _uIManager.ShowChatTime = 2.0f;
            _uIManager.ChatText = "Bill: Good work Billy.";

            Invoke("KillBill", _uIManager.ShowChatTime);
            _taskDone = true;
        }
    }
}
