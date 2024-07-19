using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// InputReader is a ScriptableObject that reads input from the player.
/// It defines several events that are raised when the corresponding input is detected.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Object/Input Reader", fileName = "New Input Reader")]
public class InputReader : ScriptableObject, ISODispose
{
    public event UnityAction JumpEvent;
    public event UnityAction LightAttackEvent;
    public event UnityAction HeavyAttackEvent;
    public event UnityAction<Vector2> MoveEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        MoveEvent?.Invoke(move);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed) JumpEvent?.Invoke();
    }
    public void LightAttack(InputAction.CallbackContext context)
    {
        if(context.performed) LightAttackEvent?.Invoke();
    }
    public void HeavyAttack(InputAction.CallbackContext context)
    {
        if(context.performed) HeavyAttackEvent?.Invoke();
    }

    private void OnDisable()
    {
        Dispose();
    }

    public DisposeAction OnDisableAction { get; set; }
    public void Dispose()
    {
        if (OnDisableAction == DisposeAction.ClearOnDisable)
        {
            JumpEvent = null;
            LightAttackEvent = null;
            HeavyAttackEvent = null;
            MoveEvent = null;
        }
    }
}
