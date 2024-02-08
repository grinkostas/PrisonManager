using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BedVariant : UpgradableModel
{
    [SerializeField] private int _variantLevel;

    public int Level => _variantLevel;
}
