using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SubmissionStatusView : MonoBehaviour
{
    [SerializeField] Text text;
    private void OnEnable()
    {
        QuizData.Instance.ChangeSubmitDic += ViewUpdate;
        ViewUpdate();
    }

    private void OnDisable()
    {
        QuizData.Instance.ChangeSubmitDic -= ViewUpdate;
    }

    async void ViewUpdate()
    {
        QuizData data = (await QuizData.GetInstanceAsync());
        int submitCount = data.NwpSubmittedDic.Where((x) => x.Value == true).Count();
        if (submitCount == 0)
        {
            text.text = string.Empty;
        }
        else
        {
            text.text = $"{submitCount}/{data.NwpSubmittedDic.Count}";
        }
    }

}
