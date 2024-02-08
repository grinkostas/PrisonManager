using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TrashFragment
{
    public List<GameObject> TrashVariations;
    public Transform CenterPoint;
    public Vector2 Offset;
    public Vector2 RotationRange;

    private void Init()
    {
        foreach (var trashVariation in TrashVariations)
        {
            trashVariation.SetActive(false);
        }
    }
    
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(Vector3.up * Random.Range(RotationRange.Min(), RotationRange.Max()));
    }

    public Vector3 GetPosition()
    {
        return new Vector3(Random.Range(-Offset.x, Offset.x), 0, Random.Range(-Offset.y, Offset.y));
    }

    public GameObject GetVariation()
    {
        return TrashVariations[Random.Range(0, TrashVariations.Count)];
    }

    public GameObject Spawn()
    {
        Init();
        var variant = GetVariation();
        variant.transform.SetParent(CenterPoint);
        variant.transform.localPosition = GetPosition();
        variant.transform.rotation = GetRotation();
        variant.SetActive(true);
        return variant;
    }
    
}
