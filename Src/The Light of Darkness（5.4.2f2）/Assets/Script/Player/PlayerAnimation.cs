using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{

    private PlayerMove move;
    private new Animation animation;
    private PlayerAttack attack;


    void Start()
    {
        move = this.GetComponent<PlayerMove>();
        animation = this.GetComponent<Animation>();
        attack = this.GetComponent<PlayerAttack>();
    }

    void LateUpdate()
    {
        if (attack.state == PlayerState.ControlWalk)
        {
            if (move.state == ControlWalkState.Moving)
            {
                PlayAnim("Run");
            }
            else if (move.state == ControlWalkState.Idle)
            {
                PlayAnim("Idle");
            }
        }
        else if (attack.state == PlayerState.NormalAttack)
        {
            if (attack.attack_state == AttackState.Moving)
            {
                PlayAnim("Run");
            }
        }

    }

    void PlayAnim(string animName)
    {
        animation.CrossFade(animName);
    }
}
