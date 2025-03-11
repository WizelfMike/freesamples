using TMPro;
using UnityEngine;

public class UIGameOverMenu : MonoBehaviour
{
    [Header("Level Names")]
    [SerializeField]
    private string MainMenuLevelName;
    [SerializeField]
    private string GameLevelName;
    
    // [Header("Conditional Text")]
    // [SerializeField]
    // private TextMeshProUGUI TextField;
    // [SerializeField]
    // private string WinText;
    // [SerializeField]
    // private string LoseText;
    [Header("Conditional display")]
    [SerializeField]
    private RectTransform WinContainer;
    [SerializeField]
    private RectTransform LoseContainer;
    [SerializeField]
    private RectTransform ButtonsContainer;
    [SerializeField]
    private Vector3 WinPosition;
    [SerializeField]
    private Vector3 LosePosition;
    
    private void Awake()
    {
        // GameOverCheck gameOverCheck = FindAnyObjectByType<GameOverCheck>();
        // TextField.text = gameOverCheck.CurrentState == GameOverState.PlayerWon ? WinText : LoseText;
        GameOverCheck gameOverCheck = FindAnyObjectByType<GameOverCheck>();
        bool playerWon = gameOverCheck.CurrentState == GameOverState.PlayerWon;
        RectTransform activeContainer =
            playerWon ? WinContainer : LoseContainer;
        activeContainer.gameObject.SetActive(true);
        
        ButtonsContainer.SetLocalPositionAndRotation(playerWon ? WinPosition : LosePosition, Quaternion.identity);
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
