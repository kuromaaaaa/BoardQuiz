using UnityEngine;
using UnityEngine.UI;

public class AnswerSubmission : MonoBehaviour
{
    bool _isSubmission;
    [SerializeField] InputField _answerInput;
    [SerializeField] Image _checkMark;
    [SerializeField] Sprite _checkTrue;
    [SerializeField] Sprite _checkFalse;
    public async void Send(bool submit)
    {
        _isSubmission = submit;

        Debug.Log("send");

        (await QuizData.GetInstanceAsync()).RPC_SendAnswer(
            NetworkRunnerLocator.Instance.LocalPlayer.PlayerId, _isSubmission, _answerInput.text);
        Debug.Log($"ìöÇ¶íÒèo : PlayerID {NetworkRunnerLocator.Instance.LocalPlayer.PlayerId} : ìöÇ¶ {_answerInput.text}");
        _checkMark.sprite = _isSubmission ? _checkTrue : _checkFalse;
    }
}
