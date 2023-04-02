using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public string stagename;
   public void ChangeScene()
    {
        
       SceneManager.LoadScene(stagename);
        
    }
    public void GetActiveScene()
    {

    }
}
