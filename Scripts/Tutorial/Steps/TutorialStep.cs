using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialStep : TutorialStepBase
{
    [SerializeField] private Transform _target;
    protected override Transform Target => _target;
}
