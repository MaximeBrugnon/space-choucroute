using UnityEngine;
public class SprintState : State
{
    Vector3 currentVelocity;

    bool sprint;
    bool sprintJump;
    Vector3 cVelocity;

    public SprintState(Character _character) : base(_character)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sprint = false;
        sprintJump = false;
        input = Vector2.zero;
        move = Vector3.zero;
        currentVelocity = Vector3.zero;
    }

    public override void HandleInput()
    {
        input = moveAction.ReadValue<Vector2>();

        move = new Vector3(input.x, 0, input.y);
        move = move.x * character.cameraTransform.right.normalized + move.z * character.cameraTransform.forward.normalized;
        move.y = 0f;

        if (input.sqrMagnitude == 0f || character.IsInWater)
        {
            sprint = false;
        }
        else
        {
            sprint = true;
        }

        if (jumpAction.triggered)
        {
            sprintJump = true;

        }
    }

    public override void LogicUpdate()
    {
        if (sprint)
        {
            character.animator.SetFloat("speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);
        }
        else
        {
            character.SetState(new StandingState(character));
        }
        if (sprintJump)
        {
            character.SetState(new SprintJumpState(character));
        }
    }

    public override void PhysicsUpdate()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;

        if (character.controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        currentVelocity = Vector3.SmoothDamp(currentVelocity, move, ref cVelocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * character.sprintSpeed + playerVelocity * Time.deltaTime);

        if (move.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(move), character.rotationDampTime);
        }
    }

    public override void Exit()
    {
        base.Exit();

        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (move.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(move);
        }
    }

}
