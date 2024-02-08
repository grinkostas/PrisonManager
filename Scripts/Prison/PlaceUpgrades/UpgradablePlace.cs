using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradablePlace : MonoBehaviour
{
    [SerializeField] private List<PlaceUpgrade> _placeUpgrades;

    private int _currentLevel = 0;
    private PlaceUpgrade _currentUpgrade;

    public void Upgrade()
    {
        _currentLevel++;
        if (_currentUpgrade != null)
        {
            _currentUpgrade.Disable();
        }
        
        _currentUpgrade = _placeUpgrades.Find(x => x.Level == _currentLevel);
        _currentUpgrade.Enable();
    }

    [System.Serializable]
    private class PlaceUpgrade
    {
        public int Level;
        public GameObject Model;

        public void Enable()
        {
            if(Model == null)
                return;
            Model.SetActive(true);
        }

        public void Disable()
        {
            if(Model == null)
                return;;
            Model.SetActive(false);
        }
    }
}
