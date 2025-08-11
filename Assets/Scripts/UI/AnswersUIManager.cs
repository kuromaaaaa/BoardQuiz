using System.Collections.Generic;
using UnityEngine;

public class AnswersUIManager : MonoBehaviour
{
    [SerializeField] GameObject _answerPrefab;
    Dictionary<int, Answer> AnswerDic = new();
    private async void OnEnable()
    {
        QuizData.Instance.ChangeAnswerDic += AnswerChange;

        QuizData q = (await QuizData.GetInstanceAsync());
        foreach (var answer in q.NwpAnswerDic)
        {
            GameObject answerBoard = Instantiate(_answerPrefab);
            answerBoard.transform.SetParent(this.transform, false);
            Answer ans = answerBoard.GetComponent<Answer>();
            ans.Initialize(
                (string)(await UserData.GetInstanceAsync()).NwpUserDic[answer.Key],
                (string)answer.Value);
            AnswerDic[answer.Key] = ans;
        }
    }

    private void OnDisable()
    {
        QuizData.Instance.ChangeAnswerDic -= AnswerChange;

        foreach (Transform tf in this.gameObject.transform)
        {
            Destroy(tf.gameObject);
        }
        AnswerDic = new();
    }

    async void AnswerChange()
    {
        Debug.Log("ìöÇ¶ÇÃïœçX");
        foreach (var ans in (await QuizData.GetInstanceAsync()).NwpAnswerDic)
        {
            AnswerDic[ans.Key].AnswerUpdate((string)ans.Value);
        }
    }
}
