using UnityEngine;

public class CrouchScript : MonoBehaviour
{
    private playerMovement playerMovement;
    private Animator animator;
    private bool isCrouching = false;

    void Start()
    {
        playerMovement = GetComponent<playerMovement>();
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogWarning("CrouchScript: Animator missing on " + gameObject.name);
        if (playerMovement == null) Debug.LogWarning("CrouchScript: playerMovement missing on " + gameObject.name);
    }

    void Update()
    {
        // Toggle crouch on key press (single press toggles)
        if (Input.GetKeyDown(KeyCode.C))
        {
            TryToggleCrouch();
        }
    }

    // PUBLIC - call from UIManager button OnClick
    public void TryToggleCrouch()
    {
        // If currently not crouching, only allow starting crouch when grounded
        if (!isCrouching)
        {
            if (playerMovement != null && playerMovement.isGrounded())
                StartCrouch();
            // else: not grounded => ignore attempt to crouch
        }
        else
        {
            // If already crouching, always allow stop (so player can stand up)
            StopCrouch();
        }
    }

    private void StartCrouch()
    {
        isCrouching = true;
        if (animator != null) animator.SetBool("crouch", true);
        // Optionally change collider size or movement speed here
        // e.g. playerMovement.movementSpeed *= 0.5f;
    }

    private void StopCrouch()
    {
        isCrouching = false;
        if (animator != null) animator.SetBool("crouch", false);
        // Reset movement speed or collider here if changed earlier
        // e.g. playerMovement.movementSpeed = playerMovement.normalSpeed;
    }

    // Optional helper for other scripts to query
    public bool IsCrouching()
    {
        return isCrouching;
    }
}
