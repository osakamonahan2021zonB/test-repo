using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBattle : MonoBehaviour
{
    public scendatas scendatas;
    public GameObject select;
    public GameObject warning;
    public Animator TV;
    public GameObject butn;
    bool down;
    bool display;
    private void Start()
    {
        down = false;
        display = false;
        TV.Play("idle");
    }
    public void Update()
    {
        Debug.Log("Alive");
        if (!butn.activeSelf && down)
        {
            TV.SetTrigger("Act");
            down = false;
        }
        if (display&&!select.activeSelf&&down)
        {
            TV.SetTrigger("Act");
            down = false;
        }
    }
    public void pushed()
    {
        Debug.Log("pree");
        if (down) return;
        bool cansel=true;
        for(int i=0; i<scendatas.LengthBox(); i++)
        {
            if(cansel)cansel = scendatas.GetBox(i + 1) == null;
        }
        if (cansel)
        {
            warning.SetActive(true);
        }
        else
        {
            TV.SetTrigger("Act");
            down = true;
        }
    }
    public void Animated()
    {
        if (display) { display = false;        return; }

        select.SetActive(true);
        display = true;
    }
}
