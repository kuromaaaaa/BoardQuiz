using Fusion;
using System;
using UnityEngine;

public class ChatData : SingletonNetWorkBehaviour<ChatData>
{
    [Networked][Capacity(100)][UnitySerializeField] public NetworkLinkedList<NetworkString<_32>> NwpCommentList { get; } = new NetworkLinkedList<NetworkString<_32>>();

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
        NwpCommentList.Add(message);
        AddCommentAction?.Invoke(message);
    }
}