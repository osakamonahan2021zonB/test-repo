using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InformButton : MonoBehaviour
{
    Animator buttonAni;
    Camera_player camera_Player;
    bool informed;
    public bool IsFormation;
    GameObject ing=null;
    bool refer;
    GameObject reg=null;
    block redB;
    public blockdata infodata;
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Grade;
    public TextMeshProUGUI Type;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI Power;
    public TextMeshProUGUI Reload;
    public TextMeshProUGUI delatecost;
    public TextMeshProUGUI detail;
    public Text marge;
    public GameObject refers;
    public GameObject tabbutton;
   public bool LockButton;
    float wid;
   public bool Close;
    // Start is called before the first frame update
    void Start()
    {
        buttonAni = GetComponent<Animator>();
        if (!IsFormation) { camera_Player = Camera.main.gameObject.GetComponent<Camera_player>(); }
        else { buttonAni.speed *= 2.1f; }
        wid = Screen.width / 3 * 2;
    }

    // Update is called once per frame
    void Update()
    {if (IsFormation)
        {
            return;
        }
        informed = camera_Player.placeObj != null;
        marge.text = camera_Player.canmarge ?"選択してください":"合成";
        if (informed && ing != camera_Player.placeObj) {
            settext();
            ing = camera_Player.placeObj;
        }
        refer = camera_Player.informObj != null;
        if (refer && ing != camera_Player.informObj) {
            settext();
            reg = camera_Player.informObj;
            redB = reg.GetComponent<block>();
        }
        tabbutton.SetActive(informed || refer);
        if (buttonAni.GetBool("close")&&(informed || refer))
        {
            buttonAni.SetTrigger("change");
            tabbutton.transform.Find("Text (TMP)").transform.rotation = Quaternion.Euler(0,0,90);
            settext();
        }
        buttonAni.SetBool("close",!(informed||refer));
        if (refer) { HP.text = "HP  " + redB.hp + "/" + infodata.HP;
            float a = infodata.cost * 0.7f * redB.hp / infodata.HP;
            delatecost.text = "回収　" + Mathf.Ceil(a);
        }
        if (LockButton && !(informed || refer)) { Close = false;LockButton = false; }
       /* if (informed||refer)
        {
            if(Input.mousePosition.x > wid)
            {
                if (!LockButton && !Close) { buttonAni.SetTrigger("change");Close=true; tabbutton.transform.Find("Text (TMP)").transform.rotation = Quaternion.Euler(0, 0, -90); }
            }
            else
            {
                if (!LockButton && Close) { buttonAni.SetTrigger("change"); Close = false; tabbutton.transform.Find("Text (TMP)").transform.rotation = Quaternion.Euler(0, 0, +90); }
            }
        }*/

    }
    public void pushed()
    {
        if ((informed || refer)) { buttonAni.SetTrigger("change"); }
        Close = !Close;
        tabbutton.transform.Find("Text (TMP)").transform.rotation = Quaternion.Euler(0, 0,!Close?90:-90);
        LockButton =true;
    }

    private void OnEnable()
    {
       if(IsFormation)settext();
    }

    public  void settext()
    {
        if (IsFormation)
        {
            if (buttonAni==null) buttonAni = GetComponent<Animator>();
            buttonAni.Play("nomal",0);
            buttonAni.SetTrigger("change");
            detail.gameObject.SetActive(true);
            detail.text = "　説明" + "\n" + infodata.explain;
        }
        else
        {
         if (informed)
         {
            infodata = camera_Player.placeObj.GetComponent<block>().blockdata;
            detail.gameObject.SetActive(true);
            detail.text = "　説明"+"\n"+infodata.explain;
            refers.SetActive(false);
         }
         else
         {
              infodata = camera_Player.informObj.GetComponent<block>().blockdata;
              redB=camera_Player.informObj.GetComponent<block>();
             if (infodata.CanTdestoroy)
             {
                 detail.gameObject.SetActive(true);
                 detail.text = "　説明" + "\n" + infodata.explain;
                 refers.SetActive(false);
             }
             else
             {
                 refers.SetActive(true);
                 detail.gameObject.SetActive(false);
             }
         }
        }
        Name.text = infodata.name;
            Grade.text = "Grade:"+infodata.Grade;
            Type.text = "Type"+infodata.blockType;
            HP.text = "HP   "+infodata.HP;
        if (infodata.blockType == blockdata.BlockType.Wall)
        {
            Power.text = "Don't Attack";
            Reload.text = "";
        }
        else
        {
            Power.text = "Power"+infodata.Damage;
            Reload.text = "Reload"+infodata.AttackRate;
        }
        delatecost.text = "回収　"+infodata.cost;
        Icon.sprite = infodata.Icon;
    }
}
