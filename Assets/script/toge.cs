using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toge :block
{
    [Space(20)]
    [SerializeField] public Transform target;
    [SerializeField] GameObject animatorobj;
    Animator burrelanimator;
    [SerializeField] damagearea damagearea;
    float shoottime;


    float higth;
    GameObject rangeobj;
    MeshRenderer rangemesh;
    public List<GameObject> targets;
    public GameObject rotator;
    override protected void Start()
    {

        base.Start();
        hp = blockdata.HP;
        higth = transform.position.y + 0.5f;
        rangeobj = transform.GetChild(0).gameObject;//子0Rangeに！！！！
        targets = rangeobj.GetComponent<range>().targets;
        rangemesh = rangeobj.transform.GetChild(0).GetComponent<MeshRenderer>();
        burrelanimator = animatorobj.GetComponent<Animator>();
        damagearea.damage.Item1 = blockdata.Damage;
    }
    override protected void Update()
    {
        base.Update();
        if (complete && damagearea.damage.Item2 == Vector3.zero)
        {
            damagearea.damage.Item2 = blockdata.Knockback.y * Vector3.up + blockdata.Knockback.z * transform.forward + blockdata.Knockback.x * transform.right; 
        }
        if (shoottime > 0)
        {
            shoottime -= Time.deltaTime;
        }
        if ( targets.Count > 0&&complete)
        {
            for (int i = 0; i < this.targets.Count; i++)
             {
                 if (this.targets[i] == null)
                 {
                     this.targets.RemoveAt(i);
                     break;
                 }
             }
            this.targets.Remove(null);
            if (rotator != null)
            {
                float priority = 100;//ホームまでの距離
                for (int i = 0; i < targets.Count; i++)
                {
                    float a = targets[i].GetComponent<enemy_base>().remaining;
                    if (a < priority)
                    {
                        target = targets[i].transform;
                        priority = a;
                    }
                    else
                    {
                        break;
                    }
                }
                if (target != null)
                {
                    rotator.transform.LookAt(new Vector3(target.position.x, rotator.transform.position.y, target.position.z));
                  if (shoottime <= 0)
                   {
                   shoot();
                   }
                }
                
            }
            else
            {
                if (shoottime <= 0)
                {
              　　  shoot();
                }
            }
        }
        rangemesh.enabled = displayrange;
    }
    void shoot()
    {
        burrelanimator.SetTrigger("shot");
        shoottime =blockdata.AttackRate;
    }
}
