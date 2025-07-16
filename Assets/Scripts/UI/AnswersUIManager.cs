using System.Collections.Generic;
using UnityEngine;

public class AnswersUIManager : MonoBehaviour
{
    [SerializeField] GameObject _answerPrefab;
    Dictionary<PlayerPrefs, Answer> AnswerDic;
    private async void OnEnable()
    {
        QuizData q = (await QuizData.GetInstanceAsync());
        QuizData.Instance.ChangeAnswerDic += AnswerChange;
        foreach(var answer in q.AnswerDic)
        {
        }
    }

    private void OnDisable()
    {
        foreach(Transform tf in this.gameObject.transform)
        {
            Destroy(tf.gameObject);
        }
    }

    void AnswerChange()
    {

    }
}
