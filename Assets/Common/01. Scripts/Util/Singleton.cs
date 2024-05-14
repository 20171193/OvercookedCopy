using UnityEngine;

/// <summary>
/// 싱글턴 베이스 클래스
/// 매니저 일반화 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 싱글턴 객체 생성
    public static void CreateInstance()
    {
        T resource = Resources.Load<T>($"{typeof(T).Name}");
        instance = Instantiate(resource);
    }

    // 싱글턴 객체 해제
    public static void ReleaseInstance()
    {
        if (instance == null)
            return;

        Destroy(instance.gameObject);
        instance = null;
    }
}