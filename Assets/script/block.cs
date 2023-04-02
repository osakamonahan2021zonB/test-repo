using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    public float hp;
    manager_block manager_Block;
    public Collider overlap;
    public bool load = false;
    protected Renderer[] _renderer=null;
    [Space (20)]
    public Renderer[] rendthings;
    protected Material original;
    [Space(20)]
    public bool complete;
    public bool displayrange=false;
    public blockdata blockdata;
    [Space(20)]
    public GameObject invinci;
    // Start is called before the first frame update
    virtual protected void Start()
	{
        manager_Block = GameObject.Find("Manager_tile").GetComponent<manager_block>();
        if(blockdata!=null)hp = blockdata.HP;
        if (!complete) vial();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (hp <= 0)
        {
            manager_Block.addhight(new Vector2Int((int)transform.position.x, (int)transform.position.z),Mathf.CeilToInt(transform.position.y),-1);
            if (manager_Block.tiles1[new Vector2Int((int)transform.position.x, (int)transform.position.z)]> 0) Instantiate(invinci).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (complete) return;
        if (other.gameObject.layer == 2||other.gameObject.layer==9) return;
        if (overlap == null)
        {
            overlap = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (overlap != null)
        {
            if (other == overlap) overlap = null;
        }
    }
    void vial()
    {
        load = true;
        displayrange = true;
        if (_renderer!=null)foreach (Renderer r in _renderer)
        { r.enabled = true; }
    }
    public void changecoler(bool can)
    {
        if (_renderer == null) {
            if (rendthings != null)
            {
                _renderer = rendthings;
            }
            else
            {
                _renderer[0] = GetComponent<Renderer>();
            }
            original = new Material(_renderer[0].material);
            foreach (Renderer r in _renderer)
            {r.material.EnableKeyword("_EMISSION"); }
        }
        if (can)
        {
            //キーワードの有効化を忘れずに
            foreach (Renderer r in _renderer)
            {r.material.SetColor("_EmissionColor", new Color(0.3f, 0.3f, 0.3f)); }
             //赤色に光らせ
        }
        else
        {
                foreach (Renderer r in _renderer)
                { r.material.SetColor("_EmissionColor", new Color(0.7f, 0f, 0f)); }
        }
    }
    public void place()
    {
        displayrange = false;
        complete = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;
        if (_renderer == null)
        {
            if (rendthings != null)
            {
                _renderer = rendthings;
            }
            else
            {
                _renderer[0] = GetComponent<Renderer>();
            }
            original = new Material(_renderer[0].material);
            foreach (Renderer r in _renderer)
            { r.material.EnableKeyword("_EMISSION"); }
        }
        foreach (Renderer r in _renderer)
        { r.material = original; }        
	}
    public void destory()
    {
        manager_Block.addhight(new Vector2Int((int)transform.position.x, (int)transform.position.z), Mathf.CeilToInt(transform.position.y), -1);
        if (manager_Block.tiles1[new Vector2Int((int)transform.position.x, (int)transform.position.z)] > 0) Instantiate(invinci).transform.position = transform.position;
        Destroy(gameObject);
    }
    void dischange(bool a)
    {
        displayrange = a;
    }
    void Damage(float damage)
    {
        hp -= damage;
        GameObject e = Instantiate(blockdata.HitEffect, transform.position, Quaternion.identity);
        e.transform.localScale = Vector3.one * blockdata.effectSize;
        foreach (Renderer r in _renderer)
        {
            r.material.EnableKeyword("_EMISSION");
            r.material.SetColor("_EmissionColor", new Color(0.9f, 0f, 0f));
        }
        Invoke("retruncolor",0.1f);
    }
    void retruncolor()
    {
        foreach (Renderer r in _renderer)
        { r.material = original; }
    }
    public void undo()
    {
        if (blockdata != null) Camera.main.gameObject.SendMessage("addcost",Mathf.Ceil(blockdata.cost/blockdata.HP*hp/0.7f));
        manager_Block.addhight(new Vector2Int((int)transform.position.x, (int)transform.position.z), Mathf.CeilToInt(transform.position.y), -1);
        if (manager_Block.tiles1[new Vector2Int((int)transform.position.x, (int)transform.position.z)] > 0) Instantiate(invinci).transform.position = transform.position;
        Destroy(gameObject);
    }
    int Rounding(float num)
    {
        float dec = num - Mathf.FloorToInt(num);
        int intege = Mathf.FloorToInt(dec * 10);

        if (intege >= 5)
        {
            return Mathf.CeilToInt(num);
        }
        else
        {
            return Mathf.FloorToInt(num);
        }
    }
    public void levelup(int number)
    {
        GameObject g=null;
        if (blockdata != null) g = blockdata.levelUps[number].Nextobj;
        GameObject s= Instantiate(g);
        Instantiate(blockdata.levelEffect, transform.position, Quaternion.identity);
        s.transform.position = transform.position;
        s.GetComponent<block>().place();
        Destroy(gameObject);
    }
}
