using UnityEngine;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    [SerializeField] Text _userName;
    [SerializeField] Text _answer;
    [SerializeField] Image _image;
    public void Initialize(string userName, string answer)
    {
        _userName.text = userName;
        _answer.text = answer;
    }

    public void AnswerUpdate(string answer)
    {
        _answer.text = answer;
    }

    public void BoardColorChange(Color color)
    {
        _image.color = color;
    }
}
