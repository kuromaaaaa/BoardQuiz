using Fusion;

public class UserData : SingletonNetWorkBehaviour<UserData>, IPlayerLeft
{

    [Networked][Capacity(100)][UnitySerializeField] public NetworkDictionary<int, NetworkString<_32>> NwpUserDic { get; } = new();

    // PlayerJoinはNetWorkStarterが呼ぶ



    public async void PlayerLeft(PlayerRef player)
    {
        (await ChatData.GetInstanceAsync()).RPC_AddComment($"System:{NwpUserDic[player.PlayerId]} が退室しました");
        NwpUserDic.Remove(player.PlayerId);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AddPlayer(PlayerRef player, string playerName)
    {
        NwpUserDic.Add(player.PlayerId, playerName);
    }
}
