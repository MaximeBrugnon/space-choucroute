using UnityEngine;
using UnityEngine.InputSystem;
public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float playerSpeed = 5.0f;
    public float sprintSpeed = 7.0f;
    public float jumpHeight = 0.8f; 
    public float gravityMultiplier = 2;
    public float rotationSpeed = 5f;
 
    [Header("Animation Smoothing")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;
    [Range(0, 1)]
    public float velocityDampTime = 0.9f;
    [Range(0, 1)]
    public float rotationDampTime = 0.2f;
    [Range(0, 1)]
    public float airControl = 0.5f;
 
    public StateMachine stateMachine;
    public StandingState standing;
    public JumpingState jumping;
    public LandingState landing;
    public SprintState sprinting;
    public SprintJumpState sprintjumping;
    public SwimingState swiming;
    public WaterLandingState waterLanding; 
 
    [HideInInspector]
    public float gravityValue = -9.81f;
    [HideInInspector]
    public float normalColliderHeight;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform cameraTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 playerVelocity;
    public bool isInWater = false;
 
 
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
 
        stateMachine = new StateMachine();
        standing = new StandingState(this, stateMachine);
        jumping = new JumpingState(this, stateMachine);
        landing = new LandingState(this, stateMachine);
        sprinting = new SprintState(this, stateMachine);
        sprintjumping = new SprintJumpState(this, stateMachine);
        swiming = new SwimingState(this, stateMachine);
        waterLanding = new WaterLandingState(this, stateMachine);
 
        stateMachine.Initialize(standing);
 
        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;
    }
 
    private void Update()
    {
        stateMachine.currentState.HandleInput();
 
        stateMachine.currentState.LogicUpdate();
    }
 
    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
}
 
