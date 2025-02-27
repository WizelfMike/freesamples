using UnityEngine;

public class PersistentGameState : MonoBehaviour
{
    [SerializeField]
    private bool OverridePrevious = false;
    
    private static PersistentGameState _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            if (OverridePrevious)
            {
                Destroy(_instance.gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }
}
