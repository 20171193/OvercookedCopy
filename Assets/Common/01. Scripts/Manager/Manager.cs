using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager
{
    public static SceneManager Scene { get {return SceneManager.Instance; } }
    public static PlableDataManager PlableData {get { return PlableDataManager.Instance; } }
    public static LayerManager Layer {get { return LayerManager.Instance; } }
    public static SoundManager Sound {get { return SoundManager.Instance; } }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        SceneManager.ReleaseInstance();
        PlableDataManager.ReleaseInstance();
        LayerManager.ReleaseInstance();
        SoundManager.ReleaseInstance(); 

        // 싱글턴 객체생성
        SceneManager.CreateInstance();
        PlableDataManager.CreateInstance();
        LayerManager.CreateInstance();
        SoundManager.CreateInstance();  
    }
}
