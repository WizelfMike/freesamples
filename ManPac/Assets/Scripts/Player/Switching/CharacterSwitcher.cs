using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacterTypes StartCharacter = PlayerCharacterTypes.Pinky;
    [SerializeField]
    private PlayerCharacter[] PlayerCharacters;
    
    [Header("Events")]
    public UnityEvent<PlayerCharacter> OnCharacterActivated;

    private int _currentActivePlayerIndex = -1;
    
    public PlayerCharacter CurrentActivePlayer => PlayerCharacters[_currentActivePlayerIndex];

    private void Start()
    {
        PrepareAllCharacters(StartCharacter);
    }

    public void InputSwitchNextPlayer(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed || _currentActivePlayerIndex == -1)
            return;

        int nextIndex = (_currentActivePlayerIndex + 1) % PlayerCharacters.Length;
        PrepareNextCharacter(PlayerCharacters[nextIndex]);
    }

    private PlayerCharacter PrepareAllCharacters(PlayerCharacterTypes type)
    {
        int characterCount = PlayerCharacters.Length;
        int activated = default;
        for (int i = 0; i < characterCount; i++)
        {
            PlayerCharacter playerCharacter = PlayerCharacters[i];
            if (playerCharacter.Type == type)
            {
                playerCharacter.PlayerActivate();
                activated = i;
                continue;
            }

            playerCharacter.PlayerDeactivate();
        }

        _currentActivePlayerIndex = activated;
        OnCharacterActivated.Invoke(PlayerCharacters[activated]);
        return PlayerCharacters[activated];
    }

    private PlayerCharacter PrepareNextCharacter(PlayerCharacter next)
    {
        if (next == CurrentActivePlayer)
            return CurrentActivePlayer;
        
        CurrentActivePlayer.PlayerDeactivate();
        _currentActivePlayerIndex = Array.IndexOf(PlayerCharacters, next);
        next.PlayerActivate();
        
        OnCharacterActivated.Invoke(CurrentActivePlayer);
        return CurrentActivePlayer;
    }
}
