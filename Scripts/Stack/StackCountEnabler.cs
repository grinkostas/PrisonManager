using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackCountEnabler : StackCountChangeListner
{
    [SerializeField] private bool _hideOnEmpty;
    [SerializeField] private List<GameObject> _gameObjects;
    
    protected override void OnStackCountChanged(int count)
    {
        if (count == 0)
        {
            ChangeActive(!_hideOnEmpty);
        }
        else
        {
            ChangeActive(_hideOnEmpty);
        }
    }

    private void ChangeActive(bool active)
    {
        foreach (var objectToHide in _gameObjects)
        {
            if(objectToHide.activeSelf != active)
                objectToHide.SetActive(active);
        }
    }
}
