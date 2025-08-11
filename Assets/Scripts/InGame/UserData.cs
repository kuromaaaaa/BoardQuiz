using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public class UserData : SingletonNetWorkBehaviour<UserData>, IPlayerLeft
{

    [Networked][Capacity(100)][UnitySerializeField] public NetworkDictionary<int, NetworkString<_32>> NwpUserDic { get; } = new();

    // PlayerJoinÇÕBasicSpawnerÇ™çsÇ§



    public async void PlayerLeft(PlayerRef player)
    {
        (await ChatData.GetInstanceAsync()).RPC_AddComment($"System:{NwpUserDic[player.PlayerId]}Ç™ëﬁé∫ÇµÇ‹ÇµÇΩ");
        NwpUserDic.Remove(player.PlayerId);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AddPlayer(PlayerRef player, string name)
    {
        NwpUserDic.Add(player.PlayerId, name);
    }
}
