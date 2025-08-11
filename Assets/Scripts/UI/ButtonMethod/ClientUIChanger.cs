using UnityEngine;

public class ClientUIChanger : MonoBehaviour
{
    [SerializeField] GameState nextState;

    public void ChangeState()
    {
        UIManager.Instance.CurrentState = nextState;
    }
}
