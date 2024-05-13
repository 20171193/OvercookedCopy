using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public static class Manager
{
    private static SceneManager sceneManager;
    public static SceneManager Scene { get {return sceneManager; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        SceneManager.ReleaseInstance();

        // 싱글턴 객체생성
        SceneManager.CreateInstance();
    }
}
