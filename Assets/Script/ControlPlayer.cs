using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float atkJumpForce = 5f;
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
        ControlCamera.Instance.SetMainPlayer(tf);
    }

    private void OnDestroy()
    {
        ReceiveInput.Instance.JumpInput -= Jump;
        ReceiveInput.Instance.LightAttackInput -= LightAttack;
        ReceiveInput.Instance.HeavyAttackInput -= HeavyAttack;
    }

    private void FixedUpdate()
    {
        IsGrounded();
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

    private void LateUpdate()
    {
        ClampPos();
    }

    private void ClampPos()
    {
        var mapController = MapManager.Instance.MapController;
        var minX = mapController.MinX;
        var maxX = mapController.MaxX;
        var minY = mapController.MinY;
        var maxY = mapController.MaxY;
        if (tf.position.x > minX && tf.position.x < maxX && tf.position.y > minY && tf.position.y < maxY) return;
        
        var _pos = tf.position;
        _pos.x = Mathf.Clamp(_pos.x, minX, maxX);
        _pos.y = Mathf.Clamp(_pos.y, minY, maxY);
        tf.position = _pos;
        
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
    private bool jumped;
    private bool isInAtk3;
    private bool somersaulted;
    public void Jump()
    {
        if (attackReset >= 0) return;

        if (isGrounded && Time.time - jumpTime > 0)
        {
            rb.AddForce((Vector2)tf.up * jumpForce);
            jumpTime = Time.time + somersaultTime;
            PlayAction(ActionAnim.Jump);
            
            //reset attack
            currentAttack = -1;
            attackReset = 0;
            attackCompletionTime = 0;
            jumped = true;
        }
        else
        {
            if (isGrounded) return;
            
            if (Time.time - jumpTime > 0)
            {
                //if rigidbody is falling, return
                if (rb.velocity.y < 0) return;
                rb.AddForce((Vector2)tf.up * somersaultForce);
                jumpTime = Time.time + somersaultLimit;
                PlayAction(ActionAnim.Somersault);
                jumped = true;
                somersaulted = true;
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
    [InfoBox("attackCompletionTime = attackReset - attackMaxInterval \n attackReset = Time.deltaTime + attackDelay \n attackDelay = attackDelay2 = attackDelay3")]
    [SerializeField] private float attackCompletionTime;
    [SerializeField] private float attackMaxInterval;
    
    [Title("Ground Attack")]
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDelay2;
    [SerializeField] private float attackDelay3;

    [SerializeField] private float fadeAttack1;
    [SerializeField] private float fadeAttack2;
    [SerializeField] private float fadeAttack3;
    
    [Title("Air Attack")]
    [SerializeField] private float airAttackDelay;
    [SerializeField] private float airAttackDelay2;
    [SerializeField] private float airAttackDelay3;
    [SerializeField] private float airFadeAttack1;
    [SerializeField] private float airFadeAttack2;
    [SerializeField] private float airFadeAttack3;
    
    public void LightAttack()
    {
        if (isGrounded)
        {
            PlayGroundAttack();
        }
        else
        {
            PlayAirAttack();
        }
    }
    public void PlayAirAttack()
    {
        if (attackCompletionTime > 0) return;
        
        speed = 0;
        moveInput = Vector2.zero;
        
        if (attackReset > 0)
        {
            currentAttack++;
            if (currentAttack >= 2)
            {
                if (!somersaulted)
                {
                    return;
                }
            }
        }
        else
        {
            currentAttack = 1;
        }

        var _delay = currentAttack switch
        {
            3 => airAttackDelay3,
            1 => airAttackDelay,
            2 => airAttackDelay2,
            _ => attackDelay
        };
        
        if (currentAttack > 0)
        {
            attackReset = Time.deltaTime + _delay;
            attackCompletionTime = attackReset - attackMaxInterval;
            rb.AddForce((Vector2)tf.up * atkJumpForce);
        }

        switch (currentAttack)
        {
            case 1:
                PlayAction(ActionAnim.AirAttack1, airFadeAttack1);
                break;
            case 2:
                PlayAction(ActionAnim.AirAttack2, airFadeAttack2);
                break;
            case 3:
                PlayAction(ActionAnim.AirAttack3, airFadeAttack3);
                isInAtk3 = true;
                break;
        }
        
        if (currentAttack >= 3)
        {
            currentAttack = 0;
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
    
    public void StopGroundAttack()
    {
        speed = moveSpeed;
        currentAttack = -1;

        if (isGrounded)
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
    }

    public void HeavyAttack()
    {

    }
    
    private bool isGrounded;
    private void IsGrounded()
    {
        isGrounded  = Physics2D.Raycast(tf.position, -tf.up,groundDistance, 1 << LayerMask.NameToLayer("Ground"));
        if (jumped && isGrounded)
        {
            if (isInAtk3)
            {
                PlayAction(ActionAnim.AirAttack3_End,fadeAttack3);
                speed = moveSpeed;
            }
            else
            {
                attackReset = 0;
                attackCompletionTime = 0;
            }

            currentAttack = -1;
            jumped = false;
            isInAtk3 = false;
            somersaulted = false;
        }
    }

    private void HandleAnimation()
    {
        if (!isGrounded || currentAttack > 0 || Time.time - jumpTime < 0)
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
