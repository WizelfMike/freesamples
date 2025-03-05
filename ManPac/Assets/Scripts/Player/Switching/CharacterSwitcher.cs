using System;
using System.Linq;
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

    private PlayerCharacter _currentPlayerActiveCharacter = null;
    private int _characterTypeCount = 0;

    private void Start()
    {
        _characterTypeCount = Enum.GetValues(typeof(PlayerCharacterTypes)).Length;
        PrepareAllCharacters(StartCharacter);
    }

    public void InputSwitchNextPlayer(InputAction.CallbackContext context)
    {
        if (_currentPlayerActiveCharacter == null)
            return;

        int typeIndex = (int) _currentPlayerActiveCharacter.Type;
        int nextIndex = (typeIndex + 1) % _characterTypeCount;
        
        PrepareNextCharacter((PlayerCharacterTypes) nextIndex);
    }

    private PlayerCharacter PrepareAllCharacters(PlayerCharacterTypes type)
    {
        int characterCount = PlayerCharacters.Length;
        PlayerCharacter activated = default;
        for (int i = 0; i < characterCount; i++)
        {
            PlayerCharacter playerCharacter = PlayerCharacters[i];
            if (playerCharacter.Type == type)
            {
                playerCharacter.PlayerActivate();
                activated = playerCharacter;
                continue;
            }

            playerCharacter.PlayerDeactivate();
        }

        _currentPlayerActiveCharacter = activated;
        OnCharacterActivated.Invoke(activated);
        return activated;
    }

    private PlayerCharacter PrepareNextCharacter(PlayerCharacterTypes type)
    {
        if (_currentPlayerActiveCharacter.Type == type)
            return _currentPlayerActiveCharacter;
        
        PlayerCharacter next = PlayerCharacters.FirstOrDefault(x => x.Type == type);
        if (next == null)
            return _currentPlayerActiveCharacter;
        
        
        _currentPlayerActiveCharacter.PlayerDeactivate();
        _currentPlayerActiveCharacter = next;
        next.PlayerActivate();
        
        OnCharacterActivated.Invoke(_currentPlayerActiveCharacter);
        
        return _currentPlayerActiveCharacter;
    }
}
