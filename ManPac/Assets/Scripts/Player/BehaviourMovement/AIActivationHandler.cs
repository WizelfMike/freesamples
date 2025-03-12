using System;
using UnityEngine;
using Unity.Behavior;

[RequireComponent(typeof(BehaviorGraphAgent))]
public class AIActivationHandler : MonoBehaviour
{
    private BehaviorGraphAgent _graphAgent;

    private void Awake()
    {
        _graphAgent = GetComponent<BehaviorGraphAgent>();
    }

    public void OnCharacterActivationChange(bool currentActivation)
    {
        _graphAgent.enabled = !currentActivation;
    }
}
