using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeManger : MonoBehaviour
{

    // ��Ԃ�\���萔
    private const int STATE_NORMAL = 0; // �m�[�}��
    private const int STATE_FORMATION = 1; // �Ґ����
    private const int STATE_BATTLE = 2; // �o�����
    public scendatas Scendata;
    public Animator ShaterAnimator;
    public GameObject[] FormationUI;
    public GameObject[] NormalUI;
    public Transform Plates;
    // ���݂̏��
    private int state;

    // Start���\�b�h�͏������������s��
    void Start()
    {
        // �Ґ���ʂɏ�����
        if (Time.timeScale == 0) Time.timeScale = 1;
    }

    // ��Ԃ�ύX����
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
        // ��Ԃɉ������������������s��
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
                // �m�[�}���̏���������������
                break;
            case STATE_BATTLE:
                // �o����ʂ̏���������������
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
            // �ړ��ʂ��v�Z����
            float t = Mathf.Clamp01(timeElapsed / duration);
            Vector3 newPosition = Vector3.Lerp(startingPosition, targetPosition, t);

            // �I�u�W�F�N�g�̈ʒu��ύX����
            Plates.position = newPosition;

            // �o�ߎ��Ԃ��X�V����
            timeElapsed += Time.deltaTime;

            // 1�t���[���҂�
            yield return null;
        }

        }
    }
    IEnumerator ActivateGameObjectsAfterDelay(float delay, GameObject[] gameObjects)
    {
        yield return new WaitForSeconds(delay);

        // �w�肳�ꂽGameObject���A�N�e�B�u�ɂ���
        foreach (GameObject gameObject in gameObjects)
        {
            if(gameObject.name!= "StageSelect")gameObject.SetActive(true);
           
        }
    }


}

