using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagearea : MonoBehaviour
{
    public float lifetime = -1;
    public (float, Vector3) damage;
    public enum Type
    {
        Enemy,
        Block,
    }
    public Type Attacktype = Type.Block;
    string damagetag;
    // Start is called before the first frame update
    void Start()
    {
        switch (Attacktype)
        {
            case Type.Block: damagetag = "box"; break;
            case Type.Enemy: damagetag = "enemy"; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime == -1) return;
        if (lifetime <= 0) Destroy(gameObject);
        lifetime -= Time.deltaTime;
    }



    private void OnTriggerEnter(Collider collision)
    {
        //  Debug.Log(collision.gameObject);
        Debug.Log(collision.gameObject);
        if (collision.gameObject.tag == "enemy")
        {
            collision.gameObject.SendMessage("Damage", damage);
        }
    }
}
