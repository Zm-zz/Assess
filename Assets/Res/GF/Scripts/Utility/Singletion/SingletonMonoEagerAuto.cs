using UnityEngine;

/// <summary>
/// MonoBehaviour�������ࣨAwake�׶ζ���ģʽ��
/// �̳д�����ڽű�Awake�׶�������������ʵ��
/// </summary>
/// <typeparam name="T">��������</typeparam>
public abstract class SingletonMonoEagerAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _applicationIsQuitting = false;
    private static bool _isInitialized = false;

    // ʹ��[RuntimeInitializeOnLoadMethod]ȷ���ڳ�������ǰע���ʼ������
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticFields()
    {
        _instance = null;
        _applicationIsQuitting = false;
        _isInitialized = false;
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            lock (typeof(T))
            {
                if (_instance == null && !_applicationIsQuitting)
                {
                    _instance = this as T;
                    DontDestroyOnLoad(gameObject);
                    _isInitialized = true;
                    Debug.Log($"[Singleton] Awake-eager instance of {typeof(T)} created: {gameObject.name}");
                    return;
                }
            }
        }

        // ��ֹ�ظ�ʵ����
        if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] Multiple instances of {typeof(T)} detected. Destroying the new one.");
            Destroy(gameObject);
        }
    }

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            if (!_isInitialized)
            {
                InitializeIfNeeded();
            }

            return _instance;
        }
    }

    private static void InitializeIfNeeded()
    {
        if (_isInitialized || _applicationIsQuitting) return;

        lock (typeof(T))
        {
            if (_isInitialized || _applicationIsQuitting) return;

            // �����ڳ����в�������ʵ��
            _instance = FindObjectOfType<T>();

            if (_instance == null)
            {
                // �����µ�GameObject��������
                var singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = $"{typeof(T).Name} (Awake-Eager Singleton)";
                DontDestroyOnLoad(singletonObject);
                Debug.Log($"[Singleton] Awake-eager instance of {typeof(T)} was auto-created.");
            }

            _isInitialized = true;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _applicationIsQuitting = true;
            _instance = null;
            _isInitialized = false;
        }
    }
}