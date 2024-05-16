using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
    Attack_01,
    Attack_02,
    Attack_03,
}
public class ControlAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private const string COMBAT_IDLE = "CombatIdle";
    private const string COMBAT_RUN = "CombatRun";
    private const string JUMP = "Jump";
    private const string SOMERSAULT = "Somersault";
    private const string FALL = "Fall";
    private const string ATK = "Atk";
    private int COMBAT_RUN_HASH => Animator.StringToHash(COMBAT_RUN);
    private int COMBAT_IDLE_HASH => Animator.StringToHash(COMBAT_IDLE);
    private int JUMP_HASH => Animator.StringToHash(JUMP);
    private int SOMERSAULT_HASH => Animator.StringToHash(SOMERSAULT);
    private int FALL_HASH => Animator.StringToHash(FALL);
    private int ATK_HASH => Animator.StringToHash(ATK);
    
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
            case ActionAnim.Attack_01:
                animator.SetInteger(ATK_HASH,1);
                break;
            case ActionAnim.Attack_02:
                animator.SetInteger(ATK_HASH,2);
                break;
            case ActionAnim.Attack_03:
                animator.SetInteger(ATK_HASH,3);
                break;
        }
    }
}
