using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SetTimer : MonoBehaviour
{
    [SerializeField] Image _pizza;
    [SerializeField] Text _text;
    [SerializeField]float _currentTime = 0.0f;

    public Tweener TimerTweener;

    public Action TweenComplete;
    public void SetTime(float time)
    {
        _currentTime = time;
        TimerTweener = DOTween.To(() => 
            _currentTime,
            (n) => _currentTime = n,
            0,
            time)
            .SetEase(Ease.Linear)
            .OnUpdate(()=> 
            {
                Debug.Log(_currentTime);
                _text.text = Math.Ceiling(_currentTime).ToString(); //Ø‚èã‚°•\Ž¦
                _pizza.fillAmount = _currentTime/time;
            }).OnComplete(()=> TweenComplete?.Invoke());
    }




    public void timerComplete()
    {
        TimerTweener.Complete();
    }
}
