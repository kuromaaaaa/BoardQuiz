using DG.Tweening;
using Fusion;
using System;

public class OriginNetWorkTimer : SingletonNetWorkBehaviour<OriginNetWorkTimer>
{
    [Networked]
    public float CurrentTime { get; set; }
    [Networked]
    public bool TimerTicking { get; private set; } = false;

    public Action TimerStartAction;
    public void TimerStart(float time)
    {
        TimerTicking = true;
        DOTween.To(() =>
            CurrentTime,
            (n) => CurrentTime = n,
            0,
            time)
            .SetEase(Ease.Linear).OnComplete(() => TimerTicking = false);

    }
}
