using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BaseBlock : block
{
    // Update is called once per frame
    [SerializeField] Slider hpbar;
    float MaxHP;
    new void Start()
    {
        MaxHP = hp;
        if (rendthings != null)
        {
            _renderer = rendthings;
        }
        else
        {
            _renderer[0] = GetComponent<Renderer>();
        }
        original = new Material(_renderer[0].material);
    }
    new void Update()
    {
        hpbar.value = hp / MaxHP;
        if (hp <= 0)
        {
            Time.timeScale = 0;
            GM.gM.SendMessage("Finishbattel",false);
        }
    }

}
