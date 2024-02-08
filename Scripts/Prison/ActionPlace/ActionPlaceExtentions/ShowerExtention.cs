using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowerExtention : ParticleExtention
{
    protected override void OnUsePlace(Prisoner prisoner)
    {
        base.OnUsePlace(prisoner);
        prisoner.Human.Variant.RemoveClothing();
    }

    protected override void OnKick(Prisoner prisoner)
    {
        prisoner.Human.Variant.DressClothing();
        base.OnKick(prisoner);
    }
}
