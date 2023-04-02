using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletsc : MonoBehaviour
{
    public float lifetime =5;
    public (float, Vector3) damage;
    public Collider ign;
    public bool dofall;
    public GameObject target;
    public int pene;
    // Start is called before the first frame update
   virtual public void Start()
    {
       Collider[] collider=gameObject.GetComponents<Collider>();
        for(int i=0;i<collider.Length;i++)Physics.IgnoreCollision(collider[i],ign, true);
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (lifetime <= 0) Destroy(gameObject);
        lifetime -= Time.deltaTime;
    }



    virtual public void OnTriggerEnter(Collider collision)
    {
        //  Debug.Log(collision.gameObject);

        if (collision.gameObject.tag == "enemy")
        {
            collision.gameObject.SendMessage("Damage",damage);
            if (pene != 0)
            {
                pene--;
                return;
            }
        }
        Destroy(gameObject);
    }

}
