using Sirenix.OdinInspector;
using UnityEngine;

public class ControlAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private const string COMBAT_IDLE = "CombatIdle";
    private const string COMBAT_RUN = "CombatRun";
    private const string JUMP = "Jump";
    private const string SOMERSAULT = "Somersault";
    private const string FALL = "Fall";
    private const string ATTACK = "Attack";
    private int COMBAT_RUN_HASH => Animator.StringToHash(COMBAT_RUN);
    private int COMBAT_IDLE_HASH => Animator.StringToHash(COMBAT_IDLE);
    private int JUMP_HASH => Animator.StringToHash(JUMP);
    private int SOMERSAULT_HASH => Animator.StringToHash(SOMERSAULT);
    private int FALL_HASH => Animator.StringToHash(FALL);
    private int ATTACK_HASH => Animator.StringToHash(ATTACK);
    
    [Button]
    public void GetAnimator()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayAction(ActionAnim _action)
    {
        switch (_action)
        {
            case ActionAnim.Idle:
                animator.SetBool(COMBAT_RUN_HASH,false);
                animator.SetBool(COMBAT_IDLE_HASH,true);
                break;
            case ActionAnim.Run:
                animator.SetBool(COMBAT_RUN_HASH,true);
                animator.SetBool(COMBAT_IDLE_HASH, false);
                break;
            case ActionAnim.OnAir:
                animator.SetBool(COMBAT_RUN_HASH,false);
                animator.SetBool(COMBAT_IDLE_HASH, false);
                break;
            case ActionAnim.Jump:
                animator.SetTrigger(JUMP_HASH);
                break;
            case ActionAnim.Somersault:
                animator.SetTrigger(SOMERSAULT_HASH);
                break;
            case ActionAnim.Fall:
                animator.SetTrigger(FALL_HASH);
                break;
            case ActionAnim.Attack:
                animator.SetBool(COMBAT_RUN_HASH,false);
                animator.SetBool(COMBAT_IDLE_HASH, false);
                animator.SetTrigger(ATTACK_HASH);
                break;
            case ActionAnim.CancelAttack:
                animator.ResetTrigger(ATTACK_HASH);
                break;
        }
    }
}
