using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficLane : MonoBehaviour
{
    [SerializeField] private List<Transform> _rayCastPoints;
    [SerializeField] private string _carLayerMask;
    public float DistanceToNextCar = float.PositiveInfinity;

    public List<Transform> RayCastPoints => _rayCastPoints;

    private void FixedUpdate()
    {
        DistanceToNextCar = GetDistanceToForwardCar(_carLayerMask);
    }

    private float GetDistanceToForwardCar(string layerMask)
    {
        float distance = float.PositiveInfinity;
        foreach (var rayCastPoint in _rayCastPoints)
        {
            float rayCastDistance = CheckRayCast(rayCastPoint, layerMask);
            if (rayCastDistance > 0)
            {
                return rayCastDistance;
            }
        }

        return distance;
    }
    
    private float CheckRayCast(Transform raycastTransform, string layerMask)
    {
        return CheckRayCast(raycastTransform, layerMask, out RaycastHit hit, out bool haveHit);
    }
    
    private float CheckRayCast(Transform raycastTransform, string layerMask, out RaycastHit hit, out bool haveHit)
    {
        hit = default;
        haveHit = false;
        if (Physics.Raycast(raycastTransform.position, raycastTransform.forward, out hit, 50f, LayerMask.GetMask(layerMask)))
        {
            haveHit = true;
            if (hit.collider.transform.SameDirection(transform) == false)
                return 0;
            float distance = Vector3.Distance(hit.point, raycastTransform.position);
            return distance;
        }

        return 0;
    }
}
