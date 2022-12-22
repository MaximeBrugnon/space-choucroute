using UnityEngine;
public class SwimingState : State
{
    private Vector3 currentVelocity;

    private Vector3 cVelocity;

    private readonly float swimSpeed = 0.5f;
    private float waterEntryTime = 0f;

    public SwimingState(Character _character) : base(_character)
    {
    }

    public override void Enter()
    {
        base.Enter();

        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity = Vector3.zero;


        character.animator.SetTrigger("swim");
    }

    public override void HandleInput()
    {
        base.HandleInput();
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;

    }

    public override void LogicUpdate()
    {
	    waterEntryTime += Time.deltaTime;
        if (waterEntryTime >= character.delayBeforeDrowing)
        {
            character.animator.SetTrigger("drown");
            //character.SetState(new DrowingState(character));
		}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
        currentVelocity.y = 0f;


        character.controller.Move(character.playerSpeed * swimSpeed * Time.deltaTime * currentVelocity);


		if (velocity.sqrMagnitude>0)
		{
            // facing direction
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity),character.rotationDampTime);
        }
    }

     public override void Exit()
    {
        base.Exit();
        character.animator.SetTrigger("move");
    }
}
