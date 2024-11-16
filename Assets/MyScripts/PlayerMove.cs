using UnityEngine;
using Cinemachine;

public class PlayerMove : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent nav;
    private Animator anim;
    private Ray ray;
    private RaycastHit hit;

    private float velocitySpeed;

    CinemachineTransposer ct;
    public CinemachineVirtualCamera playerCam;

    public Transform player; // Reference to the player's transform
    private Vector3 offset; // Offset from the player to the camera

    public float rotationSpeed = 5.0f; // Speed of camera rotation
    public float zoomSpeed = 2.0f; // Speed of zoom
    public float minZoom = 5.0f; // Minimum zoom distance
    public float maxZoom = 15.0f; // Maximum zoom distance

    void Start()
    {
        // Initialize components
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Validate Cinemachine component
        if (playerCam == null)
        {
            Debug.LogError("PlayerCam is not assigned. Please assign it in the Inspector.");
            return;
        }

        ct = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        offset = ct.m_FollowOffset; // Initialize the camera's offset
    }

    void Update()
    {
        // Calculate velocity speed
        velocitySpeed = nav.velocity.magnitude;

        // Handle player movement with mouse clicks
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                nav.SetDestination(hit.point);
            }
        }

        // Update animation based on movement
        anim.SetBool("sprinting", velocitySpeed > 0.1f);

        // Handle camera rotation with middle mouse button (scroll wheel click)
        if (Input.GetMouseButton(2))
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed; // Horizontal rotation
            Quaternion rotation = Quaternion.Euler(0, horizontal, 0); // Create rotation
            offset = rotation * offset; // Rotate offset around the player
        }

        // Handle zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float newZoom = Mathf.Clamp(offset.magnitude - scroll * zoomSpeed, minZoom, maxZoom);
            offset = offset.normalized * newZoom; // Adjust zoom while maintaining direction
        }

        // Apply the camera offset and ensure it follows the player
        ct.m_FollowOffset = offset;
    }
}
