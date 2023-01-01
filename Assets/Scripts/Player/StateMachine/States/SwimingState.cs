using UnityEngine;
public class SwimingState : State
{
    private Vector3 currentVelocity;

    private Vector3 cVelocity;

    private float waterEntryTime = 0f;
    private float swimingSpeed;

    public SwimingState(Character _character) : base(_character)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Disable root motion to disable gravity
        character.animator.applyRootMotion = false;

        input = Vector2.zero;
        move = Vector3.zero;
        currentVelocity = Vector3.zero;
        playerVelocity = Vector3.zero;
        swimingSpeed = character.playerSpeed/2;

        character.animator.SetTrigger("swim");
    }

    public override void HandleInput()
    {
        input = moveAction.ReadValue<Vector2>();

        move = new Vector3(input.x, 0, input.y);
        move = move.x * character.cameraTransform.right.normalized + move.z * character.cameraTransform.forward.normalized;
        move.y = 0;
    }

    public override void LogicUpdate()
    {
        waterEntryTime += Time.deltaTime;
        if (waterEntryTime >= character.delayBeforeDrowing)
        {
            character.SetState(new DrowingState(character));
        }
        if (character.submergence < 0.5f)
        {
            character.animator.SetTrigger("move");
            character.SetState(new StandingState(character));
        }
    }

    public override void PhysicsUpdate()
    {

        // Buoyancy
        playerVelocity.y += gravityValue * ((1f - character.buoyancy * character.submergence) * Time.deltaTime);


        if (move.sqrMagnitude > 0.03f)
        {
            currentVelocity = Vector3.SmoothDamp(currentVelocity, move, ref cVelocity, character.velocityDampTime);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(move), character.rotationDampTime);
            character.controller.Move(currentVelocity * Time.deltaTime * swimingSpeed + playerVelocity * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.SmoothDamp(currentVelocity, Vector3.zero, ref cVelocity, character.velocityDampTime);
            character.controller.Move(playerVelocity * Time.deltaTime);
        }

    }

    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = true;
    }
}
