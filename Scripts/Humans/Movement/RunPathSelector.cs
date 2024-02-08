using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class RunPathSelector : MonoBehaviour
{
    [Inject] private MovementPlane _movementPlane;

    public RunPath SelectPath()
    {
        float minDistance = float.PositiveInfinity;
        RunPath runPath = _movementPlane.RunPaths[0];
        foreach (var runPathItem in _movementPlane.RunPaths)
        {
            float runItemDistance = runPathItem.GetMinDistance(transform);
            if (runItemDistance < minDistance)
            {
                minDistance = runItemDistance;
                runPath = runPathItem;
            }
        }

        return runPath;
    }
}
