using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class View : MonoBehaviour
{
    public abstract bool IsHidden { get; }
    public abstract void Show();
    public abstract void Hide();
}
