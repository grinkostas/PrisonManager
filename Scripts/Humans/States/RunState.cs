using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Events;
using Zenject;

public class RunState : State
{
    [SerializeField] private RunPathFollower _runPathFollower;
    [SerializeField] private Scannable _runnerScannable;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _timeToDeactivate;
    [Header("Particle")] 
    [SerializeField] private ParticleSystem _particleSystem;
    

    [Inject] private Player _player;
    [Inject] private Stack _stack;
    
    private NavMeshAgentProvider Agent => _runnerScannable.Human.AgentHandler.Agent;
    private Outline Outline => _runnerScannable.Human.Variant.Outline;

    private float _startSpeed;
    private float _timer;
    
    private Coroutine _timerCoroutine;
    private Coroutine _speedChangeCoroutine;

    public UnityAction Escaped;
    public static UnityAction<RunState> StartRun;
    public static UnityAction<RunState> EndRun;

    public float RunTime => _timeToDeactivate;
    
    private void Awake()
    {
        _startSpeed = Agent.speed;
    }

    public override void OnEnter()
    {
        _particleSystem.Play();
        StartRun?.Invoke(this);
        _runnerScannable.Human.DressRobe();
        StartCoroutine(SpeedChanging());
        Outline.enabled = true;
        _runnerScannable.gameObject.SetActive(true);
        _timer = 0;
        _timerCoroutine = StartCoroutine(Timer());
        _runPathFollower.Run();
        _runnerScannable.ScanProgressChanged += OnScanProgressChanged;
        _player.Pointer.SetDestination(transform);
        _stack.CountChanged += OnStackCountChanged;
    }
    private void OnScanProgressChanged(float progress)
    {
        _timer = 0;
    }

    private IEnumerator Timer()
    {
        while (_timer < _timeToDeactivate)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        EndRun?.Invoke(this);
        Escaped?.Invoke();
    }

    private void OnStackCountChanged(int count)
    {
        if(_stack.Full)
            _player.Pointer.ReceiveDestination(transform);
    }
    
    public override void OnExit()
    {
        _player.Pointer.ReceiveDestination(transform);
        Outline.enabled = false;
        _runnerScannable.gameObject.SetActive(false);
        Agent.speed = _startSpeed;
        EndRun?.Invoke(this);
        
        _runPathFollower.StopRun();
        if(_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);
        _timerCoroutine = null;
        _runnerScannable.ScanProgressChanged -= OnScanProgressChanged;
        _stack.CountChanged -= OnStackCountChanged;
    }

    private IEnumerator SpeedChanging()
    {
        while (enabled)
        {
            if (Agent.speed < _runSpeed)
                Agent.speed = _runSpeed;
            yield return null;
        }
    }
}
