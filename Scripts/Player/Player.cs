using StaserSDK.Interactable;
using UnityEngine;


public class Player : InteractableCharacter
{
    [SerializeField] private Transform _body;
    [SerializeField] private Pointer _pointer;

    public Pointer Pointer => _pointer;

    public Transform Body => _body;
}
