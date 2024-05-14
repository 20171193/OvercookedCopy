using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager
{
    public static SceneManager Scene { get {return SceneManager.Instance; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        SceneManager.ReleaseInstance();

        // 싱글턴 객체생성
        SceneManager.CreateInstance();
    }
}
