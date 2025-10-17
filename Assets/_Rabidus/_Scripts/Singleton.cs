using UnityEngine;

/// <summary>
/// Generic Singleton base class.
/// ���������� �� ����� ������, ����� ������� Singleton.
/// </summary>
/// <typeparam name="T">�����-���������</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    /// <summary>
    /// ��������� Singleton.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // �������� ����� � �����
                _instance = (T)FindAnyObjectByType(typeof(T));

                if (_instance == null)
                {
                    // ���� �� ����� � ������� ����� GameObject
                    var singletonObj = new GameObject(typeof(T).Name);
                    _instance = singletonObj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// ���������� �� ������ ��� ����� �����.
    /// </summary>
    [SerializeField] private bool dontDestroyOnLoad = true;

    /// <summary>
    /// �������� �� ���� ��� ��������/�����������.
    /// </summary>
    [SerializeField] private bool enableLogs = false;

    protected virtual void Awake()
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);

                if (enableLogs)
                    Debug.Log($"[Singleton<{typeof(T).Name}>] Created instance.");
            }
            else if (_instance != this)
            {
                if (enableLogs)
                    Debug.LogWarning($"[Singleton<{typeof(T).Name}>] Duplicate destroyed.");
                Destroy(gameObject);
            }
        }
    }
}
