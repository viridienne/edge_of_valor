using UnityEngine;
public enum ActionAnim
{
    Idle,
    Walk,
    Run,
    Jump,
    Somersault,
    Fall,
    OnAir,
    Attack,
    Attack1,
    Attack2,
    Attack3,
    CancelAttack,
    AirAttack1,
    AirAttack2,
    AirAttack3,
    AirAttack3_End,
}
public class ControlAnimatorV2 : MonoBehaviour
{
    private Animator animator;
    private static readonly string LAYER_COMBAT = "Combat.";
    private static readonly string STATE_ATTACK_1  = LAYER_COMBAT + "Attack1";
    private static readonly string STATE_ATTACK_2 = LAYER_COMBAT + "Attack2";
    private static readonly string STATE_ATTACK_3 = LAYER_COMBAT + "Attack3";
    private static readonly string STATE_AIR_ATTACK_1 = LAYER_COMBAT + "AirAttack1";
    private static readonly string STATE_AIR_ATTACK_2 = LAYER_COMBAT + "AirAttack2";
    private static readonly string STATE_AIR_ATTACK_3_BEGIN = LAYER_COMBAT + "AirAttack3_Begin";
    private static readonly string STATE_AIR_ATTACK_3_END = LAYER_COMBAT + "AirAttack3_End";
    private static readonly string STATE_AIR_ATTACK_3_LOOP = LAYER_COMBAT + "AirAttack3_Loop";
    private int STATE_ATTACK_1_HASH => Animator.StringToHash(STATE_ATTACK_1);
    private int STATE_ATTACK_2_HASH => Animator.StringToHash(STATE_ATTACK_2);
    private int STATE_ATTACK_3_HASH => Animator.StringToHash(STATE_ATTACK_3);
    
    private int STATE_AIR_ATTACK_1_HASH => Animator.StringToHash(STATE_AIR_ATTACK_1);
    private int STATE_AIR_ATTACK_2_HASH => Animator.StringToHash(STATE_AIR_ATTACK_2);
    private int STATE_AIR_ATTACK_3_BEGIN_HASH => Animator.StringToHash(STATE_AIR_ATTACK_3_BEGIN);
    private int STATE_AIR_ATTACK_3_END_HASH => Animator.StringToHash(STATE_AIR_ATTACK_3_END);
    private int STATE_AIR_ATTACK_3_LOOP_HASH => Animator.StringToHash(STATE_AIR_ATTACK_3_LOOP);
    
    private int STATE_COMBAT_IDLE_HASH => Animator.StringToHash(LAYER_COMBAT + "Idle");
    private int STATE_COMBAT_RUN_HASH => Animator.StringToHash(LAYER_COMBAT + "Run");
    private int STATE_JUMP_HASH => Animator.StringToHash(LAYER_COMBAT + "Jump");
    private int STATE_SOMERSAULT_HASH => Animator.StringToHash(LAYER_COMBAT + "Somersault");
    private int STATE_FALL_HASH => Animator.StringToHash(LAYER_COMBAT + "Fall");

    private ActionAnim lastAction;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    private bool IsPlaying(int _state, int _layer)
    {
        return animator.GetCurrentAnimatorStateInfo(_layer).fullPathHash == _state;
    }
    
    public void Play(ActionAnim _action, float _crossFade)
    {
        int _state = 0;
        switch (_action)
        {
            case ActionAnim.Attack1:
                _state = STATE_ATTACK_1_HASH;
                break;
            case ActionAnim.Attack2:
                _state = STATE_ATTACK_2_HASH;
                break;
            case ActionAnim.Attack3:
                _state = STATE_ATTACK_3_HASH;
                break;
            case ActionAnim.Idle:
                _state = STATE_COMBAT_IDLE_HASH;
                break;
            case ActionAnim.Run:
                _state = STATE_COMBAT_RUN_HASH;
                break;
            case ActionAnim.Jump:
                _state = STATE_JUMP_HASH;
                break;
            case ActionAnim.Fall:
                _state = STATE_FALL_HASH;
                break;
            case ActionAnim.Somersault:
                _state = STATE_SOMERSAULT_HASH;
                break;
            case ActionAnim.CancelAttack:
                _state = STATE_COMBAT_IDLE_HASH;
                break;
            case ActionAnim.AirAttack1:
                _state = STATE_AIR_ATTACK_1_HASH;
                break;
            case ActionAnim.AirAttack2:
                _state = STATE_AIR_ATTACK_2_HASH;
                break;
            case ActionAnim.AirAttack3:
                _state = STATE_AIR_ATTACK_3_BEGIN_HASH;
                break;
            case ActionAnim.AirAttack3_End:
                _state = STATE_AIR_ATTACK_3_END_HASH;
                break;
        }
        Debug.Log("Play -> " + _action);
        animator.CrossFade(_state, _crossFade);
    }
    
    
}
