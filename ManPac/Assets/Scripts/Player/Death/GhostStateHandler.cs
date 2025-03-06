using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(DeathHandler))]
public class GhostStateHandler : MonoBehaviour
{
    private DeathHandler _thisDeathHandler;

    private void Start()
    {
        _thisDeathHandler = GetComponent<DeathHandler>();
    }

    public void ChangeState(ManPacStates currentState)
    {
        _thisDeathHandler.IsInDanger = currentState == ManPacStates.Aggressive;
    }
}
