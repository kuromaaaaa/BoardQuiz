using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] Text UI_Text1;
    [SerializeField] Text UI_Text2;
    // Update is called once per frame
    void Update()
    {
        UI_Text1.text = $"UI State : {UIManager.Instance.CurrentState.ToString()}";
        UI_Text2.text = $"Player Role :  {UIManager.Instance.Role.ToString()}";
    }
}
