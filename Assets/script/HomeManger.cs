using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeManger : MonoBehaviour
{

    // 状態を表す定数
    private const int STATE_NORMAL = 0; // ノーマル
    private const int STATE_FORMATION = 1; // 編成画面
    private const int STATE_BATTLE = 2; // 出撃場面
    public scendatas Scendata;
    public Animator ShaterAnimator;
    public GameObject[] FormationUI;
    public GameObject[] NormalUI;
    public Transform Plates;
    // 現在の状態
    private int state;

    // Startメソッドは初期化処理を行う
    void Start()
    {
        // 編成画面に初期化
        if (Time.timeScale == 0) Time.timeScale = 1;
    }

    // 状態を変更する
    public void SetState(int newState)
    {
        state = newState;
        foreach (GameObject gameObject in FormationUI)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in NormalUI)
        {
            gameObject.SetActive(false);
        }
        // 状態に応じた初期化処理を行う
        ShaterAnimator.SetTrigger("Act");
        
        StartCoroutine(MovePlates(new Vector3(0, 0, 0), 0.6f));
        switch (state)
        {
            case STATE_FORMATION:
                    StartCoroutine(MovePlates(new Vector3(-13.5f,0,0),1));
                    StartCoroutine(ActivateGameObjectsAfterDelay(0.6f,FormationUI));
                break;
            case STATE_NORMAL:
                    StartCoroutine(ActivateGameObjectsAfterDelay(0.6f,NormalUI));
                // ノーマルの初期化処理を書く
                break;
            case STATE_BATTLE:
                // 出撃場面の初期化処理を書く
                break;
            default:
                Debug.LogError("Invalid state: " + state);
                break;
        }
    }
    IEnumerator MovePlates(Vector3 targetPosition, float duration)
    {
        float timeElapsed = 0f;
        Vector3 startingPosition = Plates.position;
        if (targetPosition!=Plates.position)
        {
        while (timeElapsed < duration)
        {
            // 移動量を計算する
            float t = Mathf.Clamp01(timeElapsed / duration);
            Vector3 newPosition = Vector3.Lerp(startingPosition, targetPosition, t);

            // オブジェクトの位置を変更する
            Plates.position = newPosition;

            // 経過時間を更新する
            timeElapsed += Time.deltaTime;

            // 1フレーム待つ
            yield return null;
        }

        }
    }
    IEnumerator ActivateGameObjectsAfterDelay(float delay, GameObject[] gameObjects)
    {
        yield return new WaitForSeconds(delay);

        // 指定されたGameObjectをアクティブにする
        foreach (GameObject gameObject in gameObjects)
        {
            if(gameObject.name!= "StageSelect")gameObject.SetActive(true);
           
        }
    }


}

