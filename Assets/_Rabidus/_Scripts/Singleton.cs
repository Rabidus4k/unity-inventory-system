using UnityEngine;

/// <summary>
/// Generic Singleton base class.
/// Наследуйся от этого класса, чтобы сделать Singleton.
/// </summary>
/// <typeparam name="T">Класс-наследник</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    /// <summary>
    /// Экземпляр Singleton.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Пытаемся найти в сцене
                _instance = (T)FindAnyObjectByType(typeof(T));

                if (_instance == null)
                {
                    // Если не нашли — создаем новый GameObject
                    var singletonObj = new GameObject(typeof(T).Name);
                    _instance = singletonObj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Уничтожать ли объект при смене сцены.
    /// </summary>
    [SerializeField] private bool dontDestroyOnLoad = true;

    /// <summary>
    /// Выводить ли логи при создании/уничтожении.
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
