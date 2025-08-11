using Fusion;
using System;
using UnityEngine;

public class ScoreData : SingletonNetWorkBehaviour<ScoreData>
{
    [Networked,OnChangedRender(nameof(OnChangeScore))][Capacity(100)][UnitySerializeField] public NetworkDictionary<int, int> NwpScoreDic { get; } = new();

    public Action ScoreChange;

    void OnChangeScore() => ScoreChange?.Invoke();

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ScoreAdd(int id)
    {
        NwpScoreDic.Set(id, NwpScoreDic[id] + 1);
    }
}
