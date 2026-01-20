using DG.Tweening;
using Fusion;
using System;

public class OriginNetWorkTimer : SingletonNetWorkBehaviour<OriginNetWorkTimer>
{
    private Tweener _timeTween;
    
    [Networked]
    public float NwpCurrentTime { get; set; }
    [Networked]
    public bool NwpTimerTicking { get; private set; } = false;
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TimerStart(float time)
    {
        NwpTimerTicking = true;
        _timeTween = DOTween.To(() =>
            NwpCurrentTime,
            (n) => NwpCurrentTime = n,
            0,
            time)
            .SetEase(Ease.Linear).OnComplete(async () =>
            {
                NwpTimerTicking = false;
                (await NetWorkGameState.GetInstanceAsync()).RPC_ChangeState(GameState.Answer);
            });
    }

    /// <summary>
    /// タイマーを終了してStateをAnswerに変更する
    /// </summary>
    public void RPC_TimerComplete()
    {
        _timeTween.Complete();
    }
}
