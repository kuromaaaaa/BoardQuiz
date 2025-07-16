using Fusion;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    [SerializeField] GameState _gameMode;
    public void StateChange()
    {
        NetWorkGameState.Instance.CurrentGameState = _gameMode;
    }
}
