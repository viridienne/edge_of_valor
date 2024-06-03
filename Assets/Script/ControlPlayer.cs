using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private ControlAnimatorV2 controlAnimator;
    [SerializeField] private Rigidbody2D rb;

    private ActionAnim lastAction;
    private float speed;
    private Transform tf;
    private Vector2 moveInput;
    private float groundY;
    private void Start()
    {
        tf = transform;
        rb = GetComponent<Rigidbody2D>();
        GetSpriteRenderer();
        currentAttack = 0;
        speed = moveSpeed;
        ReceiveInput.Instance.JumpInput += Jump;
        ReceiveInput.Instance.LightAttackInput += LightAttack;
        ReceiveInput.Instance.HeavyAttackInput += HeavyAttack;
    }

    private void OnDestroy()
    {
        ReceiveInput.Instance.JumpInput -= Jump;
        ReceiveInput.Instance.LightAttackInput -= LightAttack;
        ReceiveInput.Instance.HeavyAttackInput -= HeavyAttack;
    }

    private void Update()
    {
        moveInput = ReceiveInput.Instance.MoveInput;
        Countdown();

        if(attackReset <= 0f)
        {
            Move();
            Flip();
            HandleAnimation();
        }
    }

    public void Countdown()
    {
        if(attackCompletionTime > 0)
        {
            attackCompletionTime -= Time.deltaTime;
        }
    
        if (attackReset >= 0)
        {
            attackReset -= Time.deltaTime;
        }
        else
        {
            if (currentAttack >= 0)
            {
                StopGroundAttack();
            }
        }
    }
    
    [SerializeField] private float somersaultForce = 5f;
    [SerializeField] private float somersaultTime = 0.5f;
    [SerializeField] private float somersaultLimit = 1f;
    private float jumpTime;
    public void Jump()
    {
        if (attackReset >= 0) return;

        if (IsGrounded() && Time.time - jumpTime > 0)
        {
            rb.AddForce((Vector2)tf.up * jumpForce);
            jumpTime = Time.time + somersaultTime;
            PlayAction(ActionAnim.Jump);
        }
        else
        {
            if (IsGrounded()) return;
            
            if (Time.time - jumpTime > 0)
            {
                //if rigidbody is falling, return
                if (rb.velocity.y < 0) return;
                rb.AddForce((Vector2)tf.up * somersaultForce);
                jumpTime = Time.time + somersaultLimit;
                PlayAction(ActionAnim.Somersault);
            }
        }
    }
    
    private void Move()
    {
        var _current = tf.position;
        var _target = _current + new Vector3(moveInput.x, 0, 0);
        
        var _pos = Vector2.Lerp(_current, _target, speed * Time.deltaTime);
        tf.position = _pos;
    }

    private void Flip()
    {
        spriteRenderer.flipX = moveInput.x switch
        {
            > 0 => false,
            < 0 => true,
            _ => spriteRenderer.flipX
        };
    }
    
    private int currentAttack;
    private float attackReset;
    private float lastAttack;
    [InfoBox("attackCompletionTime = attackReset - attackMaxInterval \n attackReset = Time.deltaTime + attackDelay \n attackDelay = attackDelay2 = attackDelay3")]
    [SerializeField] private float attackCompletionTime;
    [SerializeField] private float attackMaxInterval;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDelay2;
    [SerializeField] private float attackDelay3;

    [SerializeField] private float fadeAttack1;
    [SerializeField] private float fadeAttack2;
    [SerializeField] private float fadeAttack3;
    
    public void LightAttack()
    {
        if (IsGrounded())
        {
            PlayGroundAttack();
        }
    }
    
    public void StopGroundAttack()
    {
        speed = moveSpeed;
        currentAttack = -1;

        if (IsGrounded())
        {
            if (moveInput.x > 0 || moveInput.x < 0)
            {
                PlayAction(ActionAnim.Run);
            }
            else
            {
                PlayAction(ActionAnim.Idle);
            }
        }
        else
        {
                    
        }
    }
    public void PlayGroundAttack()
    {
        if(attackCompletionTime > 0) return;
        
        speed = 0;
        moveInput = Vector2.zero;
        
        if (attackReset > 0)
        {
            currentAttack++;
        }
        else
        {
            currentAttack = 1;
        }

        var _delay = currentAttack switch
        {
            3 => attackDelay3,
            1 => attackDelay,
            2 => attackDelay2,
            _ => attackDelay
        };
        
        if (currentAttack > 0)
        {
            lastAttack = Time.time;
            attackReset = Time.deltaTime + _delay;
            attackCompletionTime = attackReset - attackMaxInterval;
        }

        switch (currentAttack)
        {
            case 1:
                PlayAction(ActionAnim.Attack1, fadeAttack1);
                break;
            case 2:
                PlayAction(ActionAnim.Attack2, fadeAttack2);
                break;
            case 3:
                PlayAction(ActionAnim.Attack3, fadeAttack3);
                break;
        }
        
        if (currentAttack >= 3)
        {
            currentAttack = 0;
        }
    }
    public void HeavyAttack()
    {

    }
    
    private bool IsGrounded()
    {
        var _isGrounded = Physics2D.Raycast(tf.position, -tf.up,groundDistance, 1 << LayerMask.NameToLayer("Ground"));
        return _isGrounded;
    }

    private void HandleAnimation()
    {
        if (!IsGrounded() || currentAttack > 0 || Time.time - jumpTime < 0)
        {
            return;
        }
        switch (moveInput.x)
        {
            case > 0 or < 0:
                PlayAction(ActionAnim.Run);
                break;
            case 0:
                PlayAction(ActionAnim.Idle);
                break;
        }
    }
    
    private void PlayAction(ActionAnim _action, float _crossFade = 0.1f)
    {
        if (lastAction == _action) return;
        
        controlAnimator.Play(_action,_crossFade);
        lastAction = _action;
    }
    
    
#if  UNITY_EDITOR
    [Button]
    public void GetAll()
    {
        GetSpriteRenderer();
        GetControlAnimator();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * groundDistance);
    }
#endif

    #region GET

    private void GetSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void GetControlAnimator()
    {
        controlAnimator = GetComponent<ControlAnimatorV2>();
    }

    #endregion

}
