using UnityEngine;
using UnityEngine.EventSystems;

public class DrawPen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Vector2 _startPos = Vector2.zero;
    bool _isClick = false;
    [SerializeField] float _nextDrawSqr = 1f;
    [SerializeField] GameObject _sketchBook;

    [SerializeField] GameObject _ball;

    public void OnPointerDown(PointerEventData eventData)
    {
        _startPos = Input.mousePosition;
        GameObject ball = Instantiate(_ball);
        ball.transform.parent = _sketchBook.transform;
        ball.transform.position = _startPos;
        _isClick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isClick = false;
    }

    void Update()
    {
        Vector2 currentPos = Input.mousePosition;
        if (_isClick && (currentPos - _startPos).sqrMagnitude > _nextDrawSqr)
        {
            _startPos = currentPos;
            GameObject ball = Instantiate(_ball);
            ball.transform.parent = _sketchBook.transform;
            ball.transform.position = currentPos;
            Debug.Log("‰“‚¢");
        }
    }
}
