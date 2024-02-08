using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BuyZoneSaveModel
{
    public bool IsBought;
    public float BuyProgress;

    public BuyZoneSaveModel() : this(false, 0)
    {
        
    }
    
    public BuyZoneSaveModel(bool isBought, float buyProgress)
    {
        IsBought = isBought;
        BuyProgress = buyProgress;
    }
}
