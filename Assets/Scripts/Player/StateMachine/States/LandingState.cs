using UnityEngine;

public class LandingState : State
{
    float timePassed;
    float landingTime;
    bool isSprinting;

    public LandingState(Character _character) : base(_character)
    {
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
                character.SetState(new SprintState(character));
            }
            else
            {
                character.SetState(new StandingState(character));
            }
        }
        timePassed += Time.deltaTime;
    }
}
