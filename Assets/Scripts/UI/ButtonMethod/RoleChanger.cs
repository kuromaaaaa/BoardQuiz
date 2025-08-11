using UnityEngine;

public class RoleChanger : MonoBehaviour
{
    [SerializeField] PlayerRole changeRole;
    public void RoleChange()
    {
        UIManager.Instance.Role = changeRole;
    }
}
