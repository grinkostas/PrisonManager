using System;
using UnityEngine;
using UnityEngine.Events;


public class Human : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private NavMeshAgentHandler _agentHandler;
    [SerializeField] private Criminal _criminal;
    [SerializeField] private Prisoner _prisoner;
    [SerializeField] private Scannable _scannable;
    [SerializeField] private string _id;
    [SerializeField] private HumanVariantsFabric _humanVariantsFabric;
    private HumanVariant _currentVariant;

    public string Id => _id;
    public Renderer Renderer => _currentVariant.Renderer;
    public Animator Animator => _currentVariant.Animator;

    public HumanVariant Variant => _currentVariant;
    public Criminal Criminal => _criminal;
    public Prisoner Prisoner => _prisoner;
    public Scannable Scannable => _scannable;
    public NavMeshAgentHandler AgentHandler => _agentHandler;
    public int Level => _level;

    private void Awake()
    {
        _currentVariant = _humanVariantsFabric.Get();
    }

    public void DressRobe() => _currentVariant.DressRobe(_level);
}
