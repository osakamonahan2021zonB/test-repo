using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TaletButton : MonoBehaviour
{
    public scendatas Scendatas;
    public bool IsBattle;
    public int number;
    int cost;
    int energy;
    Camera_player cp;
    Button Button;
    Animator ani;
    TextMeshProUGUI text;
    blockdata blockdata;
    bool active = true;
   public Image Image;
    public Sprite NullImage;
    // Update is called once per frame
    private void Start()
    {
        Button = gameObject.GetComponent<Button>();
        ani = GetComponent<Animator>();
        text = transform.Find("Text(TMP)").GetComponent<TextMeshProUGUI>();
        if (Scendatas.GetBox(number) == null)
        {
            active = false;
            Button.interactable = false;
            text.text = "empty";
            text.color = Color.gray;
            return;
        }
        blockdata = Scendatas.GetBox(number).GetComponent<block>().blockdata;
       if(Image!=null)Image.sprite = blockdata.Icon;
       if(IsBattle)cp = Camera.main.gameObject.GetComponent<Camera_player>();
        cost =blockdata.cost;
        text.text = cost+"";
        energy = 100;
        text.enabled=IsBattle;
    }
    void Update()
    {

        if (!active) return;
        if (IsBattle) { energy = cp.energy;ani.SetBool("Informed",cp.placeObj==Scendatas.GetBox(number)); }
        Button.interactable = (energy>=cost);
        text.color = energy >= cost ?Color.white:Color.red;
        if (energy < cost) ani.SetBool("Informed",false);
    }
    public void press()
    {
        if (IsBattle) { cp.presschange(Scendatas.GetBox(number)); }
        else
        {
            Scendatas.EraseBox(number);
            active = false;
            Button.interactable = false;
            text.text = "empty";
            text.color = Color.gray;
            Image.sprite = NullImage;
        }
    }
    public void Plase()
    {
        active = true;
        blockdata = Scendatas.GetBox(number).GetComponent<block>().blockdata;
        Image.sprite = blockdata.Icon;
        Button.interactable = true;
        text.text ="";
    }
}
