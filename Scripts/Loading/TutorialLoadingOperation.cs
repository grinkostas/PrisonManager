using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Loading;

public class TutorialLoadingOperation : LoadingOperation
{
    [SerializeField] private string _tutorialId;
    
    public override void Load()
    {
        if (ES3.Load(_tutorialId, false) == false)
        {
            ES3.DeleteFile();
        }
        Finish();
    }
}
