using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    [Space]

    [SerializeField]
    List<Detector> detectors;

    [SerializeField]
    List<SteeringBehaviour> behaviours;

    [SerializeField]
    ContextSolver solver;

    [SerializeField]
    AIData aiData;


    private void Update()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }


        Vector2 dir = solver.GetDirectionToMove(behaviours, aiData);
        GetComponent<Rigidbody2D>().velocity = (Vector3)dir * speed;
    }
}
