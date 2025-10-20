using Fusion;
using System;
using System.Collections.Generic;

public class ScoreData : SingletonNetWorkBehaviour<ScoreData>, IPlayerJoined, IPlayerLeft
{
    /// <summary>
    /// Key int PlayerId , Value int Score
    /// </summary>
    [Networked][Capacity(100)][UnitySerializeField] public NetworkDictionary<int, int> NwpScoreDic { get; } = new();

    public Dictionary<int, Action<PlayerScoreChangeData>> ScoreChangeAction = new();
    
    QuizData _quizData;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ScoreAdd(int id)
    {
        NwpScoreDic.Set(id, NwpScoreDic[id] + 1);
    }

    public void PlayerJoined(PlayerRef player)
    {
        NwpScoreDic.Add(player.PlayerId, 0);

        // 新しく入ってきたクライアント以外のクライアントにJoinを通知する
        foreach (var score in ScoreChangeAction)
        {
            if(player.PlayerId == score.Key) continue;
            
            score.Value?.Invoke(new(PlayerScoreChangeState.Add, 
                new List<int>(){player.PlayerId}));
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        NwpScoreDic.Remove(player.PlayerId);
        foreach (var score in ScoreChangeAction)
        {
            score.Value?.Invoke(
                new(PlayerScoreChangeState.Remove,
                    new List<int>() { player.PlayerId })
            );
        }
    }

    public async void Correct()
    {
        if (_quizData == null) _quizData = await QuizData.GetInstanceAsync();
        
        List<int> correctAnswerPlayerIds = new List<int>();
        foreach(var kv in _quizData.NwpAnswerDic)
        {
            if(kv.Value == _quizData.NwpAnswer)
            {
                RPC_ScoreAdd(kv.Key);
                correctAnswerPlayerIds.Add(kv.Key);
            }
        }

        foreach (var score in ScoreChangeAction)
        {
            score.Value?.Invoke(new PlayerScoreChangeData(PlayerScoreChangeState.ScoreChange, correctAnswerPlayerIds));
        }
    }
}

public class PlayerScoreChangeData
{
    public PlayerScoreChangeState State;
    public List<int> PlayerIds;

    public PlayerScoreChangeData(PlayerScoreChangeState state, List<int> playerIds)
    {
        State = state;
        PlayerIds = playerIds;
    }
}

public enum PlayerScoreChangeState
{
    Add,
    Remove,
    ScoreChange
}