

using System.Collections;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("References")]
    public soundManagement soundManager;
    // REMOVED: public GameManager gameManager; - Use singleton instead
    // REMOVED: public UIManager uiManager; - Use singleton instead
    public Health playerHealth;

    [Header("Movement Settings")]
    public Rigidbody2D rb;
    public float movementSpeed = 5f;
    public float jumpSpeed = 8f;
    public BoxCollider2D boxCollider;
    public LayerMask groundLayer;
    public Animator Animator;
    public bool canMove = true;
    public GameObject respawnPoint;

    [Header("Wall Jump Settings")]
    public float wallJumpForce = 8f;
    public float wallCheckDistance = 0.4f;
    public LayerMask wallLayer;

    [Header("Mobile Controls")]
    public FixedJoystick joystick;

    private bool isTouchingWall = false;
    private bool isWallJumping = false;
    private float horizontalInput;

    [HideInInspector] public bool isFlyAttacking = false;

    private void Awake()
    {
        if (GameManager.instance != null)
            GameManager.instance.player = this;
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Find respawn point if not set
        if (respawnPoint == null)
        {
            GameObject spawn = GameObject.FindGameObjectWithTag("Respawn");
            if (spawn != null)
                respawnPoint = spawn;
        }
    }

    void Update()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            Animator.SetBool("walk", false);
            return;
        }

        // ------------- Movement Input -------------
#if UNITY_STANDALONE || UNITY_WEBGL
        horizontalInput = Input.GetAxis("Horizontal");
#elif UNITY_ANDROID || UNITY_IOS
        horizontalInput = joystick.Horizontal;
#endif

        rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);

        // ------------- Jump Input -------------
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // ------------- Flip Character -------------
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        transform.rotation = Quaternion.identity;

        // ------------- Animations -------------
        Animator.SetBool("walk", horizontalInput != 0);
        if (!isFlyAttacking)
            Animator.SetBool("jump", !isGrounded());

        CheckWall();
    }

    // =====================================================
    //                    Jump Logic
    // =====================================================
    public void Jump()
    {
        // Wall Jump
        if (isTouchingWall && !isGrounded())
        {
            isWallJumping = true;
            float jumpDirection = -Mathf.Sign(transform.localScale.x);
            rb.velocity = new Vector2(jumpDirection * wallJumpForce, jumpSpeed);
            StartCoroutine(ResetWallJump());
        }
        // Ground Jump
        else if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        if (soundManager != null)
            soundManager.playJumpSound();

        Animator.SetTrigger("jump");
    }

    private IEnumerator ResetWallJump()
    {
        yield return new WaitForSeconds(0.2f);
        isWallJumping = false;
    }

    private void CheckWall()
    {
        Vector2 direction = new Vector2(transform.localScale.x, 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);
        isTouchingWall = hit.collider != null;
    }

    // =====================================================
    //                    Ground Check
    // =====================================================
    public bool isGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            extraHeight,
            groundLayer
        );

        return hit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0;
    }

    // =====================================================
    //                    Death & Respawn
    // =====================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
            Death();
    }

    public void Death()
    {
        Animator.SetTrigger("death");
        rb.isKinematic = true;
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Use singleton
        if (GameManager.instance != null)
        {
            GameManager.instance.LoseLife();
        }

        if (GameManager.instance != null && GameManager.instance.lives > 0)
            StartCoroutine(Respawn(0.5f));
        else
            StartCoroutine(GameOverSequence());
    }

    public IEnumerator Respawn(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);

        if (respawnPoint != null)
            transform.position = respawnPoint.transform.position;

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = Vector3.zero;

        GetComponent<SpriteRenderer>().enabled = true;
        Animator.SetTrigger("reset");
        canMove = true;

        if (playerHealth != null)
            playerHealth.Revive();
    }

    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(0.5f);

        if (GameManager.instance != null && GameManager.instance.manager != null)
            GameManager.instance.manager.ShowGameOver();
    }

    // =====================================================
    //               Level Reset / New Level
    // =====================================================
    public void ResetForNewLevel()
    {
        if (respawnPoint != null)
            transform.position = respawnPoint.transform.position;

        rb.velocity = Vector2.zero;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Animator.Rebind();
        Animator.Update(0f);

        canMove = true;
        isFlyAttacking = false;

        if (playerHealth != null)
            playerHealth.Revive();
    }
}