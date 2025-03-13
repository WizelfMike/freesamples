using System;
using TMPro;
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
    [SerializeField]
    private float SwitchTimeout;
    
    [Header("Events")]
    public UnityEvent<PlayerCharacter> OnCharacterActivated;
    
    public PlayerCharacter CurrentActivePlayer => PlayerCharacters[_currentActivePlayerIndex];

    private int _currentActivePlayerIndex = -1;
    private DeathHandler[] _deathHandlers;
    private DeltaTimer _switchTimer;

    private void Awake()
    {
        _switchTimer = new DeltaTimer(SwitchTimeout);
    }

    private void Start()
    {
        int playerCharacterCount = PlayerCharacters.Length;
        _deathHandlers = new DeathHandler[playerCharacterCount];
        for (int i = 0; i < playerCharacterCount; i++)
            _deathHandlers[i] = PlayerCharacters[i].GetComponent<DeathHandler>();
        
        PrepareAllCharacters(StartCharacter);
    }

    private void Update()
    {
        if (_switchTimer.IsRunning)
            _switchTimer.Update(Time.deltaTime);
    }

    public void InputSwitchNextPlayer(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed || _currentActivePlayerIndex == -1 || _switchTimer.IsRunning)
            return;
        
        _switchTimer.Reset();
        CycleNextPlayer();
    }

    public void OnPlayerCharacterDied(PlayerCharacter playerCharacter, bool isDead)
    {
        if (!isDead || playerCharacter.Type != CurrentActivePlayer.Type)
            return;
        
        CycleNextPlayer();
    }

    private void CycleNextPlayer()
    {
        int nextIndex = (_currentActivePlayerIndex + 1) % PlayerCharacters.Length;
        int maxTries = PlayerCharacters.Length - 1;
        
        while (_deathHandlers[nextIndex].IsDead)
        {
            if (maxTries <= 0)
                break;
            
            nextIndex = (nextIndex + 1) % PlayerCharacters.Length;
            maxTries -= 1;
        }

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
