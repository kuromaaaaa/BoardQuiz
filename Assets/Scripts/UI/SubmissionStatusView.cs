using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubmissionStatusView : MonoBehaviour
{
    [SerializeField] Text text;
    private void OnEnable()
    {
        QuizData.Instance.ChangeSubmitUI += ViewUpdate;
        ViewUpdate();
    }

    private void OnDisable()
    {
        QuizData.Instance.ChangeSubmitUI -= ViewUpdate;
    }

    async void ViewUpdate()
    {
        QuizData data = (await QuizData.GetInstanceAsync());
        text.text = $"{data.SubmittedDic.Where((x) => x.Value == true).Count()}/{data.SubmittedDic.Count}";
    }

}
