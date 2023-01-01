using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Character : StateMachine 
{
    [Header("Controls")]
    public float playerSpeed = 5.0f;
    public float sprintSpeed = 7.0f;
    public float jumpHeight = 0.8f; 
    public float gravityMultiplier = 2;
    public float rotationSpeed = 5f;

    [Header("Swiming Behavior")]
    [SerializeField]
    private LayerMask waterMask;
    [Range(0f, 100f)]
    public float waterDrag = 40f;
    public float submergence;
    private float submergenceOffset;
    private float submergenceRange;
    [Range(0f, 2f)]
    public float buoyancy = 1f;
    public bool IsInWater => submergence > 0f;
    public bool IsSwiming => submergence >= 0.7f;
    public float delayBeforeDrowing = 15f;

    [Header("Animation Smoothing")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;
    [Range(0, 1)]
    public float velocityDampTime = 0.9f;
    [Range(0, 1)]
    public float rotationDampTime = 0.2f;
    [Range(0, 1)]
    public float airControl = 0.5f;

    [HideInInspector]
    public float gravityValue;
    [HideInInspector]
    public Vector3 upAxis;
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


    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        gravityValue = Physics.gravity.y * gravityMultiplier;
        upAxis = -Physics.gravity.normalized;

        // Water collision and submergence
        submergenceRange = controller.height;
        submergenceOffset = submergenceRange / 2;

        SetState(new StandingState(this));
    }
 
    private void Update()
    {
        state.HandleInput();
        state.LogicUpdate();
    }
 
    private void FixedUpdate()
    {
        state.PhysicsUpdate();

        ClearState();
    }

    void OnTriggerEnter(Collider other)
    {
        if ((waterMask & (1 << other.gameObject.layer)) != 0)
        {
            EvaluateSubmergence();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if ((waterMask & (1 << other.gameObject.layer)) != 0)
        {
            EvaluateSubmergence();
        }
    }

    void EvaluateSubmergence()
    {
        // Measuring percent of submergence by casting a ray from the top of the
        // collider to the surface of the water
        if (Physics.Raycast(
            (transform.position + controller.center) + upAxis * submergenceOffset,
            -upAxis, out RaycastHit hit, submergenceRange,
            waterMask, QueryTriggerInteraction.Collide
        ))
        {
            submergence = 1f - hit.distance / submergenceRange;
        }
        else // If no target hit: we are underwater
        {
            submergence = 1f;
        }

    }

    public float GetWaterDragTimedFactor() { 
        return 1f - waterDrag * submergence * Time.deltaTime;
    }

    private void ClearState()
    {
        submergence = 0f;
    }
}
