using Cysharp.Threading.Tasks;
using Fusion;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetWorkGameState : SingletonNetWorkBehaviour<NetWorkGameState>
{
    [Networked, OnChangedRender(nameof(OnChangeSceneState))]
    public GameState CurrentGameState { get; set; } = GameState.Title;

    public Action<GameState> ClientChangeScene;

    public void OnChangeSceneState()
    {
        ClientChangeScene?.Invoke(CurrentGameState);
    }

    public override void Spawned()
    {
        base.Spawned();
    }
}