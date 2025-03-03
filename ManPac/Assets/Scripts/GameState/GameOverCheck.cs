using UnityEngine;
using UnityEngine.Events;

public class GameOverCheck : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<GameOverState> OnGameOver;
    
    private GameOverState _currentState = GameOverState.Playing;
    
    public GameOverState CurrentState => _currentState;
    
    public void OnNoPellets()
    {
        if (_currentState != GameOverState.Playing)
            return;
        
        _currentState = GameOverState.PlayerLost;
        OnGameOver.Invoke(_currentState);
    }

    public void OnManPacGotHit(GameObject player)
    {
        if (_currentState != GameOverState.Playing)
            return;
        
        _currentState = GameOverState.PlayerWon;
        OnGameOver.Invoke(_currentState);
    }
}