using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanVariant : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Transform _model;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private List<Renderer> _dressVariants;
    [SerializeField] private List<Material> _skinMaterials;
    [SerializeField] private List<RobeSetting> _robes;
    [SerializeField] private Outline _outline;
   

    private Renderer _currentDress;
    private GameObject _robe;
    public Animator Animator => _animator;
    public Transform LeftHand => _leftHand;
    public Transform RightHand => _rightHand;
    public Renderer Renderer => _renderer;
    public Renderer Dress => _currentDress;
    public Transform Model => _model;
    public Outline Outline => _outline;

    private void OnEnable()
    {
        _currentDress = _dressVariants[Random.Range(0, _dressVariants.Count)];
        _renderer.material = _skinMaterials[Random.Range(0, _skinMaterials.Count)];
        _currentDress.gameObject.SetActive(true);
    }

    public void DressRobe(int roomLevel)
    {
        _currentDress.gameObject.SetActive(false);
        if (_robes.Count == 1)
        {
            _robes[0].Enable();
            _robe = _robes[0].Robe;
            return;
        }

        var robe = _robes.Find(x => x.Level == roomLevel);
        robe.Enable();
        _robe = robe.Robe;
    }

    public void DressCasualClothing()
    {
        if(_robe != null)
            _robe.SetActive(false);
        _currentDress.gameObject.SetActive(true);
    }

    public void RemoveClothing()
    {
        _robe.SetActive(false);   
    }

    public void DressClothing()
    {
        _robe.SetActive(true); 
    }
    
    [System.Serializable]
    private class RobeSetting
    {
        public int Level;
        public GameObject Robe;

        public void Enable()
        {
            Robe.SetActive(true);
        }

        public void Disable()
        {
            Robe.SetActive(false);
        }
    }
}
