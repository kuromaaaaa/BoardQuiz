using Fusion;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ChatData : SingletonNetWorkBehaviour<ChatData>
{
    [Networked][Capacity(100)][UnitySerializeField] public NetworkLinkedList<NetworkString<_32>> CommentList { get ; } = new NetworkLinkedList<NetworkString<_32>>();

    public Action SpawnedAction;
    public Action<string> AddCommentAction;

    public override void Spawned()
    {
        base.Spawned();
        SpawnedAction?.Invoke();
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AddComment(string message)
    {
        Debug.Log(message);
        CommentList.Add(message);
        AddCommentAction?.Invoke(message);
    }
}