using Fusion;
using System;
using System.Linq;
using UnityEngine;

public class QuizData : SingletonNetWorkBehaviour<QuizData>, IPlayerJoined
{
    [Networked][UnitySerializeField] public NetworkString<_32> NwpQuestion { get; set; }
    [Networked][UnitySerializeField] public NetworkString<_32> NwpAnswer { get; set; }
    [Networked][UnitySerializeField] public int NwpThinkingTime { get; set; }
    [Networked][UnitySerializeField] public QuizGameMode NwpGameMode { get; set; } = QuizGameMode.Select;
    // PlayerId と 答えの辞書
    [Networked, OnChangedRender(nameof(OnChangeAnswerDic))][Capacity(100)][UnitySerializeField] public NetworkDictionary<int, NetworkString<_32>> NwpAnswerDic { get; } = new();
    [Networked, OnChangedRender(nameof(OnChangeSubmittedDic))][Capacity(100)][UnitySerializeField] public NetworkDictionary<int, bool> NwpSubmittedDic { get; } = new();

    public Action ChangeSubmitDicAction;
    public Action ChangeAnswerDicAction;

    public void PlayerJoined(PlayerRef player)
    {
        RPC_PlayerAdd(player.PlayerId);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_PlayerAdd(int id)
    {
        Debug.Log($"Add PlayerID : {id}");
        NwpAnswerDic.Set(id, string.Empty);
        NwpSubmittedDic.Set(id, false);
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_PlayerRemove(int id)
    {
        Debug.Log($"Remove PlayerID : {id}");
        NwpAnswerDic.Remove(id);
        NwpSubmittedDic.Remove(id);
    }
    
    /// <summary>
    /// ゲームリスタート時に回答をクリアする
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_PlayerClear()
    {
        NwpAnswerDic.Clear();
        NwpSubmittedDic.Clear();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Question(string q, string a, int time)
    {
        NwpQuestion = q;
        NwpAnswer = a;
        NwpThinkingTime = time;
    }

    /// <summary>
    /// Playerの回答をサーバーに送信する
    /// </summary>
    /// <param name="id">送信元のPlayerId</param>
    /// <param name="submit">回答提出フラグ</param>
    /// <param name="answer">回答の内容</param>
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendAnswer(int id, bool submit, string answer)
    {
        NwpAnswerDic.Set(id, answer);
        NwpSubmittedDic.Set(id, submit);
    }

    /// <summary>
    /// 実装された辞書に変化があったときに呼び出される
    /// </summary>
    private async void OnChangeSubmittedDic()
    {
        if (NwpSubmittedDic.Count == NwpSubmittedDic.Where((x) => x.Value).Count())
        { // すべてのPlayerが回答したら

            (await OriginNetWorkTimer.GetInstanceAsync()).RPC_TimerComplete();
        }
        ChangeSubmitDicAction?.Invoke();
    }

    private void OnChangeAnswerDic()
    {
        ChangeAnswerDicAction?.Invoke();
    }

}

public enum QuizGameMode
{
    BoardQuiz = 0,
    Nepleague = 1,
    Telepathy = 2,
    Select = -1,
}