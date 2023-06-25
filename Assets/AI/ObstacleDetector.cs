using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField]
    float detectionRadius = 2f;

    [SerializeField]
    LayerMask obstacleMask;

    [SerializeField]
    bool showGizmos = true;

    // gizmo
    Collider2D[] collidersInRange;

    public override void Detect(AIData data)
    {
        collidersInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, obstacleMask);
        data.obstacles = collidersInRange;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        if (!Application.isPlaying || collidersInRange == null) return;

        Gizmos.color = Color.red;
        foreach (Collider2D obstacle in collidersInRange)
        {
            Gizmos.DrawWireSphere(obstacle.transform.position, 0.2f);
        }

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
