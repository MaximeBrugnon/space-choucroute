using UnityEngine;
public class DrowingState : State
{

    public DrowingState(Character _character) : base(_character)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.animator.SetTrigger("drown");

        // Disable root motion to disable gravity
        character.animator.applyRootMotion = false;

        playerVelocity = Vector3.zero;
    }

    public override void HandleInput()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.Enter();
    }

    public override void PhysicsUpdate()
    {
        playerVelocity.y += gravityValue * character.GetWaterDragTimedFactor() * Time.deltaTime;

        if (character.controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        character.controller.Move(playerVelocity * Time.deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
