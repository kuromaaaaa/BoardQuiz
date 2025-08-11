using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class ClientScoreManager : MonoBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] GameObject _ScorePrefab;

    Dictionary<int, ScoreView> scoreBoardDic = new();
    public async void PlayerJoined(PlayerRef player)
    {
        
        GameObject scoreBoard = Instantiate(_ScorePrefab);
        scoreBoard.transform.parent = this.transform;
        scoreBoard.transform.localScale = new Vector3(1, 1, 1);
        
        ScoreView score = scoreBoard.GetComponent<ScoreView>();
        score.Initialize((string)(await UserData.GetInstanceAsync()).NwpUserDic[player.PlayerId]);
        scoreBoardDic[player.PlayerId] = score;
    }

    public void PlayerLeft(PlayerRef player)
    {
        foreach(var kv in scoreBoardDic)
        {
            Destroy(kv.Value.gameObject);
        }
        scoreBoardDic.Clear();
    }



    public async void ChangeScore()
    {
        foreach(var kv in (await ScoreData.GetInstanceAsync()).NwpScoreDic)
        {
            scoreBoardDic[kv.Key].ScoreChane(kv.Value.ToString());
        }
    }
}
