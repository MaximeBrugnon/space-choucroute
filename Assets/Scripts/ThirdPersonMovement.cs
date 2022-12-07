using System.Collections;



using System.Security.Cryptography;
using System.Threading;
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
     * Choucroute current vecored velocity
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
        
        isGrounded = Physics.CheckSphere(groundCheck.position, GROUND_DISTANCE, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }
       
        
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        Move(direction);

        
    }

    void Move(Vector3 _direction)
    {
        animator.SetFloat("groundSpeed", _direction.magnitude);
        animator.SetBool("isJumping", !isGrounded);

        if (isGrounded) // Full controll on ground
        {
            if (_direction.magnitude >= 0.1f) // While moving / running
            {
            
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);


                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                // Runing movement
                lastMoveFromGround = moveDir.normalized * SPEED *Time.deltaTime;

                controller.Move(lastMoveFromGround);
            } else 
            {
                // No mouvement.
                lastMoveFromGround = new Vector3(0f, 0f, 0f).normalized * SPEED * Time.deltaTime;
            }


        } else // Limited control while on air
        {            
             controller.Move(lastMoveFromGround);
        }

        if (isJumping)
        {
            // Add jump force
            velocity.y = Mathf.Sqrt(-JUMP_POWER * GRAVITY);
            isJumping = false;

        }

        // Accelerating to the ground
        velocity.y += GRAVITY * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }
}


