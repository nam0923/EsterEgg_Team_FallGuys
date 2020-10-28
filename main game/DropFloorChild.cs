using UnityEngine;

public class DropFloorChild : MonoBehaviour
{
    [SerializeField]
    DropFloorMgr DropMgr;

    public void OnEnable()
    {
        DropMgr.OnEnableFunc();
    }
}
