using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger :SingletonMonoBehavior<UIChanger>
{

    private GameState _currentState;
    public GameState CurrentState 
    { 
        set
        {
            OnChangeState(value);
        }
        get => _currentState;
    }
    public PlayerRole Role { get; set; } = PlayerRole.Guest;

    [SerializeField] List<StateAndObjects> _stateActiveObject;
    [SerializeField] List<GameObject> zuttoHyoujiSuruYatu;
    private void OnEnable()
    {
        NetWorkGameState.Instance.ClientChangeScene += OnChangeState;
    }

    private void OnDisable()
    {
        NetWorkGameState.Instance.ClientChangeScene -= OnChangeState;
    }



    async void OnChangeState(GameState nextState)
    {
        if(nextState == GameState.QuestionInput)
        {
            if (Role == PlayerRole.Guest)
            {
                nextState = GameState.Waiting;
            }
            else
            {
                (await QuizData.GetInstanceAsync()).RPC_PlayerRemove(NetworkRunnerLocator.Instance.LocalPlayer);
            }
        }
        else if(nextState == GameState.GameSelect)
        {
            Role = PlayerRole.Guest;
        }
        else if(Role == PlayerRole.Host && nextState == GameState.Thinking)
        {
            nextState = GameState.GodMode;
        }
        foreach (var sao in _stateActiveObject) 
        { 
            if(sao.state == nextState)
            {
                foreach (Transform go in this.gameObject.transform)
                {
                    go.gameObject.SetActive(false);
                }

                foreach(var go in sao.objects)
                {
                    go.gameObject.SetActive(true);
                }
                break;
            }
        }
        foreach(var go in zuttoHyoujiSuruYatu)
        {
            go.SetActive(true);
        }
        _currentState = nextState;
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