using UnityEngine;
using System.Collections;


public enum ControlWalkState
{
    Moving,
    Idle,
}



public class PlayerMove : MonoBehaviour
{

    public float speed = 4;
    public ControlWalkState state = ControlWalkState.Idle;
    private PlayerDir dir;
    private PlayerAttack attack;
    private CharacterController controller;
    public bool isMoving = false;

    void Start()
    {
        dir = this.GetComponent<PlayerDir>();
        controller = this.GetComponent<CharacterController>();
        attack = this.GetComponent<PlayerAttack>();

    }

    void Update()
    {
        if (attack.state == PlayerState.ControlWalk)
        {
            float distance = Vector3.Distance(dir.targetPosition, transform.position);
            if (distance > 0.3f)
            {
                isMoving = true;
                state = ControlWalkState.Moving;
                controller.SimpleMove(transform.forward * speed);
            }
            else
            {
                isMoving = false;
                state = ControlWalkState.Idle;
            }
        }
    }

    public void SimpleMove(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
        controller.SimpleMove(transform.forward * speed);
    }
}
