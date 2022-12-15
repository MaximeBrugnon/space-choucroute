using UnityEngine;
public class SwimingState : State
{
    private Vector3 currentVelocity;

    private Vector3 cVelocity;

    private float swimSpeed = 0.5f;
    private float waterEntryTime = 0f;

    public SwimingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        character.animator.SetTrigger("swim");
    }

    public override void HandleInput()
    {
        base.Enter();
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;

    }

    public override void LogicUpdate()
    {
	    waterEntryTime += Time.deltaTime;
        if (waterEntryTime >= 15f)
        {
            character.animator.SetTrigger("drown");
            //stateMachine.ChangeState(character.dies);
		}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        gravityVelocity.y = 0f;

        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);

        character.controller.Move(currentVelocity * Time.deltaTime * swimSpeed + gravityVelocity * Time.deltaTime);


        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }
    }

     public override void Exit()
    {
        base.Exit();
        character.animator.SetTrigger("move");
    }
}
