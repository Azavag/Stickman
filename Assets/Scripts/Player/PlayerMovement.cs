using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
#endif

public class PlayerMovement : MonoBehaviour
{
    [Header("Характеристики")]
    float movementSpeed;                            // How fast moves forward and back.
    public float m_TurnSpeed;                       // How fast turns in degrees per second.

    [Header("Ссылки")]
    private Rigidbody rb;
    Animator animator;
    [SerializeField] PlayerController playerController;

    [Header("Управление")]
    float horizontalInput, verticalInput;
    Vector3 movementVector;
    [SerializeField] FloatingJoystick joystick;
    bool isJoystickActive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        movementSpeed = playerController.GetMovementSpeed();
    }
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (isJoystickActive)
        {
            horizontalInput = joystick.Horizontal;
            verticalInput = joystick.Vertical;
        }
       
    }
   
    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }
    private void Move()
    {
        movementVector = new Vector3(horizontalInput, 0, verticalInput).normalized / 10f;
        rb.MovePosition(rb.position + movementVector * movementSpeed * Time.fixedDeltaTime);
        animator.SetFloat("speed", movementVector.magnitude);
    }

    private void Turn()
    {
        if (movementVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementVector);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                m_TurnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }
    public void SetActiveJoystick(bool state)
    {
        isJoystickActive = state;
    }
    private void OnDisable()
    {
        animator.SetFloat("speed", 0);
    }
}