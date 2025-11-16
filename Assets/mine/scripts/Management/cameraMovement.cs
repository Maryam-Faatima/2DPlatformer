using UnityEngine;
using UnityEngine.SceneManagement;

public class cameraMovement : MonoBehaviour
{
    [Header("Camera Settings")]
    public GameObject targetPlayer;
    public float lookAheadDistance = 0f;
    public float cameraSpeed = 0.15f;
    public Vector2 yLimits = new Vector2(-10f, 100f);

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        // Make sure this camera knows itself to GameManager
        if (GameManager.instance != null)
            GameManager.instance.cameraFollow = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically find player after each scene load
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            targetPlayer = playerObj;

        // Reassign GameManager reference
        if (GameManager.instance != null)
            GameManager.instance.cameraFollow = this;
    }


    void FixedUpdate()
    {
        if (targetPlayer == null) return;

        Vector3 targetPosition = new Vector3(
            targetPlayer.transform.position.x + lookAheadDistance,
            Mathf.Clamp(targetPlayer.transform.position.y, yLimits.x, yLimits.y),
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, cameraSpeed);
    }

    // ✅ Instantly reset camera to player's position (for respawn/reset)
    public void ResetCamera()
    {
        if (targetPlayer == null) return;

        transform.position = new Vector3(
            targetPlayer.transform.position.x + lookAheadDistance,
            Mathf.Clamp(targetPlayer.transform.position.y, yLimits.x, yLimits.y),
            transform.position.z
        );
        velocity = Vector3.zero;
    }
}
