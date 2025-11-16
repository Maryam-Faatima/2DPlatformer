using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyFireBallHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update()

    {
        if (enemy != null)
        transform.localScale = enemy.localScale;
    }
}
