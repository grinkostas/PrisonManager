using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DayPhasePlayerMaterialChange : DayPhaseListner
{
    [SerializeField] private Renderer _skin;
    [SerializeField] private Renderer _dress;
    [SerializeField] private List<Dress> _dresses;
    
    protected override void OnPhaseChanged(DayPhase phase)
    {
        var dress = _dresses.Find(x => x.Phase == phase);
        _skin.material = dress.SkinMaterial;
        _dress.material = dress.DressMaterial;
    }

    [System.Serializable]
    private class Dress
    {
        public DayPhase Phase;
        public Material SkinMaterial;
        public Material DressMaterial;
    }

    
}
