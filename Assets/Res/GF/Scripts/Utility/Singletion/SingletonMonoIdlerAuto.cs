using UnityEngine;

public abstract class SingletonMonoIdlerAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // 尝试在场景中查找已有实例
                    _instance = (T)FindObjectOfType(typeof(T));

                    // 如果没找到，自动创建一个新的GameObject
                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T).Name} (Auto-Created Singleton)";

                        // 如果是第一次创建，标记为DontDestroyOnLoad
                        DontDestroyOnLoad(singletonObject);

                        Debug.Log($"[Singleton] An instance of {typeof(T)} was auto-created in the scene with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        // 防止重复实例化
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning($"[Singleton] Multiple instances of {typeof(T)} detected. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
            // 如果是在编辑器模式下创建的，确保不会在加载新场景时被销毁
            if (Application.isEditor && !Application.isPlaying)
            {
                Debug.Log($"[Singleton] Editor mode instance created for {typeof(T)}");
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        // 只有当销毁的是当前实例时才清空_instance
        if (_instance == this)
        {
            _applicationIsQuitting = true;
            _instance = null;
        }
    }
}