using UnityEngine;

public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    // DontDestroyOnLoad—pbool
    [SerializeField] bool DDOL = false;
    public static T Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
            Instance = this as T;
        else if (Instance != this)
            Destroy(gameObject);

        if(DDOL)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
