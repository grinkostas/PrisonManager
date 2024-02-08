using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using HomaGames.HomaBelly;

public class Analytics : MonoBehaviour
{
    private void Awake()
    {
        GameAnalytics.Initialize();
    }

}
