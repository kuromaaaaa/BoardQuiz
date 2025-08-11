using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommentAdd : MonoBehaviour
{
    [SerializeField] InputField _commentText;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ComAdd);
        _commentText.onEndEdit.AddListener((x) =>
        {
            if (Input.GetKeyDown(KeyCode.Return)) ComAdd();
        });
    }

    public async void ComAdd()
    {
        if (_commentText.text == string.Empty)
            return;
        (await ChatData.GetInstanceAsync()).RPC_AddComment($"{NetworkRunnerLocator.Name}:{_commentText.text}");
        _commentText.text = string.Empty;
        EventSystem.current.SetSelectedGameObject(_commentText.gameObject);
    }
}
