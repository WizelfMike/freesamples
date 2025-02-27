using TMPro;
using UnityEngine;

public class UIGameOverMenu : MonoBehaviour
{
    [Header("Level Names")]
    [SerializeField]
    private string MainMenuLevelName;
    [SerializeField]
    private string GameLevelName;
    
    [Header("Conditional Text")]
    [SerializeField]
    private TextMeshProUGUI TextField;
    [SerializeField]
    private string WinText;
    [SerializeField]
    private string LoseText;
    
    private void Awake()
    {
        GameOverCheck gameOverCheck = FindAnyObjectByType<GameOverCheck>();
        TextField.text = gameOverCheck.CurrentState == GameOverState.PlayerWon ? WinText : LoseText;
    }

    public void OnRetryClicked()
    {
        LevelNavigation.LoadScene(LevelNavigation.GetPreviousScene());
    }

    public void OnMainMenuClicked()
    {
        LevelNavigation.LoadScene(MainMenuLevelName);
    }

    public void OnQuitGameClicked()
    {
        Application.Quit(0);
    }
}
