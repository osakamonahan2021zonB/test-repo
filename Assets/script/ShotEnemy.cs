using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEnemy :enemy_base
{
    [SerializeField] GameObject arm;
    [SerializeField] Animator burrelanimator;
    [SerializeField] GameObject shotpoint;
    public override void Start()
    {
        base.Start();
        rangeobj.transform.localScale = (Vector3.one - Vector3.up) * enemydata.Range + Vector3.up * 0.5f;
        targets = rangeobj.GetComponent<range>().targets;
    }
    public override void Update()
    {
        
        base.Update();
        if (deceleration == 0.6f) deceleration = 1;
    }
    public override void Attack()
    {
        arm.transform.LookAt(target.transform);
        if (colltime < 0)
        {
            burrelanimator.SetTrigger("shot");
            GameObject shoting;
            shoting = Instantiate(base.enemydata.Bullet, shotpoint.transform.position + shotpoint.transform.forward * 0.2f, Quaternion.identity);
            if (base.enemydata.Bullet.name == "GunLine")
            {
                LineRenderer linerend = shoting.GetComponent<LineRenderer>();
                linerend.SetPosition(0, shotpoint.transform.position + shotpoint.transform.forward * 0.2f);
                linerend.SetPosition(1, target.transform.position);
                target.SendMessage("Damage",base.enemydata.Damage);
                GameObject e = Instantiate(enemydata.Shoteffect, shotpoint.transform.position, shotpoint.transform.rotation * Quaternion.Euler(0, 180, 0));
                e.transform.localScale = Vector3.one * enemydata.Shotsize;
            }
            colltime = base.enemydata.AtackeRate;
        }
        deceleration = 0.6f;
    }
}
