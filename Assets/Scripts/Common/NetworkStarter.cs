using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkStarter : MonoBehaviour, INetworkRunnerCallbacks
{
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    private static NetworkStarter _instance;
    public static NetworkStarter Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<NetworkStarter>();
            }

            return _instance;
        }
    }


    public Action JoinGame;

    private NetworkRunner _runner;

    [Header("NetWorkStart")]
    [SerializeField] InputField _userNameInput;
    [SerializeField] InputField _passwordInput;

    public async void StartGame()
    {
        if (_userNameInput.text == string.Empty || _passwordInput.text == string.Empty) { return; }
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        NetworkRunnerLocator.Instance = _runner;


        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }


        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = _passwordInput.text,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        NetworkRunnerLocator.Name = _userNameInput.text;

        // Chatに自身が参加したメッセージを送信する
        (await ChatData.GetInstanceAsync()).RPC_AddComment($"System:{NetworkRunnerLocator.Name}が参加しました");

        // UserDataにIDとユーザー名を追加
        (await UserData.GetInstanceAsync()).RPC_AddPlayer(_runner.LocalPlayer, NetworkRunnerLocator.Name);

        if ((await NetWorkGameState.GetInstanceAsync()).NwpCurrentGameState == GameState.Title)
        {
            NetWorkGameState.Instance.NwpCurrentGameState = GameState.GameSelect;
        }
        else
        {
            UIManager.Instance.CurrentState = NetWorkGameState.Instance.NwpCurrentGameState;
        }
        JoinGame?.Invoke();
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }
}
