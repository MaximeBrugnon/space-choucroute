using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    /**
     * Choucroute Character controller
     */
    public CharacterController controller;

    /**
     * Choucroute's Animator. Avaiable variables are:
     *   - bool isJumping
     *   - float groundSpeed (from 0.0f to 1.0f)
     */
    public Animator animator;

    /**
     * Third person camera
     */
    public Transform cam;

    /**
     * Ground detector's stuffs
     */
    public Transform groundCheck;
    public LayerMask groundMask;
    private const float GROUND_DISTANCE = 0.1f;

    /**
     * Movement factor constants
     *   - Running SPEED
     *   - Earth GRAVITY factor
     *   - JUMP_POWER factor
     */
    private const float SPEED = 15.0f;
    private const float GRAVITY = -9.81f;
    private const float JUMP_POWER = 4.0f;

    /**
     * Choucroute current vectored velocity
     */
    private Vector3 velocity;

    /**
     * Movement bool conditions
     */
    private bool isGrounded;
    private bool isJumping;

    /**
     * Dumping stuffs?
     */
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;

    /**
     * Vector3 used to save the last character movement. 
     * Used while jumping to remember the direction of the jump
     */
    private Vector3 lastMoveFromGround;


    void Start()
    {
        animator = GetComponent<Animator>();
	
    }

    void Update()
    {
        
        
        // Let's move
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (isGrounded) // Full controll on ground
        {
            MoveOnGrand(direction);
        } 

        MoveOnAir();
	
        Animate(direction);
        
    }

    void MoveOnGrand(Vector3 _direction)
    {

   	    if (_direction.magnitude >= 0.1f) // While moving / running
        {
            
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Runing movement
            lastMoveFromGround = moveDir.normalized * SPEED *Time.deltaTime;

        } else {
            // No mouvement.
            lastMoveFromGround = new Vector3(0f, 0f, 0f).normalized * SPEED * Time.deltaTime;
        }

        controller.Move(lastMoveFromGround);

    }

    private void MoveOnAir()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, GROUND_DISTANCE, groundMask);
        
        if (isGrounded)
	    {
            isJumping = false;

            // Don't get this part
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                velocity.y = Mathf.Sqrt(-JUMP_POWER * GRAVITY);
            }
        }

        // Accelerating to the ground
        velocity.y += GRAVITY * Time.deltaTime;

        controller.Move(lastMoveFromGround);
        controller.Move(velocity * Time.deltaTime);
    }

    void Animate(Vector3 _direction)
    {
        animator.SetFloat("groundSpeed", _direction.magnitude);
        animator.SetBool("isJumping", isJumping);
    }
}


