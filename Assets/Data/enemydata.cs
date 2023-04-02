using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Enemytype")]
public class enemydata : ScriptableObject
{
    public int cost;
    public float HP;
    [Range(1,3)]
    public int hight=1;
    public float Damage;
    public bool norange;
    public float Range;
    public float AtackeRate;
    public GameObject Shoteffect;
    public float Shotsize;
    public GameObject Bullet;
    public float Speed;
    public Sprite Icon;
    public GameObject HitEffect;
    public float effectSize=1.2f;
}
