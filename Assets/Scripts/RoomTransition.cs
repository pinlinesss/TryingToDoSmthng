using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("Karakter kapıdan geçince tam olarak ışınlanacağı hedef nokta (Örn: Room2_SpawnPoint)")]
    public Transform playerSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        // Sadece bizim ana karakterimiz çarptığında çalışsın
        if (other.CompareTag("Player") || other.name == "MainGuy_1")
        {
            // Karakteri doğrudan yeni odadaki SpawnPoint'e ışınla
            if (playerSpawnPoint != null)
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero; // Hızını sıfırla
                    rb.position = playerSpawnPoint.position; // Rigidbody'yi ışınla
                }
                else
                {
                    other.transform.position = playerSpawnPoint.position;
                }
            }
        }
    }
}