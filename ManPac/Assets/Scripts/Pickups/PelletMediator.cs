using UnityEngine;
using UnityEngine.Events;

public class PelletMediator : MonoBehaviour
{
    public UnityEvent<int, PelletTypes> OnPelletPickedUp;
    
    private int _pointAmount;

    public void PelletPickedUp(int pelletScore, PelletTypes pelletType) // Function is public because it needs to be called from a Unity Event
    {
        _pointAmount += pelletScore;
        OnPelletPickedUp.Invoke(pelletScore, pelletType);
    }
}
