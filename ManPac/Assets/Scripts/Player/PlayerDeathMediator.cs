using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathMediator : MonoBehaviour
{
    [SerializeField]
    public UnityEvent<PlayerCharacter, bool> OnDeathStateChanged;

    public void OnCharacterDied(PlayerCharacter playerCharacter)
    {
        OnDeathStateChanged.Invoke(playerCharacter, true);
    }

    public void OnCharacterRevived(PlayerCharacter playerCharacter)
    {
        OnDeathStateChanged.Invoke(playerCharacter, false);
    }
}