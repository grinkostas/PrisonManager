using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackSound : StackCountChangeListner
{
    [SerializeField] private AudioSource _takeSound;
    [SerializeField] private AudioSource _dropSound;

    private int _previousCount = 0;
    
    protected override void OnStackCountChanged(int count)
    {
        if (count > _previousCount)
        {
            _takeSound.Play();
        }
        else
        {
            _dropSound.Play();
        }
    }
}
