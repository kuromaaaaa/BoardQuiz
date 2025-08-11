using UnityEngine;

public class GameModeSelecter : MonoBehaviour
{
    [SerializeField] QuizGameMode _gameMode;

    public async void GameModeChange()
    {
        (await QuizData.GetInstanceAsync()).NwpGameMode = _gameMode;
    }
}
