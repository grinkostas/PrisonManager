using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Visualization : MonoBehaviour
{
    public abstract void Visualize();

    public virtual void Dispose()
    {
    }
}
