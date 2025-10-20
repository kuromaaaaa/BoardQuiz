using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ClientScoreManager : MonoBehaviour
{
    [SerializeField] GameObject _ScorePrefab;

    Dictionary<int, ScoreView> scoreBoardDic = new();
    
    UserData _userData;

    private void Start()
    {
        NetworkStarter.Instance.JoinGame += Initialize;
    }

    private async void Initialize()
    {
        ScoreData scoreData = await ScoreData.GetInstanceAsync();

        scoreData.ScoreChangeAction.Add(
            NetworkRunnerLocator.Instance.LocalPlayer.PlayerId
            ,ClientScoreChange);

        foreach (var data in scoreData.NwpScoreDic)
        {
            PlayerAdd(data.Key);
        }
    }


    private void ClientScoreChange(PlayerScoreChangeData data)
    {
        switch (data.State)
        {
            case (PlayerScoreChangeState.Add):
            {
                foreach (var ids in data.PlayerIds)
                {
                    Debug.Log("PlayerAdd : " + string.Join(",", ids));
                    PlayerAdd(ids);
                }
                break;
            }
            case PlayerScoreChangeState.Remove:
            {
                foreach (var ids in data.PlayerIds)
                {
                    Debug.Log("PlayerRemove : " + string.Join(",", ids));
                    PlayerLeft(ids);
                }
                break;
            }
            case (PlayerScoreChangeState.ScoreChange):
            {
                Debug.Log("PlayerScoreChange : " + string.Join(",", data.PlayerIds));
                ChangeScore();
                break;
            }
        }
    }

    private async void PlayerAdd(int playerId)
    {
        if(scoreBoardDic.ContainsKey(playerId)) return;
        GameObject scoreBoard = Instantiate(_ScorePrefab, transform, true);
        scoreBoard.transform.localScale = new Vector3(1, 1, 1);

        ScoreView score = scoreBoard.GetComponent<ScoreView>();
        if(_userData == null) _userData = await UserData.GetInstanceAsync();
        await UniTask.WaitUntil(() => _userData.NwpUserDic.ContainsKey(playerId));
        score.Initialize((string)_userData.NwpUserDic[playerId], 0);
        scoreBoardDic.Add(playerId, score);
    }

    private void PlayerLeft(int  playerId)
    {
        Destroy(scoreBoardDic[playerId].gameObject);
        scoreBoardDic.Remove(playerId);
    }


    private async void ChangeScore()
    {
        foreach (var kv in (await ScoreData.GetInstanceAsync()).NwpScoreDic)
        {
            scoreBoardDic[kv.Key].AddScore(kv.Value.ToString());
        }
    }
}
