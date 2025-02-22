using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;

    [Header("Zoom Settings")]
    [Tooltip("Minimum camera height (lowest terrain + 10)")]
    public float minCameraHeight = 10f;
    [Tooltip("Maximum camera height")]
    public float maxCameraHeight = 80f;
    public float zoomSpeed = 10f;

    [Header("Rotation Settings")]
    [Tooltip("Rotation speed for yaw (left/right rotation)")]
    public float yawSpeed = 90f;
    [Tooltip("Rotation speed for pitch (up/down rotation)")]
    public float pitchSpeed = 90f;

    // Internal rotation state.
    private float yaw = 25f;         // Initial yaw is 25°
    private float pitch = 29.5f;     // Initial pitch is 29.5°

    // Clamp pitch: camera cannot rotate upward past 16° and cannot rotate downward past 90°.
    private float minPitch = 16f;
    private float maxPitch = 90f;

    void Start()
    {
        // Set the initial camera position (x and z remain, y set to 24).
        Vector3 pos = transform.position;
        pos.y = 24f;
        transform.position = pos;

        // Apply the initial rotation.
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Instead of using world axes, compute camera-relative forward and right vectors,
        // but ignore any vertical component.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 movement = (forward * v + right * h) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void HandleZoom()
    {
        // Zoom using the mouse scroll wheel.
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (Mathf.Abs(scroll) > 0.001f)
        {
            Vector3 pos = transform.position;
            pos.y -= scroll;  // Subtract so that a positive scroll zooms in.
            pos.y = Mathf.Clamp(pos.y, minCameraHeight, maxCameraHeight);
            transform.position = pos;
        }
    }

    private void HandleRotation()
    {
        // Yaw: rotate left/right using Q and E keys.
        if (Input.GetKey(KeyCode.Q))
            yaw -= yawSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            yaw += yawSpeed * Time.deltaTime;

        // Pitch: use Z to rotate downward (increase pitch) and C to rotate upward (decrease pitch).
        if (Input.GetKey(KeyCode.Z))
            pitch += pitchSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.C))
            pitch -= pitchSpeed * Time.deltaTime;

        // Clamp the pitch between 16° and 90°.
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Apply the rotation.
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
