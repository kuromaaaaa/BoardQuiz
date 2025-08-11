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
    [Networked, OnChangedRender(nameof(OnChangeAnswerDic))][Capacity(100)][UnitySerializeField] public NetworkDictionary<int, NetworkString<_32>> NwpAnswerDic { get; } = new();
    [Networked, OnChangedRender(nameof(OnChangeSubmittedDic))][Capacity(100)][UnitySerializeField] public NetworkDictionary<int, bool> NwpSubmittedDic { get; } = new();

    public Action ChangeSubmitDic;
    public Action ChangeAnswerDic;

    public void PlayerJoined(PlayerRef player)
    {
        RPC_PlayerAdd(player.PlayerId);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_PlayerAdd(int id)
    {
        Debug.Log($"�ǉ��I PlayerID : {id}");
        NwpAnswerDic.Set(id, string.Empty);
        NwpSubmittedDic.Set(id, false);
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_PlayerRemove(int id)
    {
        Debug.Log($"�폜�I PlayerID : {id}");
        NwpAnswerDic.Remove(id);
        NwpSubmittedDic.Remove(id);
    }
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
    /// 
    /// </summary>
    /// <param name="id">PlayerId</param>
    /// <param name="submit">��o������</param>
    /// <param name="answer">����</param>
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendAnswer(int id, bool submit, string answer)
    {
        NwpAnswerDic.Set(id, answer);
        NwpSubmittedDic.Set(id, submit);
    }

    private async void OnChangeSubmittedDic()
    {
        if (NwpSubmittedDic.Count == NwpSubmittedDic.Where((x) => x.Value == true).Count())
        { // �S������o���Ă��邩�m�F

            NetWorkGameState gameState = (await NetWorkGameState.GetInstanceAsync());
            if (gameState.NwpCurrentGameState == GameState.Thinking)
            {
                gameState.RPC_ChangeState(GameState.Answer);
            }
        }
        ChangeSubmitDic?.Invoke();
    }

    private void OnChangeAnswerDic()
    {
        ChangeAnswerDic?.Invoke();
    }

}

public enum QuizGameMode
{
    BoardQuiz = 0,
    Nepleague = 1,
    Telepathy = 2,
    Select = -1,
}