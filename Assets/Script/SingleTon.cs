using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region SINGLETON
    private static T singleton = null;
    public static T Instance
    {
        get
        {
            if (!singleton)
                singleton = FindAnyObjectByType<T>();

            return singleton;
        }
    }
    #endregion SINGLETON

    private void Awake()
    {
        EnsureSingleInstance();
    }

    protected virtual void EnsureSingleInstance()
    {
        if (singleton == null)
        {
            singleton = this as T;
            DontDestroyOnLoad(gameObject);

            if (transform.parent != null && transform.root != null)
                DontDestroyOnLoad(transform.root.gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }
}