using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    [SerializeField] Text _userName;
    [SerializeField] Text _scoreText;

    public void Initialize(string name)
    {
        _userName.text = name;
        _scoreText.text = 0.ToString();
    }

    public void ScoreChane(string score)
    {
        _scoreText.text = score;
    }
}
