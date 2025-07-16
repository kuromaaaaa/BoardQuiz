using UnityEngine;

public class RoleChanger : MonoBehaviour
{
    [SerializeField] PlayerRole changeRole;
    public void RoleChange()
    {
        UIChanger.Instance.Role = changeRole;
    }
}
