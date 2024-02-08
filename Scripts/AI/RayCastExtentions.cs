using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class RayCastExtentions
{
    public static bool MultipleRaycast(List<Transform> raycastPoints, float lenght, LayerMask layerMask, out RaycastHit hit)
    {
        hit = new RaycastHit();
        foreach (var rayCastPoint in raycastPoints)
        {
            if (Physics.Raycast(rayCastPoint.position, rayCastPoint.forward, out hit, lenght, layerMask))
            {
                return true;
            }
        }

        return false;
    }
}
