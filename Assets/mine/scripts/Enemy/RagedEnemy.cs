using System.Collections;
using UnityEngine;

public class RagedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float range = 5f;
    [SerializeField] private int attackDamage = 10;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance = 0.5f;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private Animator animator;
    private EnemyPatrol enemyPatrol;
    private bool playerDetectedLastFrame = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        bool playerDetected = PlayerInSight();

        if (playerDetected)
        {
            // Stop patrol movement but keep facing direction frozen
            if (enemyPatrol != null && enemyPatrol.canMove)
                enemyPatrol.canMove = false;

            // Attack if cooldown allows
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                animator.SetTrigger("rangedAttack");
            }
        }
        else
        {
            // Resume patrol when player is not detected
            if (enemyPatrol != null && !enemyPatrol.canMove)
                enemyPatrol.canMove = true;
        }

        playerDetectedLastFrame = playerDetected;
    }

    // 🔥 Called by animation event
    private void RangedAttack()
    {
        int idx = FindFireball();
        fireballs[idx].transform.position = firePoint.position;
        fireballs[idx].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInSight()
    {
        if (boxCollider == null)
            return false;

        // Determine facing direction using localScale.x
        float facingDir = Mathf.Sign(transform.localScale.x);

        Vector2 boxCenter = (Vector2)boxCollider.bounds.center + Vector2.right * facingDir * range * colliderDistance;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y);

        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.left, 0f, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;

        float facingDir = Mathf.Sign(transform.localScale.x);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + Vector3.right * facingDir * range * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }
}
