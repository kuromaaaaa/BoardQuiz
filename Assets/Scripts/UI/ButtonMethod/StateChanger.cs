using UnityEngine;

public class StateChanger : MonoBehaviour
{
    [SerializeField] GameState _gameMode;
    public async void StateChange()
    {
        (await NetWorkGameState.GetInstanceAsync()).RPC_ChangeState(_gameMode);
    }
}
