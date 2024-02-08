using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LastTutorialStep : TutorialStepBase
{
    [SerializeField] private TutorialStateMachine _tutorial;

    protected override Transform Target => transform;

    public override void OnEnter()
    {
        _tutorial.EndTutorial();
    }
}
