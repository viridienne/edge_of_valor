using UnityEngine;
using UnityEngine.InputSystem;

public class ReceiveInput : MonoBehaviour
{
    public static ReceiveInput Instance;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;
    private void Awake()
    {
        Instance = this;
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        moveInput = move;
    }
}
