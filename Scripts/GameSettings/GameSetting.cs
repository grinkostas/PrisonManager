using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSetting : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
