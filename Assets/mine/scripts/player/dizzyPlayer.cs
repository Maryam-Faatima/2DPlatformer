using System.Collections;
using UnityEngine;

public class dizzyPlayer : MonoBehaviour
{
    private Animator animator;
    private playerMovement PlayerMovement;

    [Header("Dizzy Settings")]
    [Tooltip("Minimum vertical drop (startFallY - landingY) to cause dizziness")]
    public float fallHeightThreshold = 3f;
    [Tooltip("Dizzy duration in seconds")]
    public float dizzyDuration = 2f;

    private float startFallY;
    private bool isFalling = false;
    private bool isDizzy = false;
    private bool wasGroundedLastFrame = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        PlayerMovement = GetComponent<playerMovement>();

        // safety null-check
        if (PlayerMovement == null)
            Debug.LogWarning("dizzyPlayer: playerMovement component not found on same GameObject.");
    }

    void FixedUpdate()
    {
        if (isDizzy) return;

        bool grounded = PlayerMovement != null ? PlayerMovement.isGrounded() : false;

        // Transition: grounded -> not grounded => start falling
        if (!grounded && wasGroundedLastFrame)
        {
            isFalling = true;
            startFallY = transform.position.y;
        }

        // Transition: not grounded -> grounded => landed
        if (grounded && !wasGroundedLastFrame && isFalling)
        {
            float landingY = transform.position.y;
            float fallDistance = startFallY - landingY;

            // Debug.Log($"startFallY={startFallY}, landingY={landingY}, fallDistance={fallDistance}");

            if (fallDistance > fallHeightThreshold)
            {
                StartCoroutine(HandleDizzyState());
            }

            isFalling = false;
        }

        wasGroundedLastFrame = grounded;
    }

    private IEnumerator HandleDizzyState()
    {
        isDizzy = true;
        if (PlayerMovement != null) PlayerMovement.canMove = false;
        if (animator != null) animator.SetBool("dizzy", true);

        yield return new WaitForSeconds(dizzyDuration);

        if (animator != null) animator.SetBool("dizzy", false);
        if (PlayerMovement != null) PlayerMovement.canMove = true;
        isDizzy = false;
    }

    // optional: draw a small gizmo to indicate nothing (purely debug if you want)
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(transform.position.x, startFallY, transform.position.z), 0.05f);
    }
}
