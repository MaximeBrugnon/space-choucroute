using UnityEngine;

public class StandingState: State
{
    
    float gravityValue;
    bool jump;   
    Vector3 currentVelocity;
    bool grounded;
    bool sprint;
    float playerSpeed;

    private readonly float inWaterSpeed = 0.5f;

    Vector3 cVelocity;

    public StandingState(Character _character) : base(_character)
	{
	}

    public override void Enter()
    {
        base.Enter();

        jump = false;
        sprint = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;    
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (jumpAction.triggered && !character.isInWater)
        {
            jump = true;
		}
		if (sprintAction.triggered && !character.isInWater)
		{
            sprint = true;
		}

        input = moveAction.ReadValue<Vector2>();

        float speedLimiter = character.isInWater ? inWaterSpeed : 1.0f; // slower in water
        velocity = new Vector3(input.x, 0, input.y) * speedLimiter;

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
     
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float speedLimiter = character.isInWater ? inWaterSpeed : 1.0f; // slower in water
        character.animator.SetFloat("speed", input.magnitude * speedLimiter, character.speedDampTime, Time.deltaTime);

        if (sprint)
		{
            character.SetState(new SprintState(character));
        }    
        if (jump)
        {
            character.SetState(new JumpingState(character));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }
       
        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity,ref cVelocity, character.velocityDampTime);
        character.controller.Move(playerSpeed * Time.deltaTime * currentVelocity + gravityVelocity * Time.deltaTime);
  
		if (velocity.sqrMagnitude>0)
		{
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity),character.rotationDampTime);
        }
        
    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

}
