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
        
        // Sahnede "Main Camera" etiketli ana kamerayı otomatik bulur
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        moveInput = Vector3.zero;

        // Klavyeden yön girdilerini alıyoruz
        if (Input.GetKey(upKey))    moveInput.z = 1f;
        if (Input.GetKey(downKey))  moveInput.z = -1f;
        if (Input.GetKey(leftKey))  moveInput.x = -1f;
        if (Input.GetKey(rightKey)) moveInput.x = 1f;
    }

    void FixedUpdate()
    {
        if (cameraTransform == null || moveInput == Vector3.zero) return;

        // Kameranın bakış yönlerini alıyoruz
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Karakterin yukarı-aşağı (Y ekseninde) uçup gitmesini engellemek için Y eksenini sıfırlıyoruz
        camForward.y = 0f;
        camRight.y = 0f;

        // Yön vektörlerini tekrar 1 birim uzunluğa eşitliyoruz (Normalize)
        camForward.Normalize();
        camRight.Normalize();

        // Girdiyi kameranın yön vektörleriyle çarparak kameraya göre yönümüzü belirliyoruz
        Vector3 relativeMovement = (camForward * moveInput.z) + (camRight * moveInput.x);

        // Hareketi uyguluyoruz
        Vector3 targetPosition = rb.position + relativeMovement.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }
}