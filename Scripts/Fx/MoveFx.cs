using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MoveFx : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _slideCurve;
    [SerializeField] private AnimationCurve _yCurve;
    [SerializeField] private AnimationCurve _yDecreaseCurve;
    [SerializeField] private float _yOffset;
    [SerializeField] private bool _rotate;
    public float Duration => _duration;
    public UnityAction<Transform> OnEnd;

    private List<MoveData> _movingTransforms = new List<MoveData>();



    public void Move(Transform target, Transform destinationPoint, Vector3 destinationDelta= new Vector3(), bool changeParent = false, float progress = 0.0f, bool localMove = false)
    {
        var coroutine = StartCoroutine(MoveRoutine(target, destinationPoint, destinationDelta, changeParent, progress, localMove));
        _movingTransforms.Add(new MoveData(coroutine, target));
    }
    
    public IEnumerator MoveRoutine(Transform target, Transform destinationPoint, Vector3 destinationDelta = new Vector3(), bool changeParent = false, float yProgress = 0.0f,  bool localMove = false)
    {
        float wastedTime = 0.0f;
        Vector3 startPosition = target.position;
        Quaternion startRotation = target.rotation;
        
        while (_duration >= wastedTime)
        {
            yield return null;
            wastedTime += Time.deltaTime;
            float progress = Mathf.Clamp(wastedTime / _duration, 0f, 1f);
            float slideEvaluate = Mathf.Clamp(_slideCurve.Evaluate(progress), 0f, 1f);
            
            Vector3 destination = destinationDelta + destinationPoint.position;
            Vector3 slideFrameDelta = Vector3.Lerp(startPosition, destination, slideEvaluate);
            
            Quaternion frameRotation = Quaternion.Lerp(startRotation, destinationPoint.rotation, slideEvaluate);
            
            Vector3 yFrameOffset = new Vector3(0, _yCurve.Evaluate(progress) * (_yDecreaseCurve.Evaluate(yProgress) * _yOffset), 0);
            
            if (target != null)
            {
                var tempPoint = yFrameOffset + slideFrameDelta;
                if (float.IsNaN(tempPoint.y)) tempPoint.y = 0.0f;
                target.position = tempPoint;
                if(_rotate)
                    target.rotation = frameRotation;
            }
        }

        if (changeParent)
        {
            target.SetParent(destinationPoint);
            if (localMove)
            {
                target.localPosition = destinationDelta;
                if(_rotate)
                    target.localRotation = Quaternion.identity;
            }
        }

        EndMove(target);
    }

    public void EndMove(Transform target)
    {
        var dataToRemove = _movingTransforms.Find(x => x.Target == target);
        if (dataToRemove != null)
        {
            StopCoroutine(dataToRemove.MoveCoroutine);
            _movingTransforms.Remove(dataToRemove);
        }
        OnEnd?.Invoke(target);
    } 

    private class MoveData
    {
        public Coroutine MoveCoroutine { get; private set; }
        public Transform Target { get; private set; }

        public MoveData(Coroutine coroutine, Transform target)
        {
            MoveCoroutine = coroutine;
            Target = target;
        }
    }
}
