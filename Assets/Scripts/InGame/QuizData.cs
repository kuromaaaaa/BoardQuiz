using Fusion;
using System;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class QuizData : SingletonNetWorkBehaviour<QuizData>,IPlayerJoined,IPlayerLeft
{
    [Networked][UnitySerializeField] public NetworkString<_32> Question { get; set; }
    [Networked][UnitySerializeField] public NetworkString<_32> Answer { get; set; }
    [Networked][UnitySerializeField] public int ThinkingTime { get; set; }
    [Networked][UnitySerializeField] public QuizGameMode GameMode { get; set; } = QuizGameMode.Select;
    [Networked][Capacity(100)][UnitySerializeField] public NetworkDictionary<PlayerRef,NetworkString<_32>> AnswerDic { get;} = new();
    [Networked, OnChangedRender(nameof(OnChangeSubmittedDic))][Capacity(100)][UnitySerializeField] public NetworkDictionary<PlayerRef, bool> SubmittedDic { get;} = new();

    public Action ChangeSubmitUI;
    public Action ChangeAnswerDic;

    public void PlayerJoined(PlayerRef player)
    {
        RPC_PlayerAdd(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        RPC_PlayerRemove(player);
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_PlayerAdd(PlayerRef player)
    {
        AnswerDic.Set(player, string.Empty);
        SubmittedDic.Set(player, false);
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_PlayerRemove(PlayerRef player)
    {
        RPC_PlayerAdd(player);
        AnswerDic.Remove(player);
        SubmittedDic.Remove(player);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Question(string q,string a,int time)
    {
        Question = q;
        Answer = a;
        ThinkingTime = time;
    }

    void OnChangeSubmittedDic()
    {
        ChangeSubmitUI?.Invoke();
    }

    void OnChangeAnswerDic() => ChangeAnswerDic?.Invoke();

}

public enum QuizGameMode
{
    BoardQuiz = 0,
    Nepleague = 1,
    Telepathy = 2,
    Select = -1,
}