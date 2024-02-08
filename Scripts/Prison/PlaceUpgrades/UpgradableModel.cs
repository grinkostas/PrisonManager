using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class UpgradableModel : Upgradable
{
    [SerializeField] private List<PlaceUpgrade> _placeUpgrades;
    [SerializeField] private bool _instantiate;
    [SerializeField] private float _zoomDuration;


    public override void Upgrade(int level)
    {
        if(level > _placeUpgrades.Max(x=>x.Level)) 
            return;
        
        foreach (var place in _placeUpgrades)
        {
            if (place.Level != level)
                place.Disable();
            else
                place.Enable(_zoomDuration);
        }
        
    }

    [System.Serializable]
    private class PlaceUpgrade
    {
        public int Level;
        public GameObject Model;
        
        public void Enable(float zoomDuration = 0.0f)
        {
            if (Model == null)
                return;
            
            Model.SetActive(true);
            ZoomIn(zoomDuration);
        }

        public void Spawn(Transform transform, float zoomDuration = 0.0f)
        {
            if(Model == null)
                return;
            Model = Instantiate(Model, transform);
            Model.SetActive(true);
            ZoomIn(zoomDuration);
        }


        public void Disable()
        {
            if (Model == null)
                return;
            Model.gameObject.SetActive(false);
        }

        public void Disable(float zoomDuration)
        {
            if(Model == null)
                return;
            if(Model.gameObject.activeSelf == false)
                return;
            Model.transform.DOScale(Vector3.zero, zoomDuration)
                .OnComplete(() => Model.SetActive(false));
        }

        private void ZoomIn(float zoomDuration)
        {
            Vector3 startScale = Model.transform.localScale;
            Model.transform.localScale = Vector3.zero;
            Model.transform.DOScale(Vector3.one, zoomDuration).SetEase(Ease.OutBack);
        }
    }
}
