using Cysharp.Threading.Tasks;
using Fusion;
using System;

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
            QuizData quizData = (await QuizData.GetInstanceAsync());
            ScoreData score = (await ScoreData.GetInstanceAsync());
            foreach(var kv in quizData.NwpAnswerDic)
            {
                if(kv.Value == quizData.NwpAnswer)
                {
                    score.RPC_ScoreAdd(kv.Key);
                }
            }
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