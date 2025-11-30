using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Text _userName;
    [SerializeField] private Text _scoreText;

    public void Initialize(string name, int score)
    {
        _userName.text = name;
        _scoreText.text = score.ToString();
    }

    public void ChangeScore(string score)
    {
        _scoreText.text = score;
    }

    public void Incorrect()
    {
        
    }
}
