using UnityEngine;
using Unity.Cinemachine; // Eğer Cinemachine v3 (Unity 6) kullanıyorsan bu namespace gereklidir

public class CharacterSwitcher : MonoBehaviour
{
    [Header("--- CHARACTERS ---")]
    public GameObject mainGuy;
    public GameObject kurbik;

    [Header("--- SCRIPTS ---")]
    [Tooltip("MainGuy's movement script component.")]
    public MonoBehaviour mainGuyMovementScript;
    [Tooltip("Kurbik's FrogController script component.")]
    public FrogController kurbikMovementScript;

    [Header("--- CAMERA ---")]
    [Tooltip("The Virtual Camera that follows the active character.")]
    public CinemachineCamera virtualCamera; // Eski sürümlerde CinemachineVirtualCamera kullanmalısın

    [Header("--- SWITCH SETTINGS ---")]
    [Tooltip("The key used to switch between characters.")]
    public KeyCode switchKey = KeyCode.E;
    [Tooltip("Maximum distance allowed to switch to Kurbik.")]
    public float switchRadius = 3f;

    private bool isControllingKurbik = false;

    void Start()
    {
        // Start by controlling MainGuy
        SetControl(true);
    }

    void Update()
    {
        if (!isControllingKurbik)
        {
            // MainGuy controls Kurbik by approaching him
            float distance = Vector3.Distance(mainGuy.transform.position, kurbik.transform.position);
            
            if (distance <= switchRadius && Input.GetKeyDown(switchKey))
            {
                SetControl(false); // Switch to Kurbik
            }
        }
        else
        {
            // Kurbik returns control to MainGuy anytime from anywhere
            if (Input.GetKeyDown(switchKey))
            {
                SetControl(true); // Switch to MainGuy
            }
        }
    }

    void SetControl(bool controlMainGuy)
    {
        isControllingKurbik = !controlMainGuy;

        // Toggle movement scripts
        if (mainGuyMovementScript != null) mainGuyMovementScript.enabled = controlMainGuy;
        if (kurbikMovementScript != null) kurbikMovementScript.enabled = !controlMainGuy;

        // Reset Rigidbody velocities when losing control so they don't slide away
        Rigidbody rbMain = mainGuy.GetComponent<Rigidbody>();
        Rigidbody rbKurbik = kurbik.GetComponent<Rigidbody>();
        if (rbMain != null) rbMain.linearVelocity = Vector3.zero;
        if (rbKurbik != null) rbKurbik.linearVelocity = Vector3.zero;

        // Switch Camera Focus
        if (virtualCamera != null)
        {
            Transform targetTransform = controlMainGuy ? mainGuy.transform : kurbik.transform;
            virtualCamera.Follow = targetTransform;
            virtualCamera.LookAt = targetTransform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (kurbik != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(kurbik.transform.position, switchRadius);
        }
    }
}