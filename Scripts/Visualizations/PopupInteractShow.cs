using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;
using StaserSDK.Views;

public class PopupInteractShow : Popup
{
    [SerializeField] private ZoneBase _zone;

    private void OnEnable()
    {
        _zone.OnInteract += Show;
        _zone.OnExit += Hide;
    }

    private void OnDisable()
    {
        _zone.OnInteract -= Show;
        _zone.OnExit -= Hide;
    }

    private void Show(InteractableCharacter character)
    {
        Debug.Log("Show");
        Show();
    } 

    private void Hide(InteractableCharacter character)
    {
        Debug.Log("Hide");
        Hide(); 
    }
}
