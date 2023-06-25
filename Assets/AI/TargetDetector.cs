using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    float targetDetectionRange = 5f;

    [SerializeField]
    LayerMask obstacleLayer, targetLayer;

    [SerializeField]
    bool showGizmos = true;

    // gizmo
    List<Transform> visibleTargets;

    public override void Detect(AIData data)
    {
        visibleTargets = new List<Transform>();

        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, targetDetectionRange, targetLayer);

        foreach (Collider2D target in targetsInRange)
        {
            Vector2 dirToTarget = (target.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, targetDetectionRange, obstacleLayer);

            //Debug.DrawLine(transform.position, target.transform.position, hit.collider == null ? Color.green : Color.red);

            if (hit.collider != null) continue;

            visibleTargets.Add(target.transform);
        }

        data.targets = visibleTargets;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        if (!Application.isPlaying || visibleTargets == null) return;

        Gizmos.color = Color.magenta;
        foreach (Transform target in visibleTargets)
        {
            Gizmos.DrawSphere(target.position, 0.3f);
        }

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);
    }
}
