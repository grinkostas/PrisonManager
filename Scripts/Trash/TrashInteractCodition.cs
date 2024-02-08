using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;
using Zenject;

public class TrashInteractCodition : InteractCondition
{
    [Inject] private Stack _stack;
    public override bool CanInteract(InteractableCharacter character)
    {
        return _stack.ItemsCount == 0;
    }
}
