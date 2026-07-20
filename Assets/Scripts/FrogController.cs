using UnityEngine;

public class FrogController : MonoBehaviour
{
    [Header("--- MOVEMENT SETTINGS ---")]
    [Tooltip("The flat ground movement speed of the frog.")]
    public float moveSpeed = 6f;
    
    [Header("--- JUMP SETTINGS ---")]
    [Tooltip("The key used to perform a jump.")]
    public KeyCode jumpKey = KeyCode.Space;

    [Tooltip("The minimum jump force applied when immediately releasing the jump key.")]
    public float minJumpForce = 8f;

    [Tooltip("The maximum jump force applied when the jump is fully charged.")]
    public float maxJumpForce = 20f;

    [Tooltip("How many seconds it takes to reach maximum jump force while holding the key.")]
    public float maxChargeTime = 1.0f;

    [Header("--- GRAVITY AND GAME FEEL ---")]
    [Tooltip("Multiplier applied to gravity when the frog is falling down.")]
    [Range(1f, 5f)] public float fallMultiplier = 2.5f;

    [Header("--- GROUND DETECTION ---")]
    [Tooltip("The empty transform used to check if the frog is touching the ground.")]
    public Transform groundCheckPoint;

    [Tooltip("The radius of the ground detection sphere.")]
    public float groundCheckRadius = 0.3f;

    [Tooltip("The layer mask representing what is considered ground.")]
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 moveDirection;
    
    private float currentChargeTimer;
    private bool isCharging;

    private Transform mainCameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);

        float horizontal = 0f;
        float vertical = 0f;

        if (!isCharging || !isGrounded)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        if (mainCameraTransform != null)
        {
            Vector3 cameraForward = mainCameraTransform.forward;
            Vector3 cameraRight = mainCameraTransform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        }
        else
        {
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        }

        if (isGrounded)
        {
            if (Input.GetKeyDown(jumpKey))
            {
                isCharging = true;
                currentChargeTimer = 0f;
            }

            if (Input.GetKey(jumpKey) && isCharging)
            {
                currentChargeTimer += Time.deltaTime;
                currentChargeTimer = Mathf.Clamp(currentChargeTimer, 0f, maxChargeTime);
            }

            if (Input.GetKeyUp(jumpKey) && isCharging)
            {
                ExecuteJump();
            }
        }
        else
        {
            if (isCharging)
            {
                isCharging = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isCharging && isGrounded)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
        else
        {
            Vector3 velocity = moveDirection * moveSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void ExecuteJump()
    {
        float chargePercent = currentChargeTimer / maxChargeTime;
        float finalJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargePercent);

        Vector3 jumpVelocity = moveDirection * moveSpeed; 
        jumpVelocity.y = finalJumpForce;

        rb.velocity = jumpVelocity;
        isCharging = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}