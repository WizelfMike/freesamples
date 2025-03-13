using UnityEngine;

public class GameOverAudioHandler : MonoBehaviour
{
    private void Awake()
    {
        GameOverState currentState = FindAnyObjectByType<GameOverCheck>().CurrentState;
        PlayGameOverSound(currentState);
    }

    private void PlayGameOverSound(GameOverState state)
    {
        switch (state)
        {
            case GameOverState.PlayerWon:
                FMODUnity.RuntimeManager.PlayOneShot("event:/GhostCheer");
                break;
            case GameOverState.PlayerLost:
                FMODUnity.RuntimeManager.PlayOneShot("event:/DivaLaugh");
                break;

        }
    }
}
