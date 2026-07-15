using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input Settings")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Find the main camera in the scene automatically
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        moveInput = Vector3.zero;

        // Clean input check using Unity's official KeyCode system
        if (Input.GetKey(upKey))    moveInput.z = 1f;
        if (Input.GetKey(downKey))  moveInput.z = -1f;
        if (Input.GetKey(leftKey))  moveInput.x = -1f;
        if (Input.GetKey(rightKey)) moveInput.x = 1f;
    }

    void FixedUpdate()
    {
        if (cameraTransform == null) return;

        // Get camera forward and right vectors on the horizontal plane
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Calculate movement relative to the camera's point of view
        Vector3 relativeMovement = (camForward * moveInput.z) + (camRight * moveInput.x);

        // Apply physics movement smoothly
        rb.MovePosition(rb.position + relativeMovement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}