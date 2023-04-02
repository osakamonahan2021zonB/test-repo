using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_base : MonoBehaviour
{
   public float HP;
    public float remaining=100;
   public List<Vector2Int> road;
   public Vector2Int waypoint= Vector2Int.zero;
    public List<GameObject> targets;
    public GameObject target;
    public bool norange = false;
   public GameObject rangeobj;
    Rigidbody rigid;
    public enemydata enemydata;
    Vector2 pre;
    public float colltime = 0;
   public float reservroad=0;
    int roadcount=0;
    bool relook;
    public float slide = 0f;
    Vector2 add;
    [Space(20)]
    public bool dumy;
    public float deceleration=1;
    int waynumber;
    // Start is called before the first frame update
    public virtual void Start()
    {
        if (dumy) return;
        HP = enemydata.HP;
        norange = enemydata.norange;
        Vector2Int spawnpoint = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        for(int i=0;i<GM.gM.spawns.Count;i++)
        {
            if(GM.gM.spawns[i].enemy==gameObject)
            {
                waynumber=i;
                i=1000;
            }
        }

        switch(waynumber+1)
        {
            case 1:road = manager_block.MB.waylist1[spawnpoint];break;
            case 2:road = manager_block.MB.waylist2[spawnpoint];break;
            case 3:road = manager_block.MB.waylist3[spawnpoint];break;
        }
        if (!norange)
        {
         rangeobj = transform.Find("Range").gameObject;
         rangeobj.SendMessage("changeType",range.Type.Block);
         targets = rangeobj.GetComponent<range>().targets;
        }
        rigid = GetComponent<Rigidbody>();  
         for (int i = 1; i < road.Count; i++)
         {
            reservroad += (Mathf.Abs(road[i].x - road[i - 1].x)+Mathf.Abs(road[i].y - road[i - 1].y));
         }
         waypoint = road[0];
        add = Quaternion.Euler(0, 0, 90) * new Vector2(road[roadcount].x - transform.position.x, road[roadcount].y - transform.position.y).normalized * -slide;
        transform.LookAt(new Vector3(waypoint.x + add.x, transform.position.y, waypoint.y + add.y));
        roadcount++;
        colltime = enemydata.AtackeRate;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (dumy) return;
        if(colltime>0) colltime -= Time.deltaTime;
        if (transform.position.y < -1f)
        {
            Destroy(gameObject);
            Camera.main.gameObject.SendMessage("addcost", enemydata.cost);
        }
        if (HP <= 0)
        {
            Destroy(gameObject);
            Camera.main.gameObject.SendMessage("addcost",enemydata.cost);
        }
        float shortestDistance = Mathf.Infinity;
        for (int i = 0; i < targets.Count; i++)
        {   if (targets[i] == null) { targets.Remove(targets[i]); break; }
            float distance = Vector3.Distance(transform.position, targets[i].transform.position);
            if (distance < shortestDistance)
            {
                target = targets[i];
                shortestDistance = distance;
            }
        }
        if (target!=null)
        {         
            Attack();
        }
        if (waypoint != Vector2Int.zero)
        {
            //transform.LookAt(new Vector3(waypoint.x,transform.position.y,waypoint.y));
          //  remaining -= (transform.position.x - pre.x) + (transform.position.z - pre.y);
            Vector3 way = new Vector3(-transform.position.x + waypoint.x+add.x, 0,- transform.position.z + waypoint.y+add.y);
            remaining = reservroad + Mathf.Abs(way.x) + Mathf.Abs(way.z);
            if (way.magnitude < 0.3f)
            {
                waypoint = Vector2Int.zero;
            }
            if (rigid.velocity.magnitude < 0.1f) {
                if (relook) { changeway(waynumber); relook = false; }
                transform.position += transform.forward * enemydata.Speed * Time.deltaTime* deceleration;
            }
            else
            {
                relook = true;
            }
        }
        else
        {
            if (roadcount == road.Count && norange&&!rigid.useGravity)
            {
                rigid.useGravity = true;
                transform.rotation *= Quaternion.Euler(new Vector3(30, 0, 0));
            }
            if (roadcount<road.Count)
            {
                reservroad = 0;
                for (int i = 1+roadcount; i < road.Count; i++)
                {
                    reservroad += (Mathf.Abs(road[i].x - road[i - 1].x) + Mathf.Abs(road[i].y - road[i - 1].y));
                }

                waypoint =road[roadcount];      
                if (roadcount - 1 > 0) { add = Quaternion.Euler(0, 0, 90) * new Vector2(road[roadcount].x - road[roadcount - 1].x, road[roadcount].y - road[roadcount - 1].y).normalized; }
                else { add = Quaternion.Euler(0, 0, 90) * new Vector2(road[roadcount].x, road[roadcount].y).normalized; }
                if (road.Count > roadcount + 1)
                {
                   Vector2 da =Quaternion.Euler(0, 0, 90) * new Vector2(road[roadcount+1].x - road[roadcount].x, road[roadcount+1].y - road[roadcount].y).normalized;
                    add = new Vector2(add.x+da.x, add.y + da.y).normalized*slide;
                }
                else
                {
                    add *= slide;
                }
                transform.LookAt(new Vector3(waypoint.x+add.x, transform.position.y, waypoint.y+add.y));
               // Debug.Log("lookpoint"+add+" "+waypoint+" "+transform.position);
                roadcount++;
            }
        }
    }
    public virtual void Attack()
    { 
        if (colltime < 0)
        {
            foreach(GameObject g in targets)
            {
                g.SendMessage("Damage",enemydata.Damage);
            }
            colltime = enemydata.AtackeRate;
        }
    }
    void doslide(float a)
    {
        slide = a;
    }
    void changeway(int hi)
    {
        if (hi != enemydata.hight) return;
        waypoint = Vector2Int.zero;
        road = manager_block.MB.wayexe(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z)),hi);
        roadcount=0;
    }
    public void Damage((float,Vector3)damage)
    {
        HP -= damage.Item1;
        GameObject e= Instantiate(enemydata.HitEffect,transform.position,Quaternion.identity);
        e.transform.localScale = Vector3.one * enemydata.effectSize;
        if (damage.Item2!=Vector3.zero)
        {
            rigid.AddForce(damage.Item2*3,ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (dumy) return;
        if (collision.gameObject.tag == "box")
        {
            Destroy(gameObject);
            collision.gameObject.SendMessage("Damage", enemydata.Damage);
        }
    }
}
