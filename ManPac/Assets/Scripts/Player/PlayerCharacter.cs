using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacterTypes CharacterType;
    [SerializeField]
    private PlayerIndicator Indicator;
    [Description("Broadcasts when an activation change took place, the boolean argument is the current active state")]
    public UnityEvent<bool> OnActivationChange;

    public PlayerCharacterTypes Type => CharacterType;

    private bool _isPlayerControlled = true;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void PlayerDeactivate()
    {
        _playerInput.enabled = false;
        _isPlayerControlled = false;
        
        // TODO! Placeholder code is here for now,
        // Fill up with Behaviour-AI to take over control.
        
        Indicator.enabled = false;
        OnActivationChange.Invoke(_isPlayerControlled);
    }

    public void PlayerActivate()
    {
        _playerInput.enabled = true;
        _isPlayerControlled = true;
        
        // TODO! Placeholder code is here for now,
        // Fill up with code to deactivate the Behaviour-AI so the player can take back control.
        
        Indicator.enabled = true;
        OnActivationChange.Invoke(_isPlayerControlled);
    }
}