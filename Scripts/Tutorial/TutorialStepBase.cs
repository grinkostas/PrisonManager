using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HomaGames.HomaBelly;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;

public abstract class TutorialStepBase : PointerState
{
    [SerializeField] private string _analyticsId;

    public override void OnEnter()
    {
        base.OnEnter();
        DefaultAnalytics.LevelStarted(_analyticsId);
    }

}
