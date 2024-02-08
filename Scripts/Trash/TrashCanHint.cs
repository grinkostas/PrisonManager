using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class TrashCanHint : StackCountChangeListner
{
    [Inject] private Player _player;
    
    protected override void OnStackCountChanged(int count)
    {
        if (count == 0)
        {
            _player.Pointer.ReceiveDestination(transform);
        }
        else
        {
            _player.Pointer.SetDestination(transform);
        }
        
    }
}
