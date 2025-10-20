using Cysharp.Threading.Tasks;
using Fusion;
using System;
using System.Collections.Generic;

public class NetWorkGameState : SingletonNetWorkBehaviour<NetWorkGameState>
{
    [Networked, OnChangedRender(nameof(OnChangeSceneState))]
    public GameState NwpCurrentGameState { get; set; } = GameState.Title;

    public Action<GameState> ClientChangeSceneAction;

    public async void OnChangeSceneState()
    {
        ClientChangeSceneAction?.Invoke(NwpCurrentGameState);
        if(NwpCurrentGameState == GameState.Thinking)
        {
            OriginNetWorkTimer timer = (await OriginNetWorkTimer.GetInstanceAsync());
            QuizData quiz = (await QuizData.GetInstanceAsync());

            timer.CurrentTime = quiz.NwpThinkingTime;
            timer.TimerStart(quiz.NwpThinkingTime);
        }
        else if(NwpCurrentGameState == GameState.Answer)
        {
            ScoreData score = (await ScoreData.GetInstanceAsync());
            score.Correct();
        }
    }

    private void OnEnable()
    {
        ClientChangeSceneAction += PlayerClear;
    }

    private void OnDisable()
    {
        ClientChangeSceneAction -= PlayerClear;
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