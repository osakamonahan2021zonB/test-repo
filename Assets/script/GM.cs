using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GM : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
   public struct Spawns
    {
        public int Waynumber;
        public GameObject enemy;
        public int density;
        public float probability;
        public AnimationCurve Curve;
        enemydata enemd;
        public enemydata Getenemydata()
        {
        if(enemd==null)
         {
            enemd=enemy.GetComponent<enemy_base>().enemydata;
         }
         return enemd;
        }
    }
    public List<Spawns> spawns=new List<Spawns>();
   [SerializeField] int Finwave;
   [SerializeField] float wavetime;
   [SerializeField] float waveapantime;

   public float time;
   int currentwave=0;
   public Text wavedisplay;
    [SerializeField] GameObject wintext;
    [SerializeField] GameObject losetext;
    [SerializeField] GameObject fintext;
    public static  GM  gM{ get; private set; } = null;
    bool finish;
    private void Awake()
    {
        if (gM == null)
        {
            gM = this;
            return;
        }
        Destroy(this);
    }
    void Update()
    {
        if (fintext.activeSelf&&Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("op");
        }
        if (finish) return;
        if (currentwave == 0)
        {
            wavedisplay.text = "戦闘開始";
        }
        else
        {
          wavedisplay.text = "Wave"+currentwave+"/"+Finwave+"次のWaveまで"+Mathf.Floor(wavetime-time);
         if (currentwave < Finwave)
         {
            if (time < wavetime)
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                     wavetime = 0;
                     for(int i = 0; i < spawns.Count; i++)
                     {
                        if (Random.value < spawns[i].probability)
                        {
                            StartCoroutine(spawn(Rounding(spawns[i].Curve.Evaluate((float)currentwave / (float)Finwave)),spawns[i], Vector2Int.zero));
                            }
                        int a = CountSpawn(Rounding(spawns[i].Curve.Evaluate(currentwave / Finwave)), spawns[i]);
                        if (a > wavetime) wavetime = a;
                     }
                     wavetime +=waveapantime;
                     currentwave++;  
            }
         }
         else
         {
               if (GameObject.FindGameObjectsWithTag("enemy").Length==0)
                {
                    wavedisplay.text = "Wave：クリア";
                    Finishbattel(true);
                }
         }
        }

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
    int CountSpawn(int amount, Spawns target)
    {
        int a = amount;
        int item = target.density;
        return Mathf.CeilToInt(a/item);
    }
    IEnumerator spawn(int amount,Spawns target,Vector2Int place)
    {

        int a = amount;
        int item = target.density;
        int hight=target.enemy.GetComponent<enemy_base>().enemydata.hight;
        Vector2 way= Quaternion.Euler(0, 0, 90)* manager_block.MB.Tellway(place,hight).normalized/2;
        while (a>0)
        {
            if (a < item)item = a;
            for(float i = 1; i <= item; i++)
            {
               GameObject enem =Instantiate(target.enemy);
                enem.transform.position= transform.position+new Vector3(way.x, 0, way.y) * (-1 + 2 * i / (item + 1))*1.2f+Vector3.up*(hight-0.5f);
                enem.SendMessage("doslide", -1 + 2 * i / (item + 1));
                a--;
            }
            yield return new WaitForSeconds(2f);
        }
    }
    public void WaveStart()
    {
        currentwave = 1;
        StartCoroutine(spawn(2,spawns[0],new Vector2Int(0,0)));
    }
    void Finishbattel(bool win)
    {
        finish = true;
        fintext.SetActive(true);
        wintext.SetActive(win);
        losetext.SetActive(!win);
    }
}
