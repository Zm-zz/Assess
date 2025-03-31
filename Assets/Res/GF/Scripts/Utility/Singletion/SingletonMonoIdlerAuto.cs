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
                    // �����ڳ����в�������ʵ��
                    _instance = (T)FindObjectOfType(typeof(T));

                    // ���û�ҵ����Զ�����һ���µ�GameObject
                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T).Name} (Auto-Created Singleton)";

                        // ����ǵ�һ�δ��������ΪDontDestroyOnLoad
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
        // ��ֹ�ظ�ʵ����
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning($"[Singleton] Multiple instances of {typeof(T)} detected. Destroying the new one.");
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
            // ������ڱ༭��ģʽ�´����ģ�ȷ�������ڼ����³���ʱ������
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
        // ֻ�е����ٵ��ǵ�ǰʵ��ʱ�����_instance
        if (_instance == this)
        {
            _applicationIsQuitting = true;
            _instance = null;
        }
    }
}