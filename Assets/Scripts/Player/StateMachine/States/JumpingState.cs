using UnityEngine;

public class JumpingState:State
{

    float jumpHeight;

    // Local isGrounded for timing issue
    private bool isGrounded;

    Vector3 airbornMove;

    public JumpingState(Character _character) : base(_character)
	{
	}

    public override void Enter()
	{
		base.Enter();

        gravityValue = character.gravityValue;
        jumpHeight = character.jumpHeight;
        playerSpeed = character.playerSpeed;
        playerVelocity.y = 0;
        isGrounded = false;

        character.animator.SetFloat("speed", 0);
        character.animator.SetTrigger("jump");
        Jump();
	}
	public override void HandleInput()
	{
		base.HandleInput();

        input = moveAction.ReadValue<Vector2>();

        move = character.playerVelocity;
        move = move.x * character.cameraTransform.right.normalized + move.z * character.cameraTransform.forward.normalized;
        move.y = 0f;

        airbornMove = new Vector3(input.x, 0, input.y);
        airbornMove = airbornMove.x * character.cameraTransform.right.normalized + airbornMove.z * character.cameraTransform.forward.normalized;
        airbornMove.y = 0f;
    }

	public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded)
		{
            character.SetState(new LandingState(character));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

		if (!isGrounded)
		{
            character.controller.Move(playerVelocity * Time.deltaTime+ (airbornMove*character.airControl+move*(1- character.airControl))*playerSpeed*Time.deltaTime);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        isGrounded = character.controller.isGrounded;
    }

    void Jump()
    {
        // Science bitch, don't tweak that
        playerVelocity.y += Mathf.Sqrt(-2f * gravityValue * jumpHeight);
    }

}

