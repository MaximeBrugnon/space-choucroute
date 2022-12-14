using UnityEngine;

public class LandingState : State
{
    float timePassed;
    float landingTime;
    bool isSprinting;

    public LandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        timePassed = 0f;
        character.animator.SetTrigger("land");
        landingTime = 0.3f;
        isSprinting = false;
    }


    public override void HandleInput()
    {
        base.HandleInput();

        if (sprintAction.triggered)
        {
            isSprinting = true;
        }
    }


    public override void LogicUpdate()
    {

        base.LogicUpdate();
        if (timePassed > landingTime)
        {
            character.animator.SetTrigger("move");

            if (isSprinting)
            {
                stateMachine.ChangeState(character.sprinting);
            }
            else
            {
                stateMachine.ChangeState(character.standing);
            }
        }
        timePassed += Time.deltaTime;
    }
}
