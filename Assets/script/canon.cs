using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canon : block
{

    [SerializeField] Transform target;
    [SerializeField] bool active;
    public List<GameObject> targets;
    float maxdistance;
    [Space(20)]
    [SerializeField] GameObject rotater;
    [SerializeField] GameObject arm;
    [SerializeField] GameObject shotpoint;
    [SerializeField] Transform[] shotpoints;
    int shotcounter;
    [Range(0,3)]
    [SerializeField]float  additonalhigth;
    [SerializeField] float MukiHosei = 0;
    [Space(20)]
    Animator burrelanimator;
    float shoottime;
    float higth;
    [SerializeField] GameObject rangeobj;
    GameObject[] subrange=new GameObject[2];
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
        rangeobj.transform.localScale = (Vector3.one-Vector3.up)*blockdata.Range+Vector3.up*0.5f;
        thiscol = GetComponent<Collider>();
        targets = rangeobj.GetComponent<range>().targets;
        rangeobj.SendMessage("changeType",(range.Type.Enemy));
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
            for (int i = 0; i <targets.Count; i++)
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
            if (rigd.velocity.y>0)
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

        if (active&&complete)
        {
            Transform Pretarget=null;
            float prepriority=100;
            float priority=100;//ホームまでの距離
            for (int i = 0; i < Currenttargets.Count; i++)
            {
                if (Currenttargets[i] == null)
                {
                    Currenttargets.RemoveAt(i);
                    break;
                }
                float  a= Currenttargets[i].GetComponent<enemy_base>().remaining;
                if (a < priority)
                {
                    RaycastHit hitInfo;
                    bool isHit = Physics.Raycast(shotpoint.transform.position, Currenttargets[i].transform.position - shotpoint.transform.position, out hitInfo, Vector3.Distance(shotpoint.transform.position, Currenttargets[i].transform.position),layerMask);
                    if (isHit && hitInfo.collider.CompareTag("box"))
                    {
                        if (target== null&& a < prepriority)
                        {
                            prepriority = a;
                            Pretarget = Currenttargets[i].transform;
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
                    Vector3 hoseai = shotpoint.transform.position - arm.transform.position;
                    arm.transform.LookAt(Pretarget.position - hoseai);
                    arm.transform.rotation *= Quaternion.Euler(0f, MukiHosei, 0f);
                    arm.transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z);
                }
                active =false;
                return;
            }
            rotater.transform.LookAt(new Vector3(target.position.x, rotater.transform.position.y, target.position.z));
            rotater.transform.rotation *= Quaternion.Euler(0f,MukiHosei, 0f);
            Vector3 hosei = shotpoint.transform.position - arm.transform.position;
            arm.transform.LookAt(target.position-hosei);
            arm.transform.rotation *= Quaternion.Euler(0f, MukiHosei, 0f);
            arm.transform.localRotation = Quaternion.Euler(0, 0, arm.transform.localRotation.eulerAngles.z);
            Debug.Log(arm.transform.localRotation.eulerAngles);
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
        shoting=Instantiate(blockdata.Bullet,shotpoint.transform.position+shotpoint.transform.forward*0.2f,arm.transform.rotation* Quaternion.Euler(0,0,90));
        if (blockdata.Bullet.name == "GunLine")
        {
            Vector3 spot=shotpoint.transform.position;
            if (shotpoints!=null)
            {
                spot = shotpoints[shotcounter].position;
                shotcounter++;
                if (shotcounter == shotpoints.Length) shotcounter = 0;
            }    
            LineRenderer linerend=shoting.GetComponent<LineRenderer>();
            linerend.SetPosition(0,spot+shotpoint.transform.forward*0.2f);
            linerend.SetPosition(1,target.transform.position+ new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)));
            target.SendMessage("Damage",(blockdata.Damage, blockdata.Knockback.y * Vector3.up + blockdata.Knockback.x * rotater.transform.forward + blockdata.Knockback.z * rotater.transform.right));
            GameObject e= Instantiate(blockdata.Shoteffect,spot,shotpoint.transform.rotation*Quaternion.Euler(0,180,0));
            e.transform.localScale = Vector3.one*blockdata.effectsize;
        }
        else
        {
        bulletsc buletsc = shoting.GetComponent<bulletsc>();
        buletsc.ign = thiscol;
        //buletsc.lifetime = canondata.Range/ canondata.BulletSpeed;
        buletsc.lifetime = 5f;
        buletsc.damage.Item1 = blockdata.Damage;
        buletsc.damage.Item2 = blockdata.Knockback.y*Vector3.up+blockdata.Knockback.x*rotater.transform.forward+ blockdata.Knockback.z * rotater.transform.right;
        buletsc.pene = blockdata.penetration;
        Vector3 way = shotpoint.transform.forward;
        shoting.transform.parent= null;
        shoting.transform.rotation = Quaternion.LookRotation(way);
        shoting.GetComponent<Rigidbody>().velocity=(way* blockdata.BulletSpeed);
        }
    }
    void rerange()
    {
        higth = transform.position.y + 0.6f;
        if (subrange[0] != null)
        {
         Destroy(subrange[0]);
         Destroy(subrange[1]);
        }
        List<int> hi= new List<int>();
        switch ((int)higth)
        {
            case 1: hi=new List<int>{2,3}; break;
            case 2: hi=new List<int> {1,3}; break;
            case 3: hi=new List<int> {1,2}; break;
            
        }
        for(int i = 0; i < 2; i++)
        {
            subrange[i] = Instantiate(rangeobj,transform);
            subrange[i].transform.position = new Vector3(transform.position.x,hi[i]-0.5f, transform.position.z);
            submesh[i] = subrange[i].transform.GetChild(0).GetComponent<MeshRenderer>();
            subranges[i] = subrange[i].GetComponent<range>();
            subrange[i].transform.localScale = (Vector3.one - Vector3.up) * blockdata.Range*Mathf.Pow(blockdata.RangeRate,-hi[i] + higth) + Vector3.up * 0.5f;
        }
    }
}
