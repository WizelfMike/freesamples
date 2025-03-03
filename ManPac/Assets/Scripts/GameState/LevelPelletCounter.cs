using UnityEngine;
using UnityEngine.Events;

public class LevelPelletCounter : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnNoPellets;
    
    private int _levelPelletCount;

    private void Awake()
    {
        _levelPelletCount = FindObjectsByType<Pellet>(FindObjectsSortMode.None).Length;
    }

    public void ListenToPelletPickup(int pointAmount)
    {
        _levelPelletCount = Mathf.Max(_levelPelletCount - 1, 0);
        if (_levelPelletCount <= 0)
            OnNoPellets.Invoke();
    }
}
