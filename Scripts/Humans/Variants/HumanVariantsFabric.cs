using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanVariantsFabric : MonoBehaviour
{
    [SerializeField] private List<HumanVariant> _humanVariantsPrefab;
    [SerializeField] private Transform _spawnTransform;

    public HumanVariant Get()
    {
        HumanVariant prefab = _humanVariantsPrefab[Random.Range(0, _humanVariantsPrefab.Count)];
        HumanVariant result = Instantiate(prefab, _spawnTransform);
        return result;
    }
}
