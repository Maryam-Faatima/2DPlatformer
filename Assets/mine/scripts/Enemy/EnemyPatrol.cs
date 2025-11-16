using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;
    public bool canMove = true;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header ("Enemy Animator")]
    [SerializeField]private Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void moveInDirection(int direction)
    {
        if (canMove && anim != null && enemy != null)
        {
            idleTimer = 0;
            anim.SetBool("moving", true);
            enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
            enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);
        }
    }

    private void OnDisable()
    {
        if (canMove && anim != null)
            anim.SetBool("moving", false);
    }
    private void Update()
    {
        if (canMove && enemy != null)
        {
            if (movingLeft)
            {
                if (enemy.position.x >= leftEdge.position.x)
                {
                    moveInDirection(-1);
                }
                else
                {
                    // movingLeft = false;
                    DirectionChange();
                }

            }
            else
            {
                if (enemy.position.x <= rightEdge.position.x)
                {
                    moveInDirection(1);
                }
                else
                {
                    DirectionChange();
                    // movingLeft = true;
                }

            }
        }
            
        
    }
    private void DirectionChange()
    {
        if (canMove && anim != null)
        {
            anim.SetBool("moving", false);
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
                movingLeft = !movingLeft;
        }
    }
}
