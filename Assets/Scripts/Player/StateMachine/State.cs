using UnityEngine;
using UnityEngine.InputSystem;

public abstract class State
{
    protected Character character;

    protected Vector3 playerVelocity;
    protected Vector3 move;
    protected Vector2 input;
    protected float gravityValue;
    protected float playerSpeed;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction sprintAction;

    public State(Character _character)
    {
        character = _character;

        moveAction = character.playerInput.actions["Move"];
        lookAction = character.playerInput.actions["Look"];
        jumpAction = character.playerInput.actions["Jump"];
        sprintAction = character.playerInput.actions["Sprint"];

        playerSpeed = character.playerSpeed;
        gravityValue = character.gravityValue;
        playerVelocity = character.playerVelocity;
    }

    public virtual void Enter()
    {
        // Debug.Log("Enter State: " + this.ToString());
    }

    public virtual void HandleInput()
    {
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Exit()
    {
        // Debug.Log("Leaving State: " + this.ToString());
    }
}