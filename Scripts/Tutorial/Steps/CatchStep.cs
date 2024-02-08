using UnityEngine;
using System.Linq;
using Zenject;

public class CatchStep : TutorialStepBase
{
    [Inject] private HumanSpawner _humanSpawner;
    [Inject] private RadarDisabler _radarDisabler;

    protected override Transform Target
    {
        get
        {
            if (_target == null)
                _target = _humanSpawner.GetNearestTarget();
            return _target;
        }
    }

    private Transform _target = null;
    
    
    public override void OnEnter()
    {
        base.OnEnter();
        _radarDisabler.Radar.ChangeNextScanChange(100);
    }
    
}
