using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK;
using StaserSDK.Utilities;
using UnityEngine.AI;
using Zenject;

public class CriminalTransformationVisualization : Visualization
{
    [SerializeField] private Human _human;
    [SerializeField] private Collider _criminalCollider;
    [Header("Fried")] 
    [SerializeField] private MaterialFade _materialFade;
    [SerializeField] private Material _friedMaterial;
    [SerializeField] private float _friedTime;
    [SerializeField] private float _friedTransition;
    [Header("Run")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _runRation;
    [SerializeField] private float _runDistance;
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [SerializeField, AnimatorParam(nameof(_animator))]
    private string _detectAnimation;
    [SerializeField, AnimatorParam(nameof(_animator))]
    private string _idleAnimation;

    [SerializeField, AnimatorParam(nameof(_animator))]
    private string _speedRatio;
    
    [SerializeField] private float _animationDelay;
    [Header("Particle")]
    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private Transform _particlePoint;
    [SerializeField] private float _particleSpawnDelay;

    [Inject] private Timer _timer;
    [Inject] private Player _player;

    private float _startSpeed;


    public override void Visualize()
    {
        _criminalCollider.enabled = false;
        Run();
        _timer.ExecuteWithDelay(Detect, _animationDelay);
        if(_particlePrefab != null)
            _timer.ExecuteWithDelay(()=>Instantiate(_particlePrefab, _particlePoint), _particleSpawnDelay);
    }

    private void Run()
    {
        _startSpeed = _human.AgentHandler.Agent.speed;
        _human.AgentHandler.Agent.speed = _runSpeed;
        _human.Animator.SetFloat(_speedRatio, _runRation);

        
        Vector3 runVector = _human.transform.position + _player.Body.forward * _runDistance;
        
        if (NavMesh.SamplePosition(runVector, out NavMeshHit hit, 2*_runDistance, NavMesh.AllAreas))
        {
            runVector = hit.position;
        }

        _human.AgentHandler.Agent.enabled = true;
        _human.AgentHandler.SetDestination(runVector);
    }

    private void Fry()
    {
        _materialFade.Fade(_human.Renderer, _friedMaterial, _friedTransition, true, _friedTime);
        _materialFade.Fade(_human.Variant.Dress, _friedMaterial, _friedTransition, true, _friedTime);

    }
    
    private void Detect()
    {
        Fry();
        _human.AgentHandler.Agent.speed = _startSpeed;
        _human.Animator.SetFloat(_speedRatio, 0.0f);
        _human.Animator.SetTrigger(_detectAnimation);
        _human.AgentHandler.Agent.enabled = false;
        _criminalCollider.enabled = true;
    }
}
