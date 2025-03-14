using UnityEngine;
using UnityEngine.Events;

public class PelletMediator : MonoBehaviour
{
    public UnityEvent<Pellet> OnPelletPickedUp;
    
    private int _pointAmount;

    public void PelletPickedUp(Pellet pellet) // Function is public because it needs to be called from a Unity Event
    {
        _pointAmount += pellet.Score;
        OnPelletPickedUp.Invoke(pellet);
    }
}
