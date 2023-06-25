using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    float targetReachedThreshold = 0.5f;

    [SerializeField]
    bool showGizmo = true;

    Vector2 lastSeenTargetPosition;

    // gizmo parameters
    float[] cachedInterest;
    AIData cachedData;

    public override void GetSteering(AIData data, ref float[] interest, ref float[] danger)
    {
        if (data.currentTarget == null)
        {
            SelectTarget(data);
        }

        if (data.targets.Contains(data.currentTarget))
        {
            lastSeenTargetPosition = data.currentTarget.position;
        }


        bool hasReachedCurrentTarget = Vector2.Distance(transform.position, lastSeenTargetPosition) < targetReachedThreshold;

        if (hasReachedCurrentTarget)
        {
            data.currentTarget = null;
        }
        else
        {
            PursueTarget(data, ref interest);
        }

    }

    void SelectTarget(AIData data)
    {
        // if no targets left, leave current target at null
        if (data.targets == null || data.targets.Count == 0) return;

        data.currentTarget = data.targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).First();
    }

    void PursueTarget(AIData data, ref float[] interest)
    {
        //If we havent yet reached the target do the main logic of finding the interest directions
        Vector2 directionToTarget = lastSeenTargetPosition - (Vector2)transform.position;
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);
            result = Mathf.Clamp01(result);
            interest[i] = result;
        }

        cachedInterest = interest;
        cachedData = data;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;
        if (!Application.isPlaying || cachedInterest == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(lastSeenTargetPosition, 0.2f);

        Gizmos.color = Color.green;
        for (int i = 0; i < cachedInterest.Length; i++)
        {
            Gizmos.DrawRay(transform.position, cachedInterest[i] * Directions.eightDirections[i]);
        }
    }
}
