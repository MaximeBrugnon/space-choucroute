using UnityEngine;

public class SprintJumpState : State
{
    bool grounded;

    float jumpHeight;

    Vector3 airVelocity;

    public SprintJumpState(Character _character) : base(_character)
    {
    }

    public override void Enter()
    {
        base.Enter();

        grounded = false;
        gravityValue = character.gravityValue;
        jumpHeight = character.jumpHeight;
        playerSpeed = character.playerSpeed;
        playerVelocity.y = 0;

        character.animator.SetFloat("speed", 0);
        character.animator.SetTrigger("sprintJump");
        Jump();
    }
    public override void HandleInput()
    {
        base.HandleInput();

        input = moveAction.ReadValue<Vector2>();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (grounded)
        {
            character.SetState(new LandingState(character));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!grounded)
        {

            move = character.playerVelocity;
            airVelocity = new Vector3(input.x, 0, input.y);

            move = move.x * character.cameraTransform.right.normalized + move.z * character.cameraTransform.forward.normalized;
            move.y = 0f;
            airVelocity = airVelocity.x * character.cameraTransform.right.normalized + airVelocity.z * character.cameraTransform.forward.normalized;
            airVelocity.y = 0f;
            character.controller.Move(playerVelocity * Time.deltaTime + (airVelocity * character.airControl + move * (1 - character.airControl)) * playerSpeed * Time.deltaTime);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
    }

    void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

}

