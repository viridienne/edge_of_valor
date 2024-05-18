using Sirenix.OdinInspector;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private ControlAnimator controlAnimator;
    [SerializeField] private Rigidbody2D rb;

    private float speed;
    private Transform tf;
    private Vector2 moveInput;
    private float groundY;
    private bool isGrounded;
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
        IsGrounded();
        
        if(attackReset < 0)
        {
            Move();
            Flip();
            HandleAnimation();
        }

        Countdown();
    }

    public void Countdown()
    {
        if (attackReset >= 0)
        {
            attackReset -= Time.deltaTime;
        }
        else
        {
            speed = moveSpeed;
            PlayAttack(ActionAnim.CancelAttack);
        }
    }
    
    [SerializeField] private float somersaultForce = 5f;
    [SerializeField] private float somersaultTime = 0.5f;
    [SerializeField] private float somersaultLimit = 1f;
    private float jumpTime;
    private bool hasJumped;
    public void Jump()
    {
        if (attackReset >= 0) return;
        
        if (isGrounded)
        {
            PlayAction(ActionAnim.OnAir);
            PlayAction(ActionAnim.Jump);
            rb.AddForce((Vector2)tf.up * jumpForce);
            jumpTime = Time.time + somersaultTime;
            hasJumped = true;
            isGrounded = false;
        }
        else
        {
            if (Time.time - jumpTime > 0)
            {
                rb.AddForce((Vector2)tf.up * somersaultForce);
                PlayAction(ActionAnim.Somersault);
                jumpTime = Time.time + somersaultLimit;
                hasJumped = true;
                isGrounded = false;
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
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDelay2;
    [SerializeField] private float attackDelay3;
    
    public void LightAttack()
    {
        speed = 0;
        var _delay = currentAttack switch
        {
            3 => attackDelay3,
            1 => attackDelay,
            2 => attackDelay2,
            _ => attackDelay
        };
        
        if (attackReset > 0)
        {
            currentAttack++;
            if (currentAttack > 3)
            {
                currentAttack = 1;
            }
        }
        else
        {
            currentAttack = 1;
        }

        attackReset = Time.deltaTime + _delay;
        PlayAttack(ActionAnim.LightAttack);
    }

    public void PlayAttack(ActionAnim _attack)
    {
        switch (_attack)
        {
            case ActionAnim.LightAttack:
                PlayAction(ActionAnim.LightAttack);
                break;
            case 0:
                PlayAction(ActionAnim.CancelAttack);
                break;
        }
    }
    public void HeavyAttack()
    {

    }
    
    private void IsGrounded()
    {
        isGrounded = Physics2D.Raycast(tf.position, -tf.up,groundDistance, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void HandleAnimation()
    {
        if (!isGrounded)
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
    
    private void PlayAction(ActionAnim _action)
    {
        controlAnimator.PlayAction(_action);
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
        controlAnimator = GetComponent<ControlAnimator>();
    }

    #endregion

}
