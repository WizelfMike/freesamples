using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacterTypes CharacterType;
    [Description("Broadcasts when an activation change took place, the boolean argument is the current active state")]
    public UnityEvent<bool> OnActivationChange;

    public PlayerCharacterTypes Type => CharacterType;

    private bool _isPlayerControlled = false;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public bool PlayerDeactivate()
    {
        if (!_isPlayerControlled)
            return false;
        
        _playerInput.DeactivateInput();
        _isPlayerControlled = false;
        
        // TODO! Placeholder code is here for now,
        // Fill up with Behaviour-AI to take over control.
        
        Debug.Log($"Player character `{gameObject.name}` got deactivated, AI is going to take over control");
        OnActivationChange.Invoke(_isPlayerControlled);
        return true;
    }

    public bool PlayerActivate()
    {
        if (_isPlayerControlled)
            return false;
        
        _playerInput.ActivateInput();
        _isPlayerControlled = true;
        
        // TODO! Placeholder code is here for now,
        // Fill up with code to deactivate the Behaviour-AI so the player can take back control.
        Debug.Log($"Player character `{gameObject.name}` got activated, the player is granted back control");
        OnActivationChange.Invoke(_isPlayerControlled);
        return true;
    }
}