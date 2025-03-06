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

    public void ChangeToScaredState(ManPacStates currentState)
    {
        if (currentState == ManPacStates.Aggressive)
        {
            _thisDeathHandler.IsInDanger = true;
        }

        if (currentState == ManPacStates.Avoidant)
        {
            _thisDeathHandler.IsInDanger = false;
        }
    }
}
