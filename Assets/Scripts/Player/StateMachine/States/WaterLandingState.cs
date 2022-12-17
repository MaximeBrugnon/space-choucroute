using UnityEngine;

public class WaterLandingState:State
{
    float timePassed;
    float landingTime;

    public WaterLandingState(Character _character) : base(_character)
	{
	}

    public override void Enter()
	{
		base.Enter();
        timePassed = 0f;
        character.animator.SetTrigger("land");
        landingTime = 0.5f;
    }

    public override void LogicUpdate()
    {
        
        base.LogicUpdate();
		if (timePassed> landingTime)
		{
            character.animator.SetTrigger("move");
            character.SetState(new StandingState(character));
        }
        timePassed += Time.deltaTime;
    }
}
