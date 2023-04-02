using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class range : MonoBehaviour
{
    public List<GameObject> targets;
    public enum Type {
        Enemy,
        Block,
    }
    public Type Attacktype =Type.Block;
    string Tag;
    MeshRenderer mesh;
    Material original;
    public float RestrictRang;
    List<GameObject> Hiddentargets=null;
    private void Start()
    {
        
        switch (Attacktype)
        {
            case Type.Block:Tag = "box"; break;
            case Type.Enemy:Tag = "enemy"; break;
        }
        if (Attacktype == Type.Enemy)
        {
        mesh = transform.GetChild(0).GetComponent<MeshRenderer>();
        original = new Material(mesh.material);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.tag == Tag)
        {
            if (RestrictRang > 0)
            {
                Vector3 targetDirection = other.gameObject.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, targetDirection);    
                if (angle < 0)
                {
                    angle += 360;
                }
                if (angle > RestrictRang / 2) { Hiddentargets.Add(other.gameObject); return; }
            }
            targets.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        if (other.tag == Tag)
        {
            if (Hiddentargets != null)
            {
                if (Hiddentargets.Count > 0)
                {
                    if (Hiddentargets.Contains(other.gameObject))
                    {
                        Hiddentargets.Remove(other.gameObject);
                        return;
                    }
                }
            }
            targets.Remove(other.gameObject);
        }
    }
    private void Update()
    {
        if (Attacktype != Type.Enemy) return;
        if (mesh.enabled==true)
        {
            switch ((int)transform.position.y)
            {
                case 0:
                    mesh.material.SetColor("_Color", new Color(255, 0,0, 1));
                    break;
                case 1:
                    mesh.material.SetColor("_Color", new Color(0,255,0,1));
                    break;
                case 2:
                    mesh.material.SetColor("_Color", new Color(0,0,255, 1));
                    break;
            }
        }
        if (Hiddentargets != null)
        {
         if (Hiddentargets.Count > 0)
         {
            foreach(GameObject g in Hiddentargets)
            {
                Vector3 targetDirection = g.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, targetDirection);
                if (angle < 0)
                {
                    angle += 360;
                }
                if (angle <= RestrictRang / 2)
                {
                    Hiddentargets.Remove(g);
                    targets.Add(g);
                    return;
                }
            }
         }
        }

    }
    public void changeType(Type a)
    {
        Attacktype = a;
    }
}
