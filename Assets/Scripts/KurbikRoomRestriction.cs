using UnityEngine;

public class KurbikRoomRestriction : MonoBehaviour
{
    [Header("--- TAGS ---")]
    [Tooltip("The tag assigned to room transition triggers that Kurbik cannot cross.")]
    public string roomTransitionTag = "RoomTransition";

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if Kurbik hits a room transition trigger
        if (other.CompareTag(roomTransitionTag))
        {
            StopKurbik();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Extra safety check while inside the boundary
        if (other.CompareTag(roomTransitionTag))
        {
            StopKurbik();
        }
    }

    void StopKurbik()
    {
        if (rb != null)
        {
            // Cancel all forward momentum immediately
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
        
        // Push Kurbik back slightly or freeze input logic if necessary
        // You can fine-tune this with a small knockback if he tries to force through
    }
}