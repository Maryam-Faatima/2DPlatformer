using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public soundManagement soundManager;
    public float movementSpeed = 10f;
    private float direction;
    private Animator animator;
    private bool isExploding = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        soundManager = FindObjectOfType<soundManagement>();
    }

    void OnEnable()
    {
        // Reset state whenever the projectile is reused from the pool
        isExploding = false;
        if (animator != null)
            animator.ResetTrigger("explode");
    }

    void Update()
    {
        if (!isExploding)
            transform.Translate(movementSpeed * Time.deltaTime * direction, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploding) return; // prevent multiple explosions

        isExploding = true;
        soundManager.playExplosionSound();
        animator.SetTrigger("explode");

        // 🔹 Damage Enemy
        if (collision.CompareTag("enemy"))
        {
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(1);
        }

        // 🔹 Optional: Handle wall collisions (just explode)
        if (collision.CompareTag("Wall"))
        {
            // no damage, just explode
        }

        // Deactivate projectile after short delay
        StartCoroutine(DeactivateAfterDelay(0.5f));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); // ✅ return projectile to pool
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        float localScaleX = transform.localScale.x;

        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
}
