using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BedModel : MonoBehaviour
{
    [SerializeField] private PlaceUpgradeModel _placeUpgradeModel;
    [SerializeField] private Bed _bed;
    [SerializeField] private List<BedVariant> _beds;
    [SerializeField] private bool _right = true;
    
    public BedVariant Variant => _beds.Find(x => x.Level == _bed.Room.Level);

    private void OnEnable()
    {
        Flip();
        Variant.gameObject.SetActive(true);
    }

    private void Flip()
    {
        if (_right == false)
        {
            Vector3 scale = Variant.transform.localScale;
            scale.x *= -1;
            Variant.transform.localScale = scale;
        }
    }
}
