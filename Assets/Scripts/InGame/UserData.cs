using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public class UserData : SingletonNetWorkBehaviour<UserData>, IPlayerLeft
{

    [Networked][Capacity(100)][UnitySerializeField] public NetworkDictionary<PlayerRef, NetworkString<_32>> UserList { get; } = new NetworkDictionary<PlayerRef, NetworkString<_32>>();

    public async void PlayerLeft(PlayerRef player)
    {
        (await ChatData.GetInstanceAsync()).RPC_AddComment($"System:{UserList[player]}‚ª‘ÞŽº‚µ‚Ü‚µ‚½");
        UserList.Remove(player);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AddPlayer(PlayerRef player ,string name )
    {
        UserList.Add(player, name);
    }
}
