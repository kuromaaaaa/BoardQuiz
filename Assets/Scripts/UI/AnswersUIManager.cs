using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

public class AnswersUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _answerPrefab;
    private Dictionary<int, Answer> _answerDic = new();
    
    QuizData _quizData;
    
    [SerializeField] AnswerColor _color;
    
    private async void OnEnable()
    {
        QuizData.Instance.ChangeAnswerDicAction += AnswerChange;

        if(_quizData == null) _quizData = await QuizData.GetInstanceAsync();
        foreach (var answer in _quizData.NwpAnswerDic)
        {
            GameObject answerBoard = Instantiate(_answerPrefab);
            answerBoard.transform.SetParent(this.transform, false);
            Answer ans = answerBoard.GetComponent<Answer>();
            ans.Initialize(
                (string)(await UserData.GetInstanceAsync()).NwpUserDic[answer.Key],
                (string)answer.Value);
            _answerDic[answer.Key] = ans;
        }
    }

    private void OnDisable()
    {
        QuizData.Instance.ChangeAnswerDicAction -= AnswerChange;

        foreach (Transform tf in this.gameObject.transform)
        {
            Destroy(tf.gameObject);
        }
        _answerDic = new();
    }

    async void AnswerChange()
    {
        if(_quizData == null) _quizData = await QuizData.GetInstanceAsync();
        foreach (var ans in _quizData.NwpAnswerDic)
        {
            _answerDic[ans.Key].AnswerUpdate((string)ans.Value);
        }
    }

    async void AnswerResult(List<int> ids)
    {
        SeManager.Instance.Play(SoundType.DrumRoll);

        await UniTask.WaitForSeconds(1000);
        
        string answer = (string)_quizData.NwpAnswer;

        foreach (var idsAndAnswer in _quizData.NwpAnswerDic)
        {
            if ((string)idsAndAnswer.Value == answer)
            {
                _answerDic[idsAndAnswer.Key].BoardColorChange(_color.Correct);
                if (idsAndAnswer.Key == NetworkRunnerLocator.Instance.LocalPlayer.PlayerId)
                {
                    SeManager.Instance.Play(SoundType.Correct);
                }
            }
            else
            {
                _answerDic[idsAndAnswer.Key].BoardColorChange(_color.Incorrect);
                if (idsAndAnswer.Key == NetworkRunnerLocator.Instance.LocalPlayer.PlayerId)
                {
                    SeManager.Instance.Play(SoundType.Incorrect);
                }
            }
        }
    }
}

[System.Serializable]
class AnswerColor
{
    public Color Correct;
    public Color Incorrect;
}