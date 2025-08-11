using Cysharp.Threading.Tasks;
using UnityEngine;

public class CommentView : MonoBehaviour
{
    [SerializeField] GameObject _textBlock;
    [SerializeField] Color _systemColor;
    [SerializeField] Color _userColor;
    [SerializeField] Color _etcColor;
    private void OnEnable()
    {
        ChatData.Instance.AddCommentAction += CommentInstantiate;
    }

    private void OnDisable()
    {
        ChatData.Instance.AddCommentAction -= CommentInstantiate;
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => ChatData.Instance.IsSpawned);
        AllCommentInstantiate();
    }

    void AllCommentInstantiate()
    {
        foreach (var comment in ChatData.Instance.NwpCommentList)
        {
            CommentInstantiate((string)comment);
        }
    }

    void CommentInstantiate(string comment)
    {
        GameObject text = Instantiate(_textBlock);
        string[] s = comment.Split(':');
        Color color = _etcColor;
        if (s[0] == NetworkRunnerLocator.Name)
        {
            color = _userColor;
        }
        else if (s[0] == "System")
        {
            color = _systemColor;
        }
        text.GetComponent<Comment>().Initialize(s[0], s[1], color);
        text.transform.SetParent(transform, false);
    }

}
