using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial2 : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager = null;
    private Farmer _farmerAtDoor = null;
    [SerializeField] private ChickenPen _trunk;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.ShowChatTime = 6.0f;
        _uiManager.ChatText = "Duke: Hey there buddy. BREED me some more of them brown chickens.\n 5 Of them will do.";
    }

    // Update is called once per frame
    void Update()
    {
        if (_farmerAtDoor)
        {
            if (_trunk.Chickens.Where(c => c.Breeder.IsSuperiorRace).Count() >= 5)
            {
                _uiManager.TopText = "'LMB' to send chickens to Duke";

                if(Input.GetAxis("Action1") > 0.0f)
                {
                    _uiManager.ShowChatTime = 6.0f;
                    _uiManager.ChatText = "Duke: Thanks fella, see ya.";
                    Invoke(nameof(EndTutorial), 6.0f);
                }
            }
            else
            {
                _uiManager.TopText = "Load atleast 5 brown chickens in the trunk";
            }
        }
        else
        {
            _uiManager.TopText = "";
        }
    }

    private void EndTutorial()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        _farmerAtDoor = Farmer.GetFromCollider(other);


        _uiManager.TopText = "'LMB' to send chickens to Duke";
    }

    private void OnTriggerExit(Collider other)
    {
        _farmerAtDoor = null;
        _uiManager.TopText = "";
    }
}
