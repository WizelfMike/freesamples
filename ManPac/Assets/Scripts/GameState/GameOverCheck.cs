using UnityEngine;
using UnityEngine.Events;

public class GameOverCheck : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<GameOverState> OnGameOver;
    
    private GameOverState _currentState = GameOverState.Playing;
    
    public void OnNoPellets()
    {
        Debug.Log("You Lost!");
        _currentState = GameOverState.PlayerLost;
        OnGameOver.Invoke(_currentState);
    }

    public void OnManPacGotHit(GameObject player)
    {
        Debug.Log("You Won!");
        _currentState = GameOverState.PlayerWon;
        OnGameOver.Invoke(_currentState);
    }
}