using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class efectdie : MonoBehaviour
{
   public float lifetime = 5f; // �폜�܂ł̎��ԁi�b�j
    public bool DontDie;

void Start()
{
    if(gameObject.name=="GunLine(Clone)")lifetime = 1/GetComponent<Renderer>().material.GetFloat("_Kank");
            if(!DontDie)Destroy(gameObject, lifetime);

}
    public void aaaaaaaaaaaaaaaaaaaaaadisableSelf()
    {
        gameObject.SetActive(false);
    }
}
