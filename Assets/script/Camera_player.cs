using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Camera_player : MonoBehaviour
{
     private Vector3 velocity;              // 移動方向
    [SerializeField] private float moveSpeed = 5.0f;        // 移動速度
    Camera cam;
    [SerializeField] float sensitiveRotate = 0.8f;
    public bool reversalrotateX =false;
    public bool reversalrotateY =false;
    public float[] MoveLinY=new float[2];
    public float[] MoveLinX=new float[2];
    public float[] MoveLinZ=new float[2];
    [SerializeField] bool canplace;
    public GameObject placeObj;
    public GameObject apperDisplay;
    public GameObject informObj;
    public Button DelateUI;
    public Button MargeUI;
    public bool canmarge;
    public List<GameObject> canmargeobjs=new List<GameObject>();
    manager_block manager_Block;
    public int energy;
    public int cost;
    Text energydis;
    block block;
    [SerializeField] LayerMask activelayer;
    public bool active;
    Vector3[] rayway= {new Vector3(1,0,0), new Vector3(0, 1, 0) , new Vector3(0, 0, 1) };
    private void Start()
    {
        cam = this.GetComponent<Camera>();
        energydis = GameObject.Find("CostText").GetComponent<Text>();
        manager_Block = GameObject.Find("Manager_tile").GetComponent<manager_block>();
        DelateUI.interactable = false;
        MargeUI.interactable = false;
    }
    void Update()
    {
        if(Input.GetKeyDown("f"))
        {
            energy += 1000;
        }
        energydis.text = "エネルギー:" + energy;
        #region 移動系
            if (Input.GetMouseButton(1))
            {
                float rotateX = -Input.GetAxis("Mouse X") * sensitiveRotate * (reversalrotateX ? 1 : -1);
                float rotateY = -Input.GetAxis("Mouse Y") * -sensitiveRotate * (reversalrotateY ? 1 : -1);
                if ((rotateY + transform.eulerAngles.x >= 90&&rotateY+transform.eulerAngles.x<=180 )||( rotateY + transform.eulerAngles.x <= 270 && rotateY + transform.eulerAngles.x >= 180)) {rotateY = 0; }
                cam.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + rotateY, transform.rotation.eulerAngles.y + rotateX, 0.0f);
            }
        Vector3 a;
            if(Input.GetKey("w")|| Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("s"))
            {
            float moveX=0;
            float moveY=0;
            if ((Input.GetKey("w")))moveY+=0.7f;
            if ((Input.GetKey("a")))moveX-=0.7f;
            if ((Input.GetKey("s")))moveY-= 0.7f;
            if ((Input.GetKey("d")))moveX+= 0.7f;
            a = moveY * transform.up + moveX * transform.right;
        }
        else
        {
           if (Input.GetMouseButton(2))
           {
                float moveX =- Input.GetAxis("Mouse X");
                float moveY = -Input.GetAxis("Mouse Y");
                 a = moveY * transform.up + moveX * transform.right;
            // x軸方向の移動制
           }
           else
           {
                a = Input.GetAxis("Mouse ScrollWheel")*transform.forward* moveSpeed*3;
           }
        }
        if (transform.position.x >= MoveLinX[0] && a.x > 0)
        {
            a.x = 0;
            a *= 0.9f;
        }
        else if (transform.position.x <= MoveLinX[1] && a.x < 0)
        {
            a.x = 0; a *= 0.9f;
        }

        // z軸方向の移動制限
        if (transform.position.z >= MoveLinZ[0] && a.z > 0)
        {
            a.z = 0; a *= 0.9f;
        }
        else if (transform.position.z <= MoveLinZ[1] && a.z < 0)
        {
            a.z = 0; a *= 0.9f;
        }

        // y軸方向の移動制限
        if (transform.position.y >= MoveLinY[0] && a.y > 0)
        {
            a.y = 0; a *= 0.9f;
        }
        else if (transform.position.y <= MoveLinY[1] && a.y < 0)
        {
            a.y = 0; a *= 0.9f;
        }
        velocity = a;
        // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
        velocity = velocity* moveSpeed*0.02f;
            // いずれかの方向に移動している場合
            if (velocity.magnitude > 0)
            {
                transform.position += velocity;
            }
        #endregion
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
       if (placeObj == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
             if (Physics.Raycast(ray, out hit, 20.0f, activelayer.value)&& hit.collider.gameObject.tag == "box")
             {
                    Debug.Log(hit.collider.name);
                    if (canmarge)
                    {
                        GameObject g = hit.collider.gameObject;
                        if (canmargeobjs.Contains(g)&& informObj !=g)
                        {
                                g.SendMessage("destory");
                                informObj.SendMessage("levelup",GetIndexOfLevelUp(informObj.GetComponent<block>().blockdata, g.name.Replace("(Clone)", "")));
                                informObj = null;
                                foreach(GameObject game in canmargeobjs)
                                {
                                    if (game.GetComponent<Outline>() != null)
                                    {
                                        Destroy(game.GetComponent<Outline>());
                                    }
                                }
                                canmargeobjs.Clear();
                                canmarge = false;
                        }
                    }
                    else
                    {
                     if (informObj != hit.collider.gameObject) {
                         if (informObj != null)
                         {
                             Destroy(informObj.GetComponent<Outline>());
                             informObj.SendMessage("dischange", false);
                         }
                         informObj = hit.collider.gameObject;
                         var outline = informObj.AddComponent<Outline>();
                         outline.OutlineMode = Outline.Mode.OutlineAll;
                         outline.OutlineColor = Color.yellow;
                         outline.OutlineWidth = 6f;
                         informObj.SendMessage("dischange",true);
                         DelateUI.interactable = true;
                         MargeUI.interactable = true;
                     }
                     else
                     {
                         Destroy(informObj.GetComponent<Outline>());
                         informObj.SendMessage("dischange", false);
                         informObj = null;
                         DelateUI.interactable = false;
                         MargeUI.interactable = false;
                     }
                    }
             }
             else
             {
                    if (canmarge)
                    {
                        canmarge = false;
                        if (canmargeobjs != null)
                        {
                         foreach (GameObject game in canmargeobjs)
                         {
                             if (game.GetComponent<Outline>() != null)
                             {
                                 Destroy(game.GetComponent<Outline>());
                             }
                         }
                         canmargeobjs.Clear();
                        }
                    }
                    else
                    {
                        if (EventSystem.current.IsPointerOverGameObject())
                         {

                         }
                     else if (informObj != null)
                      {
                         Destroy(informObj.GetComponent<Outline>());
                         informObj.SendMessage("dischange", false);
                         informObj = null;
                         DelateUI.interactable = false;
                         MargeUI.interactable = false;
                      }                   
                    }                  
             }
            }
        }
       else
       {
            if (informObj != null) {
                Destroy(informObj.GetComponent<Outline>());
                informObj.SendMessage("dischange", false);
                informObj = null;
                DelateUI.interactable = false;
                MargeUI.interactable = false;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (apperDisplay != null)
                {
                    Destroy(apperDisplay);
                    apperDisplay = null;
                    canplace = false;
                }
                return;
            }
            if (Physics.Raycast(ray, out hit, 20.0f, activelayer.value))
            {
                checkdisplayer(hit.point,hit.collider);
            }
            else
            {
                if (apperDisplay != null)
                {
                    Destroy(apperDisplay);
                    apperDisplay = null;
                    canplace = false;
                }
            }//緑のディスプレイ
            block b= placeObj.GetComponent<block>();
            if (b.blockdata != null)cost= b.blockdata.cost;
            if (apperDisplay != null)
            {
                if (block == null) block = apperDisplay.GetComponent<block>();
                if (energy < cost)
                {
                    canplace = false;
                }
                else
                {
                        canplace = (block.overlap == null);
                    if (!manager_Block.tiles1.ContainsKey(new Vector2Int((int)apperDisplay.transform.position.x, (int)apperDisplay.transform.position.z))
                        ||new Vector2(apperDisplay.transform.position.x,apperDisplay.transform.position.z)==Vector2.zero
                        ||new Vector2(apperDisplay.transform.position.x, apperDisplay.transform.position.z) == new Vector2(manager_Block.endspot.position.x,manager_Block.endspot.position.z))
                    {
                        canplace = false;
                        
                    }
                }
                block.changecoler(canplace);
            }
            //if (canplace) Debug.Log("せち");
            if (Input.GetKeyDown("q")) {apperDisplay.transform.Rotate(0,-90,0); }
            if (Input.GetKeyDown("e")) {apperDisplay.transform.Rotate(0,90,0); }
            if (Input.GetMouseButtonDown(0))
            {
                if (apperDisplay == null) placeObj = null;
                if (canplace)
                {
                 placeblock(apperDisplay.transform.position, placeObj);
                 energy -= cost;
                }
            }
        }
    }

    public Vector3 AttractToPlayer(Transform[] childTransforms)
    {
        // 子オブジェクトの平均位置を計算
        Vector3 averagePosition = Vector3.zero;
        for (int i = 0; i < childTransforms.Length; i++)
        {
            averagePosition += childTransforms[i].position;
        }
        return  averagePosition /= childTransforms.Length;
    }
    void addcost(int a)
    {
        energy += a;
    }
   public void undo()
    {
        informObj.SendMessage("undo");
        DelateUI.interactable = false;
    }
    void checkdisplayer(Vector3 tarpos,Collider c)
    {
        if (c.tag== "box")
        {
            Vector3 way= tarpos - c.gameObject.transform.position-c.GetComponent<BoxCollider>().center;
            if(way.y<Mathf.Abs(way.x)|| way.y <Mathf.Abs(way.z))
            {
                tarpos +=Mathf.Abs(way.x) >= Mathf.Abs(way.z)?Vector3.right*way.x:Vector3.forward * way.z;
                tarpos.y = Mathf.Floor(tarpos.y);

            }
            //tarpos -=new Vector3(transform.forward.x,0, transform.forward.z)*0.1f;
        }
        if (apperDisplay == null)
        {
            if (masuka(tarpos).y+ (placeObj.transform.lossyScale.y) / 2 > 3) return;
            apperDisplay =Instantiate(placeObj,masuka(tarpos)+Vector3.up* (placeObj.transform.lossyScale.y) / 2, Quaternion.identity);
            block = apperDisplay.GetComponent<block>();
            canplace = false;
        }
        else
        {
            if (masuka(tarpos).y + (placeObj.transform.lossyScale.y) / 2 > 3) return;
            if (masuka(tarpos).x != apperDisplay.transform.position.x|| masuka(tarpos).z != apperDisplay.transform.position.z || masuka(tarpos).y != masuka(apperDisplay.transform.position).y-1)
            {
                Destroy(apperDisplay);
                apperDisplay = Instantiate(placeObj, masuka(tarpos) + Vector3.up * (placeObj.transform.lossyScale.y) / 2, Quaternion.identity);
                block = apperDisplay.GetComponent<block>();
                canplace = false;
            }
        }
        
    }
    public void marge()
    {

        if (canmargeobjs != null)
        {
            foreach (GameObject game in canmargeobjs)
            {
                if (game.GetComponent<Outline>() != null)
                {
                    Destroy(game.GetComponent<Outline>());
                }
            }
            canmargeobjs.Clear();
        }
        foreach(Vector3 v3 in rayway)
        {
            foreach(RaycastHit hit in Physics.RaycastAll(informObj.transform.position-v3*1.5f,v3,2.1f,1<<0))
            {
                GameObject g = hit.collider.gameObject;
                string st = g.name.Replace("(Clone)", "");
                int aa = GetIndexOfLevelUp(informObj.GetComponent<block>().blockdata,st);
                if (g != informObj&&aa!=-1)
                {
                  var outline = g.AddComponent<Outline>();
                  outline.OutlineMode = Outline.Mode.OutlineAll;
                  outline.OutlineColor = Color.cyan;
                  outline.OutlineWidth = 5f;
                  canmargeobjs.Add(g);
                } 
            }
        }
        canmarge = canmargeobjs.Count > 0;
    }
    public int GetIndexOfLevelUp(blockdata blockdata,string name)
    {
        for (int i = 0; i < blockdata.levelUps.Count; i++)
        {
            if (blockdata.levelUps[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }

    void placeblock(Vector3 tarpos,GameObject obj)
    {
        block.place();
        apperDisplay.layer = 0;
        apperDisplay.GetComponent<Rigidbody>().isKinematic = false;
        apperDisplay.GetComponent<Collider>().isTrigger = false;
        if (masuka(tarpos).y + (placeObj.transform.lossyScale.y) / 2 < 3)
        {
            apperDisplay = Instantiate(placeObj, masuka(tarpos) + Vector3.up * (placeObj.transform.lossyScale.y) / 2, Quaternion.identity);
            block = apperDisplay.GetComponent<block>();
        }
        else
        {
            apperDisplay = null;
        }
        canplace = false;
        manager_Block.addhight(new Vector2Int((int)tarpos.x, (int)tarpos.z),Mathf.CeilToInt(tarpos.y),(int)block.blockdata.HP);
    }
    Vector3 masuka(Vector3 target)//マス目化
    {
        return new Vector3(Rounding(target.x),Rounding(target.y), Rounding(target.z));
       //  tyuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu
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
    }//四捨五入
    public void presschange(GameObject game)
    {
        placeObj = game;
    }
}
