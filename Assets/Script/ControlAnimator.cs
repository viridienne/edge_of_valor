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
    Attack,
    Hit,
    Die
}
public class ControlAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
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
                break;
            case ActionAnim.Run:
                break;
        }
    }
}
