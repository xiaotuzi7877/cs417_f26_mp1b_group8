using UnityEngine;
using UnityEngine.InputSystem;

public class BreakOutTeleport : MonoBehaviour
{
    public Transform roomPosition;
    public Transform externalPosition;
    public InputActionReference action;

    private bool isOutside = false;

    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            isOutside = !isOutside;
            Transform target = isOutside ? externalPosition : roomPosition;
            transform.position = target.position;
        };
    }
}