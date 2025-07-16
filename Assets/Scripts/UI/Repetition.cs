using DG.Tweening;
using UnityEngine;

public class Repetition : MonoBehaviour
{
    [SerializeField] RectTransform _transform;
    [SerializeField] Vector2 Pos1;
    [SerializeField] Vector2 Pos2;
    [SerializeField] float _moveSpeed;

    bool _state = true;
    bool state 
    { 
        get => _state; 
        set 
        {
            _transform.DOAnchorPos((value ? Pos1 : Pos2), _moveSpeed);
            _state = value;
        } 
    }

    public void Move()
    {
        state = !state;
    }
}
