using Fusion;
using UnityEngine;

public class ScoreData : SingletonNetWorkBehaviour<ScoreData>
{
    [Networked][Capacity(100)][UnitySerializeField] public NetworkDictionary<PlayerRef, int> ScoreList { get; } = new ();



}
