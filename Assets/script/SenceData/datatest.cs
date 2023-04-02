using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class datatest : MonoBehaviour
{
    public scendatas Scendata;
    public TextMeshProUGUI Text;
    private void Start()
    {
        Text = gameObject.GetComponent<TextMeshProUGUI>();
        string a = null;
        for (int i = 1; i <= 5; i++)
        {
            a += Scendata.GetBox(i);
        }
        Text.text = a;
    }
}
