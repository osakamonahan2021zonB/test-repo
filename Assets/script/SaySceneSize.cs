using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SaySceneSize
{
    // 属性の設定
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        // スクリーンサイズの指定
        Screen.SetResolution(960, 540, false);
    }
}
