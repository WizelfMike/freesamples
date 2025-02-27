using UnityEngine;

public class UIStartMenu : MonoBehaviour
{
    [SerializeField]
    private string LevelName;
    
    public void OnStartClicked()
    {
        LevelNavigation.LoadScene(LevelName);
    }

    public void OnExitClicked()
    {
        Application.Quit(0);
    }
}