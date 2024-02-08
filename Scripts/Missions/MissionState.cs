using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Events;
using Zenject;

public class MissionState : PointerState, IProgressible
{
    [SerializeField] private Transform _target;
    [SerializeField] private string _missionId;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField] private int _reward;
    [SerializeField] private CinemachineVirtualCamera _hintCamera;
    
    public Sprite Icon => _icon;
    public string Description => _description;
    public bool Completed => ES3.Load(_missionId, false);
    public int Reward => _reward;
    public CinemachineVirtualCamera HintCamera => _hintCamera;
    
    public UnityAction<float> ProgressChanged { get; set; }
    protected override Transform Target => _target;

    [Inject] private Balance _balance;
    
    public void Complete()
    {
        if(Completed == false)
            _balance.Earn(Reward);
        ES3.Save(_missionId, true);
        ReceiveDestination();
    }
    
    
    
}
