using UnityEngine;
using UnityEngine.UI;

public class QuestionInput : MonoBehaviour
{
    [SerializeField] InputField Q;
    [SerializeField] InputField A;
    [SerializeField] Slider _time;
    public async void QuestionSend()
    {
        (await QuizData.GetInstanceAsync()).RPC_Question(Q.text, A.text, (int)_time.value);
    }
}
