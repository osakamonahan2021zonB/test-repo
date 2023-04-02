using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationYajirusi : MonoBehaviour
{
    public scendatas Scendatas;
    public Transform[] buttons;
    public Transform basepoint;
    public GameObject[] Scloes;
    public GameObject[] blockdataobjects;
    public blockdata[] blockdatas;
    public InformButton InformButton;
    Image image;
    bool display;
    public int number;
    int tracking;
    float Interval=0.1f;
    float time;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        display = false;
        tracking = 0;
        for(int i = 5; i>0; i--)
        {
          if (Scendatas.GetBox(i) == null) { transform.position = buttons[i-1].position+Vector3.up*80f; display = true;tracking = i; }
        }
        image.enabled = display;
        time += Time.deltaTime;
        if (time > Interval)
        {
            time = 0;
            int a;
            a = FindNearest();
            if (a != number)
            {
                number = a;
                InformButton.infodata = blockdatas[a];
                InformButton.settext();
            }
        }
    }
    private void Awake()
    {
        InformButton.infodata = blockdatas[FindNearest()];
    }
    private int FindNearest()
    {
        float nearestDistance = Mathf.Infinity;
        int nearestIndex = 0;

        for (int i = 0; i < Scloes.Length; i++)
        {
            float distance =(basepoint.position-Scloes[i].transform.position).sqrMagnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }
    public void cilck()
    {
        Scendatas.SetBox(blockdataobjects[number],tracking);
        buttons[tracking - 1].gameObject.GetComponent<TaletButton>().Plase();
    }
}
