using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SaySceneSize
{
    // �����̐ݒ�
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        // �X�N���[���T�C�Y�̎w��
        Screen.SetResolution(960, 540, false);
    }
}
