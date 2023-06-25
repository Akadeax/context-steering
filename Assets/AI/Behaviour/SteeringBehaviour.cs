using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    public abstract void GetSteering(AIData data, ref float[] interest, ref float[] danger);
}
