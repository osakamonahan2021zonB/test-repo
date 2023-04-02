using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "MyscendData", menuName = "MyscendData")]
public class scendatas : ScriptableObject
{
    //　使用するキャラクタープレハブ
    [SerializeField]
    private GameObject[] boxs=new GameObject[5];

    public void SetBox(GameObject talet,int i)
    {
        boxs[i - 1] = talet;
    }

    public GameObject GetBox(int i)
    {
        return boxs[i-1];
    }
    public float LengthBox()
    {
        return boxs.Length;
    }
    public void EraseBox(int i)
    {
        boxs[i - 1]=null;
    }
    public void SetBox(int i,GameObject game)
    {
        boxs[i - 1] = game;
    }
}
