using UnityEngine;

public class GameOverListener : MonoBehaviour
{
    [SerializeField]
    private string GameOverSceneName;

    public void OnGameOver(GameOverState state)
    {
        LevelNavigation.LoadSceneSync(GameOverSceneName);
    }
}