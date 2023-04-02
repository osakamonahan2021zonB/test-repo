using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class manager_block : MonoBehaviour
{

    public static manager_block MB { get; private set; } = null;
    public Dictionary<Vector2Int, int> tiles1 = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> tiles2 = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> tiles3 = new Dictionary<Vector2Int, int>();
    public Transform[] addstart;
    List<Vector2Int> addstartspot=new List<Vector2Int>();
    public Transform endspot;
    public GameObject tile;
    //移動可能の方向（上下左右）
    Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down , Vector2Int.left, Vector2Int.right,new Vector2Int(1,1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
    public (int Hight, int BlockAp)[] wayvalues = new (int Hight, int BlockAp)[3];
   public Dictionary<Vector2Int,List<Vector2Int>> waylist1 = new Dictionary<Vector2Int, List<Vector2Int>>();
   public Dictionary<Vector2Int,List<Vector2Int>> waylist2 = new Dictionary<Vector2Int, List<Vector2Int>>();
   public Dictionary<Vector2Int,List<Vector2Int>> waylist3 = new Dictionary<Vector2Int, List<Vector2Int>>();
  public  LineRenderer[] liner=new LineRenderer[3];

    private void Awake()
    {
        if (MB == null)
        {
            MB = this;
            return;
        }
        Destroy(this);
    }
    private void OnDestroy()
    {
        MB = null;
    }
    /// <summary>
    /// 経路系メソッド
    /// </summary>
    /// <param name="map">マップ</param>>
    /// <param name="start">スタート</param>>
    /// <param name="end">遠藤</param>>
    void Start()
    {
        wayvalues[0] = (1, 1);
        wayvalues[1] = (2, 1);
        wayvalues[2] = (3, 0);
        for (int i=0;i<tile.transform.childCount; i++)
        {
            Transform achi=tile.transform.GetChild(i);
            Vector2Int achiv = new Vector2Int((int)achi.position.x, (int)achi.position.z);
            if(tiles1.ContainsKey(achiv))
            {
                if (tiles1[achiv] == 0)
                {
                    tiles1[achiv] = -1;
                }
                else
                {
                    if (tiles2[achiv] == 0)
                    {
                        tiles2[achiv] = -1;
                    }
                    else
                    {
                        if (tiles3[achiv] == 0)
                        {
                            tiles3[achiv] = -1;
                        }
                    }
                }
            }
            else
            {
              tiles1.Add(achiv, 0);
              tiles2.Add(achiv, 0);
              tiles3.Add(achiv, 0);
            }
        }
        waylist1.Add(Vector2Int.zero, Find(new Vector2Int(0, 0), new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)),wayvalues[0].Hight));
        waylist2.Add(Vector2Int.zero, Find(new Vector2Int(0, 0), new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)), wayvalues[1].Hight));
        waylist3.Add(Vector2Int.zero, Find(new Vector2Int(0, 0), new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)), wayvalues[2].Hight));
        if (addstart.Length>0)
        {
            for (int i = 0; i < addstart.Length; i++)
            {
                addstartspot.Add(new Vector2Int((int)addstart[i].position.x, (int)addstart[i].position.z));
                waylist1.Add(addstartspot[i],Find(addstartspot[i], new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)), wayvalues[0].Hight));
                waylist2.Add(addstartspot[i], Find(addstartspot[i], new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)),wayvalues[1].Hight));
                waylist3.Add(addstartspot[i], Find(addstartspot[i], new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)), wayvalues[2].Hight));
            }
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown("f"))
        {
            Setwaylistvalue(2,1,1);
        }
    }

    public void Setwaylistvalue(int number, int BlockAp,int Hight)
    {
        if (wayvalues[number-1].Hight==Hight&&wayvalues[number-1].BlockAp == BlockAp)
        {

        }
        else
        {
            wayvalues[number-1].Hight = Hight;
            wayvalues[number-1].BlockAp = BlockAp;
            reloadpath(number);
        }
        for(int i = 0; i < 3; i++)
        {
            Debug.Log("wayvalues"+i+";"+wayvalues[i].Hight);
        }
    }

    public List<Vector2Int> wayexe(Vector2Int vector2,int Type)
    {
        switch (Type)
        {
            case 1:
                if (!waylist1.ContainsKey(vector2))
                {
                    waylist1.Add(vector2, Find(vector2, new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)), 1));
                }
                return waylist1[vector2];
            case 2:
                if (!waylist2.ContainsKey(vector2))
                {
                    waylist2.Add(vector2, Find(vector2, new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)), 2));
                }
                return waylist2[vector2];
            case 3:
                if (!waylist3.ContainsKey(vector2))
                {
                    waylist3.Add(vector2, Find(vector2, new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z)), 3));
                }
                return waylist3[vector2];
        }
        return null;
    }

    // Update is called once per frame
     List<Vector2Int> Find(Vector2Int strat,Vector2Int end,int waynumber,float blockAppropriate=1)
    {
        //TODO LIST
        Dictionary<Vector2Int, int> thistiles = null;
        switch (wayvalues[waynumber-1].Hight)
        {
            case 1: thistiles = tiles1; break;
            case 2: thistiles = tiles2; break;
            case 3: thistiles = tiles3; break;
        }
        if (!thistiles.ContainsKey(end))return null;
        Queue<(Vector2Int, int)> queue = new Queue<(Vector2Int, int)>();
        var latespot =new Dictionary<Vector2Int,int>();
        //調査済みList+ひとつ前
        var visited = new Dictionary<Vector2Int,Vector2Int>();

        queue.Enqueue((strat,0));
        visited.Add(strat, new Vector2Int(-1, -1));
        //queeu enpty >noway
        while (queue.Count > 0||latespot.Count>0)
        {//取り出し
            if (queue.Count == 0)
            {
                int i =-1;
                foreach(var a  in latespot.Values)
                {
                    if (i == -1) i = a;
                    if (i > a) i = a;
                }
                var vector2Ints = latespot.FirstOrDefault(c => c.Value ==i);
                queue.Enqueue((vector2Ints.Key,i));
                latespot.Remove(vector2Ints.Key);
            }
            (Vector2Int pos, int count) current = queue.Dequeue();
            if (latespot.ContainsValue(current.count))
            {
                var vector2Ints=latespot.FirstOrDefault(c => c.Value== current.count);
                queue.Enqueue((vector2Ints.Key, current.count));
                latespot.Remove(vector2Ints.Key);

            }
            //タイル探し
            
            foreach (var dir in dirs)
            {
                (Vector2Int pos, int count) next = (current.pos + dir, current.count + 1);
                if (visited.ContainsKey(next.pos)) continue;
                if (!thistiles.ContainsKey(next.pos)) continue;   
                {
                    if (thistiles.ContainsKey(next.pos))
                    {
                        if (thistiles[next.pos] == -1)
                        {
                            continue;
                        }
                        if (thistiles[next.pos] > 0)
                        {
                        if (!latespot.ContainsKey(next.pos)&&dir.magnitude == 1){ latespot.Add(next.pos,(current.count+Mathf.CeilToInt(thistiles[next.pos]*blockAppropriate)));visited.Add(next.pos, current.pos);}
                        continue;
                        }
                    }
                }
                if (dir.magnitude > 1)
                {
                       if (!thistiles.ContainsKey(current.pos+new Vector2Int(dir.x,0))) continue;
                       if (thistiles[current.pos + new Vector2Int(dir.x, 0)] != 0)
                       {
                           continue;
                       }
                       if (!thistiles.ContainsKey(current.pos + new Vector2Int(0,dir.y))) continue;
                       if (thistiles[current.pos + new Vector2Int(0,dir.y)]!=0)
                       {
                           continue;
                       }
                }
                //条件終了
                queue.Enqueue(next);
                visited.Add(next.pos,current.pos);
                //next==Goal
                if (next.pos == end)
                {
                    return showpath(visited,end,strat,waynumber);
                }
            }


        }
        Debug.LogError("nopath");
        return null;
    }

    public void addhight(Vector2Int vector2,int hight,int Hp)
    {
        if (hight >= 2)
        {
            if (tiles1[vector2] == 0) { hight = 1; }
            else
            {
                if (tiles2[vector2] == 0 && hight == 3) hight = 2;
            }
        }
        if (Hp == -1) Hp = 0;
        switch (hight)
        {
           case 1: tiles1[vector2] = Hp; break;
           case 2: tiles2[vector2] = Hp; break;
           case 3: tiles3[vector2] = Hp; break;
        }

        for (int i = 0; i < 3; i++)
        {
            if (wayvalues[i].Hight == hight)
            {
                reloadpath(i+1);
            }
        }
    }
    public void reloadpath(int waynumber)
    {
        Vector2Int Roundedend = new Vector2Int(Mathf.RoundToInt(endspot.position.x), Mathf.RoundToInt(endspot.position.z));
        switch (waynumber)
        {
            case 1:
                waylist1.Clear();
                waylist1.Add(Vector2Int.zero, Find(new Vector2Int(0, 0),Roundedend, 1,wayvalues[0].BlockAp));
                for (int a = 0; a < addstartspot.Count; a++)
                {
                    waylist1.Add(Vector2Int.zero, Find(addstartspot[a],Roundedend,1,wayvalues[0].BlockAp));
                }
                break;
            case 2:
                waylist2.Clear();
                waylist2.Add(Vector2Int.zero, Find(new Vector2Int(0, 0),Roundedend,2,wayvalues[1].BlockAp));
                for (int a = 0; a < addstartspot.Count; a++)
                {
                    waylist2.Add(Vector2Int.zero, Find(addstartspot[a],Roundedend,wayvalues[1].Hight,wayvalues[1].BlockAp));
                }
                break;
            case 3:
                waylist3.Clear();
                waylist3.Add(Vector2Int.zero, Find(new Vector2Int(0, 0), Roundedend, 3,wayvalues[2].BlockAp));
                for (int a = 0; a < addstartspot.Count; a++)
                {
                    waylist3.Add(Vector2Int.zero, Find(addstartspot[a],Roundedend,3,wayvalues[2].BlockAp));
                }
                break;
        }
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("enemy"))
        {
            e.SendMessage("changeway",waynumber);
        }
    }
    public Vector2 Tellway(Vector2Int from, int higth)
    {
        Debug.Log("Tellway");
        List<Vector2Int> a = wayexe(from,higth);
        return a[0] - from;
    }
    /// <summary>
    /// ゴールから遡るpath
    /// </summary>
    /// <param name="visited"></param>
    /// <param name="goal"></param>
    List<Vector2Int> showpath(Dictionary<Vector2Int, Vector2Int> visited, Vector2Int goal,Vector2Int start,int Waumumber)
    {
        Debug.Log("shou" + Waumumber + "" + wayvalues[Waumumber - 1].Hight);
        int Higth =wayvalues[Waumumber-1].Hight;
        var path = new List<Vector2Int>();
        Vector2Int current = goal;
        Vector2 way =new Vector2();
        int i=0;
        while (true)
        {
            Vector2Int previous = visited[current];
            if (previous.x == -1) break;

            if (way != current - previous)
            {
                path.Add(current);          
                way = current - previous;
            }
            current = previous;
        }
        if (path != null)
        {
            path.Reverse();
            if (start==Vector2Int.zero)
            {
                liner[Waumumber-1].positionCount=path.Count+1;
                liner[Waumumber - 1].SetPosition(0,Vector3.up*(-0.5f+Higth+Waumumber*0.07f));
                foreach(Vector2 vector2 in path)
                {
                    i++;
                    liner[Waumumber - 1].SetPosition(i,new Vector3(vector2.x,-0.5f+Higth + Waumumber * 0.07f, vector2.y));
                }
            }
            int num = addstartspot!=null?addstartspot.IndexOf(start):-1;
            if (num >= 0)
            {
                LineRenderer addline=addstart[num].GetChild(Waumumber-1).GetComponent<LineRenderer>();
                addline.positionCount = path.Count + 1;
                addline.SetPosition(0,new Vector3(start.x,-0.5f +Higth,start.y));
                foreach (Vector2 vector2 in path)
                {
                    i++;
                    addline.SetPosition(i, new Vector3(vector2.x, -0.5f + Higth + Waumumber * 0.07f, vector2.y));
                }
            }
            return path;
        }
        else
        {
            return null;
        }
    }

}
