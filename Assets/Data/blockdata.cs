using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Block/BlockData")]
public class blockdata : ScriptableObject
{
    [Range(1,5)]
    public int Grade = 1;
    public int cost;
    public float HP;
    public Sprite Icon;
    public bool CanTdestoroy=false;
    [System.Serializable]
    public struct LevelUp
    {
        public string name;
        public GameObject Nextobj;
    }
    public List<LevelUp> levelUps;
    public GameObject levelEffect;
    public enum BlockType
    {
        Wall,
        Canon,
        Attacker,
        Unique,
    }
    public BlockType blockType;
    public enum AttackType
    {
        None,
        Stab,
        Slash,
        Shoot,
    }       
    public AttackType attackType; 
    public float Damage;
    public int penetration=0;
    public Vector3 Knockback;
    public float Range;
    public float RangeRate;
    public float AttackRate;
    public GameObject Bullet;
    public GameObject Shoteffect;
    public float effectsize = 0.7f;
    public float BulletSpeed;
    public string explain;
    public GameObject HitEffect;
    public float effectSize = 1.2f;

}
