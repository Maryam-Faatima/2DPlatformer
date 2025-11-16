using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public GameManager gameManager;
    public UIManager uiManager;
    public EnemyPatrol enemyPatrol;

    [Header("Health")]
    [SerializeField] private float startingHealth = 4f;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 0.5f;
    [SerializeField] private int numberOfFlashes = 3;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable || dead) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            if (anim != null)
                anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                dead = true;
                if (anim != null)
                    anim.SetTrigger("death");

                if (CompareTag("Player"))
                {
                    playerMovement playerMove = GetComponent<playerMovement>();
                    if (playerMove != null)
                    {
                        playerMove.Death();
                        return; // Stop further death handling from Health.cs
                    }
                }


                // Disable all behaviors (movement, AI, etc.)
                if (components != null)
                {
                    foreach (Behaviour component in components)
                    {
                        if (component != null)
                            component.enabled = false;
                    }
                }

                // If this is an enemy, stop movement and destroy after animation
                if (CompareTag("enemy"))
                {
                    if (enemyPatrol != null)
                        enemyPatrol.canMove = false;

                    // Wait for death animation before removing
                    StartCoroutine(DestroyAfterDeath());
                }
            }
        }
    }

    private IEnumerator DestroyAfterDeath()
    {
        // Wait for animation to finish (or fallback to 1s)
        float deathAnimLength = 1f;
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name.ToLower().Contains("death"))
                {
                    deathAnimLength = clip.length;
                    break;
                }
            }
        }

        yield return new WaitForSeconds(deathAnimLength + 0.1f);
        Destroy(gameObject);
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    public void setHealth(int HealthAmount)
    {
        currentHealth = HealthAmount;
    }

    public void Revive()
    {
        currentHealth = startingHealth;
        dead = false;

        foreach (Behaviour component in components)
            if (component != null)
                component.enabled = true;

        if (anim != null)
        {
            anim.ResetTrigger("death");
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

}
