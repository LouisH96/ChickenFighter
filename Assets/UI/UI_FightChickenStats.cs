using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FightChickenStats : MonoBehaviour
{
    [SerializeField] private Image _hp = null;
    [SerializeField] private Text _text = null;

    public Chicken Chicken = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Chicken)
        {
            _text.text =  Chicken.name + ": " + Chicken.CurrentHealth + "/" + Chicken.MaxHealth;
            _hp.transform.localScale = new Vector3(Chicken.CurrentHealthRatio, 1, 1);
        }
        else
        {
            _text.text = "empty";
            _hp.transform.localScale = new Vector3(0.0f, 1, 1);
        }
    }
}
