using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public class GameModeSelecter : MonoBehaviour
{
    [SerializeField] QuizGameMode _gameMode;
    
    public async void GameModeChange()
    {
        (await QuizData.GetInstanceAsync()).GameMode = _gameMode;
    }
}
