using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SetTimer : MonoBehaviour
{
    [SerializeField] Image _pizza;
    [SerializeField] Text _text;
    [SerializeField] float _currentTime = 0.0f;

    public Tweener SetTime(float time, Action complete)
    {
        _text.gameObject.SetActive(true);
        _pizza.gameObject.SetActive(true);
        _currentTime = time;
        return DOTween.To(() =>
            _currentTime,
            (n) => _currentTime = n,
            0,
            time)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                Debug.Log(_currentTime);
                _text.text = Math.Ceiling(_currentTime).ToString(); //�؂�グ�\��
                _pizza.fillAmount = _currentTime / time;
            }).OnUpdate(() =>
            {
                _text.text = (Math.Ceiling(_currentTime).ToString());
                _pizza.fillAmount = _currentTime / time;
            }
            ).OnComplete(() => 
            {
                _text.gameObject.SetActive(false);
                _pizza.gameObject.SetActive(false);
                complete?.Invoke();
            });
    }
}
