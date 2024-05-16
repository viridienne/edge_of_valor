using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReceiveInput : MonoBehaviour
{
    public static ReceiveInput Instance;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;
    public Action JumpInput;
    public Action LightAttackInput;
    public Action HeavyAttackInput;
    private void Awake()
    {
        Instance = this;
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        moveInput = move;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed) JumpInput?.Invoke();
    }
    public void LightAttack(InputAction.CallbackContext context)
    {
        if(context.performed) LightAttackInput?.Invoke();
    }
    public void HeavyAttack(InputAction.CallbackContext context)
    {
        if(context.performed) HeavyAttackInput?.Invoke();
    }
}
