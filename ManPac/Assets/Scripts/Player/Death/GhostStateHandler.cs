using UnityEngine;
using UnityEngine.Events;

public class GhostStateHandler : MonoBehaviour
{
    private DeathHandler thisDeathHandler;
    private void Start()
    {
        thisDeathHandler = GetComponent<DeathHandler>();
    }

    public void ChangeToScaredState(ManPacStates currentState)
    {
        if (currentState == ManPacStates.Aggressive)
        {
            thisDeathHandler.IsInDanger = true;
        }

        if (currentState == ManPacStates.Avoidant)
        {
            thisDeathHandler.IsInDanger = false;
        }
    }
}
