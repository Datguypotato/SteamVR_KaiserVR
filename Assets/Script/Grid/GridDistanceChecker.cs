using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDistanceChecker : MonoBehaviour
{
    public Transform[] childrenTransform;

    private void Awake()
    {
        childrenTransform = new Transform[transform.childCount];
        for (int i = 0; i < childrenTransform.Length; i++)
        {
            //childrenTransform[i] = transform.GetChild(i);
        }
    }

    public Transform GetClosestTarget(Vector3 hitPpoint)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = hitPpoint;
        foreach (Transform potentialTarget in childrenTransform)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
}
