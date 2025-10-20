using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehavior<UIManager>
{

    [SerializeField] private GameState _currentState;
    public GameState CurrentState
    {
        set
        {
            OnChangeState(value);
        }
        get => _currentState;
    }
    public PlayerRole Role { get; set; } = PlayerRole.Guest;

    [SerializeField] private List<StateAndObjects> _stateActiveObject;
    [SerializeField] private List<GameObject> zuttoHyoujiSuruYatu;

    [SerializeField] private SetTimer _timer;
    private Tweener _timerTweener;
    private void OnEnable()
    {
        NetWorkGameState.Instance.ClientChangeSceneAction += OnChangeState;
        QuizData.Instance.ChangeSubmitDicAction += AllSubmitAnswer;
    }

    private void OnDisable()
    {
        NetWorkGameState.Instance.ClientChangeSceneAction -= OnChangeState;
        QuizData.Instance.ChangeSubmitDicAction -= AllSubmitAnswer;
    }

    private async void OnChangeState(GameState nextState)
    {
        switch (nextState)
        {
            case (GameState.QuestionInput):
            {
                Debug.Log($"Player{NetworkRunnerLocator.Instance.LocalPlayer.PlayerId} : {Role}");
                switch (Role)
                {
                    case (PlayerRole.Guest):
                    {
                        nextState = GameState.Waiting;
                        (await QuizData.GetInstanceAsync()).RPC_PlayerAdd(NetworkRunnerLocator.Instance.LocalPlayer.PlayerId);
                        break;
                    }
                    case (PlayerRole.PlayingHost):
                    {
                        (await QuizData.GetInstanceAsync()).RPC_PlayerAdd(NetworkRunnerLocator.Instance.LocalPlayer.PlayerId);
                        break;
                    }
                    case (PlayerRole.Host):
                    {
                        (await QuizData.GetInstanceAsync()).RPC_PlayerRemove(NetworkRunnerLocator.Instance.LocalPlayer.PlayerId);
                        break;
                    }
                }
                break;
            }
            case (GameState.GameSelect):
            {
                Role = PlayerRole.Guest;
                break;
            }
            case (GameState.Thinking):
            {
                if (Role == PlayerRole.Host) nextState = GameState.GodMode;
                TimerStart();
                break;
            }
            case (GameState.Answer):
            {
                _timerTweener?.Kill();
                break;
            }
        }


        foreach (var sao in _stateActiveObject)
        {
            if (sao.state == nextState)
            {
                foreach (Transform go in this.gameObject.transform)
                {
                    go.gameObject.SetActive(false);
                }

                foreach (var go in sao.objects)
                {
                    go.gameObject.SetActive(true);
                }
                break;
            }
        }
        foreach (var go in zuttoHyoujiSuruYatu)
        {
            go.SetActive(true);
        }
        _currentState = nextState;
    }

    public async void TimerStart()
    {
        OriginNetWorkTimer timer = await OriginNetWorkTimer.GetInstanceAsync();
        await NetWorkGameState.GetInstanceAsync();
        await UniTask.WaitUntil(() => timer.TimerTicking);
        _timerTweener = _timer.SetTime(timer.CurrentTime, () =>
        {
            NetWorkGameState.Instance.NwpCurrentGameState = GameState.Answer;
        });
    }

    private void AllSubmitAnswer()
    {
        _timerTweener?.Complete();
    }
}

[Serializable]
public class StateAndObjects
{
    public GameState state;
    public GameObject[] objects;
}

public enum PlayerRole
{
    Host,
    Guest,
    PlayingHost
}