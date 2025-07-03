using Fusion;
using System;
using UnityEngine;

public class ChatData : NetworkBehaviour
{
    [Networked][Capacity(100)][UnitySerializeField] public NetworkLinkedList<NetworkString<_32>> CommentList { get ; } = new NetworkLinkedList<NetworkString<_32>>();

    public static ChatData _instance;
    public static ChatData Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ChatData>();
            }

            return _instance;
        }
    }


    public Action SpawnedAction;
    public Action<string> AddCommentAction;

    public override void Spawned()
    {
        base.Spawned();
        SpawnedAction?.Invoke();
        DontDestroyOnLoad(gameObject);

        Debug.Log("ê|ÅóÉ|Å[ÉìÅ´");
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AddComment(string message)
    {
        CommentList.Add(message);
        AddCommentAction?.Invoke(message);
    }
}