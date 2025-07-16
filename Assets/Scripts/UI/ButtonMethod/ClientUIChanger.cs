using UnityEngine;

public class ClientUIChanger : MonoBehaviour
{
    [SerializeField] GameState nextState;

    public void ChangeState()
    {
        UIChanger.Instance.CurrentState = nextState;
    }
}
