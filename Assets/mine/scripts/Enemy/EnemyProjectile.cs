using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 5f;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;
    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0f;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (hit) return;

        transform.Translate(speed * Time.deltaTime, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (hit) return;

        // Check if it hit the Player
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }

        // Mark hit AFTER damage
        hit = true;
        coll.enabled = false;

        // Explosion animation
        if (anim != null)
        {
            anim.SetTrigger("explode");
            Invoke(nameof(Deactivate), 0.3f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Deactivate()
    {
        hit = false;            // Reset hit state for reuse
        gameObject.SetActive(false);
    }
}
