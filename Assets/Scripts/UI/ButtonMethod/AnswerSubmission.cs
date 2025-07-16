using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class AnswerSubmission : MonoBehaviour
{
    bool _isSubmission;
    [SerializeField] InputField _answerInput;
    public async void Send()
    {
        _isSubmission = !_isSubmission;
        (await QuizData.GetInstanceAsync()).SubmittedDic.Set(NetworkRunnerLocator.Instance.LocalPlayer,_isSubmission);
        (await QuizData.GetInstanceAsync()).AnswerDic.Set(NetworkRunnerLocator.Instance.LocalPlayer, _answerInput.text);
    }

    public async void ProgressSend()
    {
        (await QuizData.GetInstanceAsync()).AnswerDic.Set(NetworkRunnerLocator.Instance.LocalPlayer, _answerInput.text);
    }
}
