using UnityEngine;
using UnityEngine.Events;

public class PelletMediator : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int> OnPelletPickedUp;
    
    private int _pointAmount;

    public void AddScore(int amount) // Function is public because it needs to be called from a Unity Event
    {
        _pointAmount += amount;
        OnPelletPickedUp.Invoke(amount);
        Debug.Log(_pointAmount);
    }
}
