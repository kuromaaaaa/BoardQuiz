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

    //SerializaFieldでListにしてStart時に辞書にする
    [SerializeField] private List<StateAndObjects> _stateActiveObject;
    private Dictionary<GameState, List<GameObject>> _stateActiveDic = new();
    
    [SerializeField] private List<GameObject> _alwaysActiveObject;
    
    [SerializeField] private SetTimer _timer;
    private Tweener _timerTweener;

    private void Awake()
    {
        base.Awake();
        foreach (var obj in _stateActiveObject)
        {
            _stateActiveDic.Add(obj.state, obj.objects);
        }
    }
    
    private void OnEnable()
    {
        NetWorkGameState.Instance.ClientChangeStateAction += OnChangeState;
    }

    private void OnDisable()
    {
        NetWorkGameState.Instance.ClientChangeStateAction -= OnChangeState;
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
        ChangeActiveObject(_stateActiveDic[nextState]);
        _currentState = nextState;
    }

    private void ChangeActiveObject(List<GameObject> activeObjects)
    {
        foreach (Transform go in this.gameObject.transform)
        {
            go.gameObject.SetActive(false);
        }

        foreach (var go in activeObjects)
        {
            go.gameObject.SetActive(true);
        }
        
        foreach (var go in _alwaysActiveObject)
        {
            go.SetActive(true);
        }
    }
    
    
    public async void TimerStart()
    {
        OriginNetWorkTimer timer = await OriginNetWorkTimer.GetInstanceAsync();
        await NetWorkGameState.GetInstanceAsync();
        await UniTask.WaitUntil(() => timer.NwpTimerTicking);
        _timerTweener = _timer.SetTime(timer.NwpCurrentTime);
    }
}

[Serializable]
public class StateAndObjects
{
    public GameState state;
    public List<GameObject> objects;
}

public enum PlayerRole
{
    Host,
    Guest,
    PlayingHost
}