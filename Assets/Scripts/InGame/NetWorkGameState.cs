using Cysharp.Threading.Tasks;
using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkGameState : SingletonNetWorkBehaviour<NetWorkGameState>
{
    [Networked, OnChangedRender(nameof(OnChangeSceneState))]
    public GameState NwpCurrentGameState { get; set; } = GameState.Title;

    public Action<GameState> ClientChangeStateAction;

    //State変更時に呼び出される
    public async void OnChangeSceneState()
    {
        ClientChangeStateAction?.Invoke(NwpCurrentGameState);
        if(NwpCurrentGameState == GameState.Thinking)
        {
            OriginNetWorkTimer timer = (await OriginNetWorkTimer.GetInstanceAsync());
            QuizData quiz = (await QuizData.GetInstanceAsync());

            timer.NwpCurrentTime = quiz.NwpThinkingTime;
            timer.RPC_TimerStart(quiz.NwpThinkingTime);
        }
        else if(NwpCurrentGameState == GameState.Answer)
        {
            ScoreData score = (await ScoreData.GetInstanceAsync());
            score.Correct();

            await UniTask.WaitForSeconds(1);
            NwpCurrentGameState = GameState.GameSelect;
        }
    }

    private void OnEnable()
    {
        ClientChangeStateAction += PlayerClear;
    }

    private void OnDisable()
    {
        ClientChangeStateAction -= PlayerClear;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ChangeState(GameState state)
    {
        NwpCurrentGameState = state;
    }

    void PlayerClear(GameState state)
    {
        if (NwpCurrentGameState == GameState.GameSelect)
        {
            QuizData.Instance.RPC_PlayerClear();
        }
    }
}