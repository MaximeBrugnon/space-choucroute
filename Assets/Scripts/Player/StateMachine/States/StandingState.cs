using UnityEngine;

public class StandingState : State
{

    private Vector3 currentVelocity;
    private Vector3 cVelocity;
    private bool jump;
    private bool sprint;


    public StandingState(Character _character) : base(_character)
    {
    }

    public override void Enter()
    {
        base.Enter();

        jump = false;
        sprint = false;
        input = Vector2.zero;
        move = Vector3.zero;
        currentVelocity = Vector3.zero;
        playerVelocity = Vector3.zero;
    }

    public override void HandleInput()
    {
        input = moveAction.ReadValue<Vector2>();

        move = new Vector3(input.x, 0, input.y);
        move = move.x * character.cameraTransform.right.normalized + move.z * character.cameraTransform.forward.normalized;
        move.y = 0f;

        if (jumpAction.triggered && !character.IsInWater)
        {
            jump = true;
        }

        if (sprintAction.triggered && !character.IsInWater)
        {
            sprint = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float animationSpeed = input.magnitude;
        if (character.IsInWater)
        {
            animationSpeed *= character.GetWaterDragTimedFactor();
        }
        character.animator.SetFloat("speed", animationSpeed, character.speedDampTime, Time.deltaTime);

        if (sprint)
        {
            character.SetState(new SprintState(character));
        }
        if (jump)
        {
            character.SetState(new JumpingState(character));
        }

        if (character.IsSwiming)
        {
            character.SetState(new SwimingState(character));
        }
    }

    public override void PhysicsUpdate()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;

        if (character.IsInWater) // Drag
        {
            move *= character.GetWaterDragTimedFactor();
        }

        if (character.controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        currentVelocity = Vector3.SmoothDamp(currentVelocity, move, ref cVelocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + playerVelocity * Time.deltaTime);

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
