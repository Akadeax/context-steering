using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directions
{
    public static List<Vector2> eightDirections = new()
    {
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized
    };
}

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    [SerializeField]
    float radius = 2f, agentColliderSize = 0.6f;

    [SerializeField]
    bool showGizmo = true;

    //gizmo parameters
    float[] dangersResultTemp = null;

    public override void GetSteering(AIData data, ref float[] interest, ref float[] danger)
    {
        foreach (Collider2D obstacle in data.obstacles)
        {
            Vector2 dirToObstacle = obstacle.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distanceToObstacle = dirToObstacle.magnitude;

            float weight;
            // If we are right next to an obstacle, avoid it at all costs
            if (distanceToObstacle <= agentColliderSize)
            {
                weight = 1f;
            }
            // otherwise, avoid it with weight [0,1] depending on distance from it
            else
            {
                weight = (radius - distanceToObstacle) / radius;
            }

            //Add obstacle parameters to the danger array
            for (int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float result = Vector2.Dot(dirToObstacle.normalized, Directions.eightDirections[i]);

                float valueToPutIn = result * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        dangersResultTemp = danger;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;
        if (!Application.isPlaying || dangersResultTemp == null) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < dangersResultTemp.Length; i++)
        {
            Gizmos.DrawRay(transform.position, dangersResultTemp[i] * Directions.eightDirections[i]);
        }
    }
}

