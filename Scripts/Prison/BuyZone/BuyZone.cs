using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HomaGames.HomaBelly;
using StaserSDK.Interactable;
using UnityEngine.Events;
using Zenject;

public class BuyZone : MonoBehaviour
{
    [SerializeField] private PlayerZone _playerZone;
    [SerializeField] private string _id;
    [SerializeField] private float _price;
    [SerializeField] private float _buyTime;
    [SerializeField] private List<GameObject> _objectsToActivat;
    [SerializeField] private List<GameObject> _objectsToHide;
    [SerializeField] private List<BuyZone> _buyZonesToHide;
    [SerializeField] private View _zoneView;
    [SerializeField] private bool _isBought;
    [SerializeField] private bool _debugMode;
    [SerializeField] private bool _sendAnalytics;

    [Inject] private Balance _balance;
    
    private BuyZoneSaveModel _saveModel = new BuyZoneSaveModel();
    
    public UnityAction Bought;
    public UnityAction<float> BuyProgressChanged;

    public float Price => _price;
    public float Spend => _saveModel.BuyProgress;
    public bool IsBought => _isBought || (_saveModel.IsBought || _saveModel.BuyProgress >= _price);
    public PlayerZone Zone => _playerZone;
    
    private void OnEnable()
    {
        _saveModel = ES3.Load(_id, new BuyZoneSaveModel());
        if(IsBought && (_debugMode == false || _isBought))    
        {
            Buy();
            return;
        }
        
        Hide();
        
        _playerZone.OnInteract += StartBuying;
    }

    private void OnDisable()
    {
        if(IsBought == false)
            Hide();
        _playerZone.OnInteract -= StartBuying;
    }

    private void Start()
    {
        BuyProgressChanged?.Invoke( _saveModel.BuyProgress);
    }

    private void StartBuying(InteractableCharacter character)
    {
        StartCoroutine(Buying());
    }

    private IEnumerator Buying()
    {
        float moneyPerSecond = _price / _buyTime;
        while (_playerZone.IsCharacterInside && _saveModel.BuyProgress < _price)
        {
            float moneyPerFrame = moneyPerSecond * Time.deltaTime;
            
            if (moneyPerFrame > _balance.RealAmount && _balance.RealAmount > 0.0f) 
                moneyPerFrame = _balance.RealAmount;
            
            if(_balance.TrySpend(moneyPerFrame, false) == false)
                break;
            
            _saveModel.BuyProgress += moneyPerFrame;
            BuyProgressChanged?.Invoke( _saveModel.BuyProgress);
            if (_saveModel.BuyProgress >= _price)
                break;
            yield return null;
        }
        _balance.Save();
        Save();
        if (_price - _saveModel.BuyProgress < 1 )
            Buy();
        
    }

    private void Save()
    {
        if(_debugMode == false)
            ES3.Save(_id, _saveModel);
    }
    
    private void Buy()
    {
        Show();
        _saveModel.IsBought = true;
        Save();
        Bought?.Invoke();
        _zoneView.Hide();
        if(_sendAnalytics)
            DefaultAnalytics.LevelStarted(_id);
    }

    private void Show()
    {
        foreach (var objectToActivate in _objectsToActivat)
        {
            objectToActivate.SetActive(true);
        }

        foreach (var objectToHide in _objectsToHide)
        {
            objectToHide.SetActive(false);
        }

        foreach (var buyZone in _buyZonesToHide)
        {
            buyZone.gameObject.SetActive(true);
        }
    }
    
    private void Hide()
    {
        foreach (var buyZone in _buyZonesToHide)
        {
            buyZone.Disable();
        }
        foreach (var objectToActivate in _objectsToActivat)
        {
            objectToActivate.SetActive(false);
        }
    }

    public void Disable()
    {
        Hide();
        gameObject.SetActive(false);
    }
    
    
}
