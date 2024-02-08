using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabState : TutorialStepBase
{
    protected override Transform Target => Criminal.LastDetected.transform;
}
