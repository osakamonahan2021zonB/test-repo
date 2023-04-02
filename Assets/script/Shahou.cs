using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shahou : block
{
    [SerializeField] Transform target;
    [SerializeField] bool active;
    public List<GameObject> targets;
    float maxdistance;
    [Space(20)]
    [SerializeField] GameObject rotater;
    [SerializeField] GameObject arm;
    [SerializeField] GameObject shotpoint;
    [SerializeField] float MukiHosei=0;
    [SerializeField] float Hightbounus=0;
    [Range(0, 3)]
    [SerializeField] float additonalhigth;
    [Space(20)]
    Animator burrelanimator;
    [Range(0, 3)]
    int ThoughHight;
    float shoottime;
    float higth;
    GameObject rangeobj;
    GameObject[] subrange = new GameObject[2];
    MeshRenderer[] submesh = new MeshRenderer[2];
    range[] subranges = new range[2];
    MeshRenderer rangemesh;
    Collider thiscol;
    Rigidbody rigd;
    public LayerMask layerMask;
    bool rerangit;
    override protected void Start()
    {
        base.Start();
        hp = blockdata.HP;
        higth = transform.position.y + 0.505f;
        rangeobj=transform.Find("rang").gameObject;
        rangeobj.transform.localScale = (Vector3.one - Vector3.up) * blockdata.Range + Vector3.up * 0.5f;
        thiscol = GetComponent<Collider>();
        targets = rangeobj.GetComponent<range>().targets;
        rangeobj.SendMessage("changeType", (range.Type.Enemy));
        rangemesh = rangeobj.transform.Find("ソナー").GetComponent<MeshRenderer>();
        burrelanimator = arm.GetComponent<Animator>();
        rigd = this.GetComponent<Rigidbody>();
        rerange();
    }
    override protected void Update()
    {
        base.Update();
        if (shoottime > 0)
        {
            shoottime -= Time.deltaTime;
        }
        rangemesh.enabled = displayrange;
        submesh[0].enabled = displayrange;
        submesh[1].enabled = displayrange;
        if (subranges[0].targets.Count > 0)
        {
            for (int i = 0; i < subranges[0].targets.Count; i++)
            {
                if (subranges[0].targets[i] == null)
                {
                    subranges[0].targets.RemoveAt(i);
                    break;
                }
            }
        }
        if (subranges[1].targets.Count > 0)
        {
            for (int i = 0; i < subranges[1].targets.Count; i++)
            {
                if (subranges[1].targets[i] == null)
                {
                    subranges[1].targets.RemoveAt(i);
                    break;
                }
            }
        }
        if (targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null)
                {
                    targets.RemoveAt(i);
                    break;
                }
            }

        }
        List<GameObject> Currenttargets = new List<GameObject>();
        Currenttargets.AddRange(targets);
        Currenttargets.AddRange(subranges[0].targets);
        Currenttargets.AddRange(subranges[1].targets);
        active = Currenttargets.Count > 0;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        if (rerangit)
        {
            if (rigd.velocity.y > 0)
            {
                rerangit = false;
                rerange();
            }
        }
        else
        {
            if (rigd.velocity.y < 0)
            {
                rerangit = true;
            }
        }
        if (active && complete)
        {
            /* float priority = 100;//ホームまでの距離
             for (int i = 0; i < Currenttargets.Count; i++)
             {
                 if (Currenttargets[i] == null)
                 {
                     Currenttargets.RemoveAt(i);
                     break;
                 }
                 float a = Currenttargets[i].GetComponent<enemy_base>().remaining;
                 if (a < priority)
                 {
                     priority = a;
                     target = Currenttargets[i].transform;
                 }
                 else
                 {
                     break;
                 }
             }
             if (target == null)
             {
                 active = false;
                 return;
             }
             rotater.transform.LookAt(new Vector3(target.position.x, rotater.transform.position.y, target.position.z));
             rotater.transform.rotation *= Quaternion.Euler(0f, MukiHosei, 0f);
             if(Mathf.Round(transform.position.y) <= Mathf.Round(target.position.y))
             {

             Vector3 hosei = shotpoint.transform.position - arm.transform.position;
             arm.transform.LookAt(target.position - hosei);
             arm.transform.rotation *= Quaternion.Euler(0f, MukiHosei, 0f);
             arm.transform.rotation = Quaternion.Euler(new Vector3(arm.transform.rotation.eulerAngles.x, rotater.transform.eulerAngles.y,0));
             }

             if (shoottime <= 0)
             {
                 shoot(Mathf.Round(transform.position.y) > Mathf.Round(target.position.y));
             }*/
            Transform Pretarget = null;
            float prepriority = 100;
            float priority = 100;//ホームまでの距離
            for (int i = 0; i < Currenttargets.Count; i++)
            {
                if (Currenttargets[i] == null)
                {
                    Currenttargets.RemoveAt(i);
                    break;
                }
                float a = Currenttargets[i].GetComponent<enemy_base>().remaining;
                if (a < priority)
                {
                    target = null;
                    RaycastHit hitInfo;
                    bool isHit = Physics.Raycast(transform.position, Currenttargets[i].transform.position - transform.position, out hitInfo, Vector3.Distance(transform.position, Currenttargets[i].transform.position), layerMask);
                    Vector3 pos = Currenttargets[i].transform.position - transform.position;
                    if (isHit && hitInfo.collider.CompareTag("box"))
                    {

                        isHit = Physics.Raycast(transform.position+Hightbounus*Vector3.up,new Vector3(pos.x, transform.position.y + Hightbounus ,pos.z), out hitInfo, Vector3.Distance(transform.position, new Vector3(pos.x, transform.position.y , pos.z)), layerMask);
                        if (!isHit)
                        {
                            target = Currenttargets[i].transform;
                            priority = a;
                        }
                        else
                        {            
                         if (target == null && a < prepriority&& hitInfo.collider.CompareTag("box"))
                         {
                            prepriority = a;
                            Pretarget = Currenttargets[i].transform;
                         }
                        }
                    }
                    else
                    {
                        target = Currenttargets[i].transform;
                        priority = a;
                    }
                }
                else
                {
                    break;
                }

            }
            if (target == null)
            {

                if (Pretarget != null)
                {
                    rotater.transform.LookAt(new Vector3(Pretarget.position.x, rotater.transform.position.y, Pretarget.position.z));
                    rotater.transform.rotation *= Quaternion.Euler(0f, MukiHosei, 0f);
                }
                active = false;
                return;
            }
            rotater.transform.LookAt(new Vector3(target.position.x, rotater.transform.position.y, target.position.z));
            rotater.transform.rotation *= Quaternion.Euler(0f, MukiHosei, 0f);
            if (shoottime <= 0)
            {
                shoot();
            }
        }

    }

    void culcuratedistanse(float hig)
    {

    }
    void shoot()
    {
        burrelanimator.SetTrigger("shot");
        shoottime = blockdata.AttackRate;
        GameObject shoting;
        shoting = Instantiate(blockdata.Bullet, shotpoint.transform.position + shotpoint.transform.forward * 0.2f, arm.transform.rotation * Quaternion.Euler(0, 0, 90));
        bulletsc buletsc = shoting.GetComponent<bulletsc>();
        buletsc.ign = thiscol;
        buletsc.damage.Item1 = blockdata.Damage;
        buletsc.damage.Item2 = blockdata.Knockback.y * Vector3.up + blockdata.Knockback.x * rotater.transform.forward + blockdata.Knockback.z * rotater.transform.right;
        buletsc.target = target.gameObject;
        shoting.transform.parent = null;
    }
    void rerange()
    {
        higth = transform.position.y + 0.6f;
        if (subrange[0] != null)
        {
            Destroy(subrange[0]);
            Destroy(subrange[1]);
        }
        List<int> hi = new List<int>();
        switch ((int)higth)
        {
            case 1: hi = new List<int> { 2, 3 }; break;
            case 2: hi = new List<int> { 1, 3 }; break;
            case 3: hi = new List<int> { 1, 2 }; break;

        }
        for (int i = 0; i < 2; i++)
        {
            subrange[i] = Instantiate(rangeobj, transform);
            subrange[i].transform.position = new Vector3(transform.position.x, hi[i] - 0.5f, transform.position.z);
            submesh[i] = subrange[i].transform.GetChild(0).GetComponent<MeshRenderer>();
            subranges[i] = subrange[i].GetComponent<range>();
            subrange[i].transform.localScale = (Vector3.one - Vector3.up) * blockdata.Range * Mathf.Pow(blockdata.RangeRate, -hi[i] + higth) + Vector3.up * 0.5f;
        }
    }
}
