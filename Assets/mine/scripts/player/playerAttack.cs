using System.Collections;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public soundManagement soundManager;
    private playerMovement PlayerMovement;
    private Animator animator;

    [Header("Attack Settings")]
    public float coolDownTime = 0.2f;
    private float coolDownTimer = 0f;

    [Header("Fireball Settings")]
    public GameObject fireBallPrefab;
    public GameObject firePoint;

    void Start()
    {
        PlayerMovement = GetComponent<playerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        coolDownTimer += Time.deltaTime;

#if UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.F) && CanAttack())
        {
            PerformAttack();
        }
#endif
    }

    // =====================================================
    //                 Attack Logic (Shared)
    // =====================================================
    public void PerformAttack()
    {
        if (!CanAttack()) return;

        coolDownTimer = 0f;
        animator.SetTrigger("attack");

        // Get fireball from object pool
        GameObject fireball = FireBallPool.Instance.GetFireball();
        fireball.transform.position = firePoint.transform.position;
        fireball.transform.rotation = Quaternion.identity;
        fireball.SetActive(true);

        fireball.GetComponent<Projectile>().SetDirection(transform.localScale.x);

        // Play sound
        soundManager.playFireSound();
    }

    // =====================================================
    //                Attack Availability Check
    // =====================================================
    private bool CanAttack()
    {
        // Only attack if cooldown complete & player not moving horizontally
        return coolDownTimer > coolDownTime && PlayerMovement.canAttack();
    }

    // =====================================================
    //            For Mobile Attack Button (UI)
    // =====================================================
    public void OnAttackButtonPressed()
    {
        PerformAttack();
    }
}
