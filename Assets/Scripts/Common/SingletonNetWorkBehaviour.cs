using Cysharp.Threading.Tasks;
using Fusion;
using System.Threading;
using UnityEngine;

public abstract class SingletonNetWorkBehaviour<T> : NetworkBehaviour where T : NetworkBehaviour
{
    // NetWorkObject���X�|�[������Ă��邩
    public bool IsSpawned { get; protected set; }

    // DontDestroyOnLoad�pbool
    [SerializeField] bool DDOL = false;

    public static T Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
            Instance = this as T;
        else if (Instance != this)
        {
            Debug.Log("��ȏ゠���");
            Destroy(gameObject);
        }
    }
    public override void Spawned()
    {
        base.Spawned();
        IsSpawned = true;
        if (DDOL) DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ���g���X�|�[�����ꂽ��<T>Instance��Ԃ�
    /// </summary>
    /// <returns></returns>
    public static async UniTask<T> GetInstanceAsync()
    {
        await UniTask.WaitUntil(() => Instance != null && (Instance as SingletonNetWorkBehaviour<T>).IsSpawned);
        return Instance;
    }

    public static async UniTask<T> GetInstanceAsync(CancellationToken cancellationToken = default)
    {
        await UniTask.WaitUntil(() => Instance != null && (Instance as SingletonNetWorkBehaviour<T>).IsSpawned, default, cancellationToken);
        return Instance;
    }

}
