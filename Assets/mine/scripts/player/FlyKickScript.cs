using System.Collections;
using UnityEngine;

public class FlyKickScript : MonoBehaviour
{
    private Animator animator;
    private playerMovement PlayerMovement;
    private Rigidbody2D rb;
    private bool isFlyKicking = false;

    [Header("Fly Kick Settings")]
    public float flyKickForce = 8f;
    public float flyKickDuration = 0.7f;

    void Start()
    {
        animator = GetComponent<Animator>();
        PlayerMovement = GetComponent<playerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ✅ Keyboard support
        if (Input.GetKeyDown(KeyCode.LeftShift))
            TryFlyKick();
    }

    // Called from both keyboard and UI button
    public void TryFlyKick()
    {
        if (isFlyKicking || PlayerMovement.isGrounded())
            return;

        StartCoroutine(DoFlyKick());
    }

    private IEnumerator DoFlyKick()
    {
        isFlyKicking = true;
        PlayerMovement.isFlyAttacking = true;
        animator.SetTrigger("flyKick");

        if (rb != null)
        {
            float direction = Mathf.Sign(transform.localScale.x);
            rb.velocity = new Vector2(direction * flyKickForce, rb.velocity.y);
        }

        yield return new WaitForSeconds(flyKickDuration);

        isFlyKicking = false;
        PlayerMovement.isFlyAttacking = false;
    }
}
