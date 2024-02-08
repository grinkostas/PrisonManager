using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;
using Random = UnityEngine.Random;

public class ShoulderStackDropper : MonoBehaviour
{
    [SerializeField] private Stack _stack;
    [SerializeField] private float _dropRange;
    [SerializeField] private float _dropDelay;
    [SerializeField] private float _runDelay;
    [SerializeField] private float _tempPointDestroyDelay;

    [Inject] private Timer _timer;
    [Inject] private MovementPlane _movementPlane;

    private bool _isDropping = false;
    private Transform _parent;

    private void Start()
    {
        _parent = new GameObject().transform;
    }


    [Button("Drop All")]
    public void DropAll()
    {
        if(_isDropping)
            return;
        StartCoroutine(Dropping());
    }

    private IEnumerator Dropping()
    {
        _isDropping = true;
        var criminals = new List<Stack.CriminalStackData>(_stack.Criminals);
        foreach (var criminalStackData in criminals)
        {
            GameObject tempPoint = new GameObject();
            Vector2 randomCirclePoint = Random.insideUnitCircle * _dropRange;
            Vector3 tempPosition = transform.position + new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y);
            tempPoint.transform.position = tempPosition;
            _stack.TryTakeAndPlace(tempPoint.transform);
            _timer.ExecuteWithDelay(() =>
            {
                var human = criminalStackData.Criminal.Human;
                if (human == null)
                    return;

                var agentHandler = human.AgentHandler;
                if(agentHandler == null)
                    return;
                agentHandler.Agent.enabled = true;
                agentHandler.SetDestination(_movementPlane.ReleasedPoint.position);
                human.transform.SetParent(null);

            }, _runDelay);
            Destroy(tempPoint, _tempPointDestroyDelay);
            yield return new WaitForSeconds(_dropDelay);
        }

        _isDropping = false;
    }
}
