using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos = true;

    //gizmo parameters
    float[] interestGizmo = new float[8];
    Vector2 resultDirection = Vector2.zero;
    private float rayLength = 2;

    public Vector2 GetDirectionToMove(List<SteeringBehaviour> behaviours, AIData data)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        //Loop through each behaviour
        foreach (SteeringBehaviour behaviour in behaviours)
        {
            behaviour.GetSteering(data, ref interest, ref danger);
        }

        //subtract danger values from interest array
        for (int i = 0; i < 8; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        interestGizmo = interest;

        //get the average direction
        Vector2 outputDirection = Vector2.zero;
        for (int i = 0; i < 8; i++)
        {
            outputDirection += Directions.eightDirections[i] * interest[i];
        }

        outputDirection.Normalize();

        resultDirection = outputDirection;

        return outputDirection;
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying && showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDirection * rayLength);
        }
    }
}