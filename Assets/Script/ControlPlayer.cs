using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private ControlAnimator controlAnimator;
    private void Start()
    {
        GetSpriteRenderer();
    }

    private float groundY;
    private void Update()
    {
        var _moveInput = ReceiveInput.Instance.MoveInput;
        var _target = new Vector3(_moveInput.x, 0, _moveInput.y);
        var _current = transform.position;
        if (IsGrounded())
        {
            _target.y = groundY;
        }
        var _pos = Vector2.Lerp(_current, _target, moveSpeed * Time.deltaTime);
        transform.position = _pos;

        spriteRenderer.flipX = _moveInput.x switch
        {
            > 0 => false,
            < 0 => true,
            _ => spriteRenderer.flipX
        };
    }
    

    public bool IsGrounded()
    {
        RaycastHit _hit;
        bool _isGrounded = false;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, groundDistance))
        {
            groundY = _hit.point.y;
            _isGrounded = true;
        }

        return _isGrounded;
    }
    
    public void GetSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void GetControlAnimator()
    {
        controlAnimator = GetComponent<ControlAnimator>();
    }
    
    public void PlayAction(ActionAnim _action)
    {
        controlAnimator.PlayAction(_action);
    }

#if UNITY_EDITOR
    
[Button]
public void GetAll()
{
    GetSpriteRenderer();
    GetControlAnimator();
}
#endif
    
}
